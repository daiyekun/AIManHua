var builder = DistributedApplication.CreateBuilder(args);

// ── Passwords ────────────────────────────────────────────────────

var mysqlPassword = builder.AddParameter("mysql-password", "aimanhua123");
var rabbitMqUser = builder.AddParameter("rabbitmq-user", "guest");
var rabbitMqPassword = builder.AddParameter("rabbitmq-password", "guest");

// ── Infrastructure ──────────────────────────────────────────────

var mysql = builder.AddMySql("mysql", password: mysqlPassword, port: 3306)
    .WithDataVolume("aimanhua-mysql-data")
    .AddDatabase("aimanhua");

var redis = builder.AddRedis("redis", port: 6379)
    .WithDataVolume("aimanhua-redis-data");

var rabbitMq = builder.AddRabbitMQ("rabbitmq",
        userName: rabbitMqUser,
        password: rabbitMqPassword,
        port: 5672)
    .WithManagementPlugin(port: 15672)
    .WithDataVolume("aimanhua-rabbitmq-data");

var minio = builder.AddContainer("minio", "minio/minio")
    .WithImageTag("latest")
    .WithHttpEndpoint(port: 9000, targetPort: 9000)
    .WithHttpEndpoint(port: 9001, targetPort: 9001, name: "console")
    .WithEnvironment("MINIO_ROOT_USER", "minioadmin")
    .WithEnvironment("MINIO_ROOT_PASSWORD", "minioadmin")
    .WithVolume("aimanhua-minio-data", "/data")
    .WithArgs("server", "/data", "--console-address", ":9001");

// ── Services ─────────────────────────────────────────────────────

var apiService = builder.AddProject<Projects.AIManHua_ApiService>("api")
    .WithReference(mysql)
    .WithReference(redis)
    .WithReference(rabbitMq)
    .WithEnvironment("ConnectionStrings__minio", "localhost:9000")
    .WithEnvironment("Minio__Endpoint", "localhost:9000")
    .WithEnvironment("Minio__AccessKey", "minioadmin")
    .WithEnvironment("Minio__SecretKey", "minioadmin")
    .WithEnvironment("ASPIRE_ENVIRONMENT", builder.Environment.EnvironmentName);

var agentService = builder.AddProject<Projects.AIManHua_AgentService>("agent")
    .WithReference(mysql)
    .WithReference(redis)
    .WithReference(rabbitMq)
    .WithEnvironment("ConnectionStrings__minio", "localhost:9000")
    .WithEnvironment("Minio__Endpoint", "localhost:9000")
    .WithEnvironment("Minio__AccessKey", "minioadmin")
    .WithEnvironment("Minio__SecretKey", "minioadmin")
    .WithEnvironment("ASPIRE_ENVIRONMENT", builder.Environment.EnvironmentName);

var web = builder.AddNpmApp("web", "../AIManHua.Web", "dev")
    .WithHttpEndpoint(port: 5173, env: "PORT")
    .WithReference(apiService)
    .WithReference(agentService);

builder.Build().Run();
