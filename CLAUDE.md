# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 项目概述

AI 漫画图片生成智能体系统 —— 根据用户输入的提示词，调用 AI 大模型自动生成漫画分镜、图片，并支持在线编辑台词和排版。全后端分离架构。

## 构建和运行命令

### 后端 (.NET 10)

```bash
# 还原依赖
dotnet restore

# 编译全部项目
dotnet build

# 通过 Aspire AppHost 启动全部服务（推荐开发方式）
dotnet run --project src/AIManHua.AppHost

# 单独启动某个服务
dotnet run --project src/AIManHua.ApiService
dotnet run --project src/AIManHua.AgentService

# EF Core 数据库迁移
dotnet ef migrations add InitialCreate --project src/AIManHua.Infrastructure --startup-project src/AIManHua.ApiService
dotnet ef database update --project src/AIManHua.Infrastructure --startup-project src/AIManHua.ApiService

# 运行测试（测试项目尚未创建测试代码）
dotnet test
```

### 前端 (React)

```bash
cd src/AIManHua.Web
npm install
npm run dev          # 开发服务器，端口 5173，API 代理到 localhost:5000
npm run build        # 生产构建
npm run lint         # ESLint 检查
```

### 基础设施 (Docker)

通过 AppHost 启动时自动拉起，无需手动操作。

```bash
# 启动全部（含基础设施容器 + API + Agent + Web），一站式开发
dotnet run --project src/AIManHua.AppHost

# 或手动使用 docker-compose（仅基础设施 + Celery Worker）
docker-compose up -d
docker-compose up -d mysql redis
```

数据持久化 Docker 卷（删除后数据不丢失）：
- `aimanhua-mysql-data`
- `aimanhua-redis-data`
- `aimanhua-rabbitmq-data`
- `aimanhua-minio-data`

### Python 图像处理 Worker

```bash
cd src/AIManHua.ImageWorker
pip install -r requirements.txt
celery -A celery_app.worker.app worker --loglevel=info --concurrency=4
```

## 技术栈

| 层次 | 技术 | 用途 |
|------|------|------|
| 云原生编排 | .NET Aspire 13.x | 服务编排、配置管理、链路追踪 |
| 后端框架 | .NET 10 + ASP.NET Core | REST API |
| AI 智能体 | Microsoft.SemanticKernel | AI Agent 编排、多模型调用 |
| ORM | EF Core + MySql.EntityFrameworkCore 10.x | 数据持久化 |
| 缓存 | StackExchange.Redis 2.x | 任务状态缓存、热门提示词 |
| 对象存储 | Minio 6.x | 生成图片/漫画存储 |
| 消息队列 | MassTransit 8.x + RabbitMQ | 生成任务异步排队 |
| 图像处理 | Pillow + Pillow-SIMD + OpenCV + MoviePy | 排版、气泡、拼接、滤镜 |
| 异步Worker | Celery 5.x (Python) | 图像处理任务消费 |
| 前端 | React 19 + Vite 6 + Canvas (Fabric.js 6) | 提示词输入、分镜预览、在线编辑 |
| 状态管理 | Zustand 5 | 前端全局状态 |
| 可观测性 | OpenTelemetry (Trace + Metrics) | 分布式链路追踪 |

## 项目结构说明

```
src/
├── AIManHua.AppHost/          # Aspire 编排入口，定义全部服务及依赖关系
├── AIManHua.ServiceDefaults/  # Aspire 共享默认配置：OTel、健康检查、服务发现、弹性
├── AIManHua.ApiService/       # 主 API 服务（端口动态分配），对外暴露 REST 接口
├── AIManHua.AgentService/     # AI Agent 服务（端口动态分配），封装 Semantic Kernel 智能体
├── AIManHua.Domain/           # 领域层：Entity、Enum、Repository 接口
├── AIManHua.Infrastructure/   # 基础设施层：DbContext、仓储实现、外部服务适配器
├── AIManHua.ImageWorker/      # Python Celery Worker：图像后处理（气泡、排版、拼接、滤镜）
└── AIManHua.Web/              # React 前端（独立项目，不在 .slnx 中）
infrastructure/                # Docker 容器配置文件（MySQL init、Redis conf、nginx conf）
```

## 关键设计决策

1. **Aspire 容器编排**：`dotnet run --project src/AIManHua.AppHost` 自动拉起全部基础设施容器（MySQL + Redis + RabbitMQ + MinIO）并注入连接串到 API/Agent 服务，无需手动 docker-compose。Aspire Dashboard 中可点击 Web 链接直达前端页面。

2. **OpenTelemetry 链路追踪**：ServiceDefaults 已配置好 Trace 和 Metrics 的自动采集（AspNetCore + HttpClient + Runtime），通过 `OTEL_EXPORTER_OTLP_ENDPOINT` 环境变量配置导出目标。

3. **MySQL 版本**：使用 Oracle 官方 `MySql.EntityFrameworkCore`（非 Pomelo），因为后者尚未发布 .NET 10 兼容版本。API 调用方式为 `UseMySQL(connectionString)`。

4. **MassTransit + RabbitMQ**：用于 .NET 服务间的异步消息传递（任务状态通知等）。Python Celery 直接连接 RabbitMQ 进行图像处理任务的消费。

5. **前端 Canvas 编辑**：使用 Fabric.js 6.x 实现分镜的拖拽排版、图片缩放旋转、气泡台词叠加等可视化编辑能力。

6. **图像处理管线**：Python Celery Worker 负责图生图的后处理，包含四个处理器模块：`bubble`（气泡）、`layout`（排版）、`stitch`（拼接）、`effects`（滤镜）。

7. **Snowflake ID 生成**：所有实体主键使用 64 位 Snowflake 算法，禁止使用 GUID 和数据库自增。`SnowflakeIdGenerator` 注册为 Singleton，在 Repository 层调用 `NextId()` 赋值。EF Core 配置 `ValueGeneratedNever()` 确保不依赖数据库生成主键。

8. **JWT 认证**：使用 `System.IdentityModel.Tokens.Jwt` + `Microsoft.AspNetCore.Authentication.JwtBearer`。JwtService 生成 Token，AuthController 提供 `/api/auth/register`、`/api/auth/login`、`/api/auth/me` 三个端点。密码使用 BCrypt 哈希。

9. **持久化卷**：全部有状态服务使用 Docker 命名卷（named volumes），容器重启不丢数据。MySQL → `aimanhua-mysql-data`，Redis → `aimanhua-redis-data`，RabbitMQ → `aimanhua-rabbitmq-data`，MinIO → `aimanhua-minio-data`。

## 当前状态

已完成的业务功能：
- 邮箱注册 / 登录 / 获取当前用户（AuthController）
- Snowflake 64位 ID 生成器
- JWT 令牌签发与验证
- 所有实体配置 `ValueGeneratedNever()`，由应用层分配 ID
- EF Core 设计时工厂，支持 `dotnet ef migrations` 命令

尚未编写业务逻辑的功能：
- AI 漫画生成流程（ComicController 为空桩）
- Agent 编排逻辑（ComicGenAgent 为空桩）
- 图像处理管线（Celery tasks / processors 为空桩）
- 前端 Canvas 编辑器交互逻辑
