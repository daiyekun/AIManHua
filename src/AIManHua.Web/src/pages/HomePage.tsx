import { useState } from "react";
import AuthModal from "../components/AuthModal/AuthModal";
import "./HomePage.css";

const features = [
  { icon: "✨", title: "提示词生图", desc: "支持中/英文提示词，自定义画风、尺寸、风格" },
  { icon: "🎬", title: "智能漫画", desc: "四格漫画、长篇条漫、剧情分镜自动生成" },
  { icon: "🎨", title: "风格模板", desc: "日系、国漫、Q版、写实、古风等百种预设风格" },
  { icon: "☁️", title: "作品管理", desc: "云端保存、二次编辑、下载、分享作品" },
];

const galleryItems = [
  { img: "https://placehold.co/400x500/EDEDFF/5B5FFF?text=四格漫画", tag: "四格漫画" },
  { img: "https://placehold.co/400x320/E8F4FD/5B5FFF?text=角色立绘", tag: "角色立绘" },
  { img: "https://placehold.co/400x400/FFF0ED/FF7A50?text=剧情条漫", tag: "剧情条漫" },
  { img: "https://placehold.co/400x360/F0FFED/36D399?text=日系插画", tag: "日系插画" },
  { img: "https://placehold.co/400x480/F5F0FF/5B5FFF?text=古风插画", tag: "古风插画" },
  { img: "https://placehold.co/400x340/FFF5E6/FF7A50?text=Q版角色", tag: "Q版角色" },
];

const steps = [
  { step: "01", icon: "🔑", title: "登录 / 注册账号", desc: "免费注册，即刻开启 AI 创作之旅" },
  { step: "02", icon: "💡", title: "输入创意提示词，选择风格", desc: "描述你想要的画面，选择喜欢的风格模板" },
  { step: "03", icon: "🚀", title: "一键生成，下载使用", desc: "AI 自动生成高清图片或漫画，支持二次编辑" },
];

export default function HomePage() {
  const [authOpen, setAuthOpen] = useState(false);
  const [authTab, setAuthTab] = useState<"login" | "register">("login");

  const openAuth = (tab: "login" | "register") => { setAuthTab(tab); setAuthOpen(true); };

  return (
    <div className="homepage">

      {/* ═══════════ 1. 导航栏 ═══════════ */}
      <nav className="navbar">
        <div className="container navbar-inner">
          <a href="/" className="logo">
            <span className="logo-icon">🎨</span>
            <span className="logo-text">AI漫图生成</span>
          </a>
          <div className="nav-menu">
            <a href="#">首页</a>
            <a href="#">作品广场</a>
            <a href="#">创作教程</a>
            <a href="#">会员中心</a>
            <a href="#">帮助中心</a>
          </div>
          <div className="nav-actions">
            <button className="btn btn-outline btn-sm" onClick={() => openAuth("login")}>登录</button>
            <button className="btn btn-primary btn-sm" onClick={() => openAuth("register")}>注册</button>
          </div>
          <button className="menu-toggle" aria-label="菜单">&#9776;</button>
        </div>
      </nav>

      {/* ═══════════ 2. 英雄区 ═══════════ */}
      <section className="hero">
        <div className="container hero-grid">
          <div className="hero-text">
            <div className="hero-badge-row">
              <span className="hero-badge">免费试用</span>
              <span className="hero-badge">高清出图</span>
              <span className="hero-badge">海量风格</span>
              <span className="hero-badge">极速生成</span>
            </div>
            <h1 className="hero-title">AI 智能生成漫画/图片<br /><span className="hero-highlight">一句话创作专属作品</span></h1>
            <p className="hero-desc">输入提示词，快速生成二次元插画、条漫、四格漫画、角色立绘</p>
            <div className="hero-actions">
              <button className="btn btn-primary btn-lg" onClick={() => openAuth("register")}>立即开始创作</button>
              <button className="btn btn-outline btn-lg" onClick={() => openAuth("register")}>免费注册账号</button>
            </div>
            <p className="hero-link">已有账号？<button className="link-btn" onClick={() => openAuth("login")}>点击登录</button></p>
          </div>
          <div className="hero-visual">
            <div className="hero-carousel">
              <div className="carousel-card card-1">四格漫画生成</div>
              <div className="carousel-card card-2">角色立绘生成</div>
              <div className="carousel-card card-3">剧情条漫生成</div>
            </div>
            <div className="hero-mock">
              <div className="mock-input">💬 输入你的创意提示词...</div>
              <div className="mock-result">✨ AI 生成结果预览</div>
            </div>
          </div>
        </div>
      </section>

      {/* ═══════════ 3. 功能介绍 ═══════════ */}
      <section className="features section">
        <div className="container">
          <h2 className="section-title">核心功能</h2>
          <p className="section-subtitle">强大的 AI 能力，让漫画创作变得简单高效</p>
          <div className="features-grid">
            {features.map((f, i) => (
              <div className="feature-card" key={i}>
                <div className="feature-icon">{f.icon}</div>
                <h3 className="feature-title">{f.title}</h3>
                <p className="feature-desc">{f.desc}</p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* ═══════════ 4. 作品案例 ═══════════ */}
      <section className="gallery section">
        <div className="container">
          <h2 className="section-title">用户热门作品</h2>
          <p className="section-subtitle">看看其他创作者用 AI 漫图生成了什么</p>
          <div className="gallery-grid">
            {galleryItems.map((item, i) => (
              <div className="gallery-item" key={i}>
                <img src={item.img} alt={item.tag} loading="lazy" />
                <div className="gallery-overlay">
                  <span className="gallery-tag">{item.tag}</span>
                  <button className="btn btn-white btn-sm">一键同款</button>
                </div>
              </div>
            ))}
          </div>
          <div className="gallery-more">
            <a href="#" className="btn btn-outline">查看更多作品 &rarr;</a>
          </div>
        </div>
      </section>

      {/* ═══════════ 5. 操作流程 ═══════════ */}
      <section className="steps section">
        <div className="container">
          <h2 className="section-title">3 步快速创作</h2>
          <p className="section-subtitle">无需复杂操作，简单三步即可完成 AI 创作</p>
          <div className="steps-row">
            {steps.map((s, i) => (
              <div className="step-card" key={i}>
                <div className="step-number">{s.step}</div>
                <div className="step-icon">{s.icon}</div>
                <h3 className="step-title">{s.title}</h3>
                <p className="step-desc">{s.desc}</p>
                {i < steps.length - 1 && <div className="step-line" />}
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* ═══════════ 6. 页脚 ═══════════ */}
      <footer className="footer">
        <div className="container footer-inner">
          <div className="footer-brand">
            <span className="logo-text">AI漫图生成</span>
            <p>用 AI 让每个人都能创作漫画</p>
          </div>
          <div className="footer-links">
            <a href="#">用户协议</a>
            <a href="#">隐私政策</a>
            <a href="#">联系我们</a>
            <a href="#">帮助中心</a>
          </div>
          <p className="footer-copy">&copy; 2026 AI漫图生成 版权所有</p>
        </div>
      </footer>

      {/* ── 登录/注册弹窗 ── */}
      <AuthModal isOpen={authOpen} onClose={() => setAuthOpen(false)} initialTab={authTab} />
    </div>
  );
}
