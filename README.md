# 🚀 JackSite

一个现代化的全栈 Web 应用，采用 Monorepo 架构和微服务体系构建。

## 📁 项目结构

```
JackSite/
├── client/               # 🌐 前端客户端应用
├── server/               # 🔧 后端服务集群
│   ├── apps/
│   │   ├── gateway/      # 🚪 API 网关服务
│   │   └── host/         # 🏠 主机服务
│   ├── docs/             # 📚 后端文档
│   ├── infrastructure/   # 🏗️ 基础设施配置
│   ├── services/         # 🔧 微服务模块
│   │   └── src/
│   └── shared/           # 📦 后端共享代码
├── docker/               # 🐳 Docker 配置
│   └── .dockerignore
├── docs/                 # 📚 项目文档
├── .github/              # 🤖 GitHub Actions 配置
└── scripts/              # 🛠️ 构建和部署脚本
```

## 🏗️ 架构设计

### 微服务架构
- **Gateway Service** - API 网关，处理路由和认证
- **Host Service** - 主机服务，核心业务逻辑
- **Infrastructure** - 基础设施配置和部署
- **Shared** - 跨服务共享代码和工具

### 前后端分离
- **Client** - 前端单页应用
- **Server** - 后端微服务集群

## ⚡ 快速开始

### 环境要求

- Node.js >= 18.0.0
- pnpm >= 8.0.0
- Docker >= 20.0.0
- Docker Compose >= 2.0.0

### 本地开发

```bash
# 克隆项目
git clone https://github.com/IAMWHCZ/JackSite.git
cd JackSite

# 安装依赖
pnpm install

# 启动开发环境（所有服务）
pnpm dev

# 分别启动服务
pnpm dev:client     # 前端: http://localhost:3000
pnpm dev:gateway    # 网关: http://localhost:3001
pnpm dev:host       # 主机: http://localhost:3002
```

### Docker 部署

```bash
# 构建所有服务镜像
docker-compose build

# 启动所有服务
docker-compose up -d

# 查看服务状态
docker-compose ps

# 查看日志
docker-compose logs -f
```

## 📋 可用脚本

```bash
# 开发环境
pnpm dev                  # 启动所有服务
pnpm dev:client           # 仅启动前端
pnpm dev:gateway          # 仅启动网关服务
pnpm dev:host            # 仅启动主机服务

# 构建
pnpm build               # 构建所有服务
pnpm build:client        # 仅构建前端
pnpm build:server        # 仅构建后端服务

# 测试
pnpm test                # 运行所有测试
pnpm test:client         # 仅测试前端
pnpm test:server         # 仅测试后端

# 代码质量
pnpm lint                # 检查所有代码风格
pnpm lint:fix            # 自动修复代码风格
pnpm type-check          # TypeScript 类型检查

# Docker 相关
pnpm docker:build        # 构建 Docker 镜像
pnpm docker:up           # 启动 Docker 容器
pnpm docker:down         # 停止 Docker 容器
```

## 🛠️ 技术栈

### 前端 (Client)
- ⚛️ React 18 + TypeScript
- ⚡ Vite 构建工具
- 🎨 Tailwind CSS / Ant Design
- 📱 响应式设计
- 🔄 React Query 状态管理
- 🧭 React Router 路由

### 后端 (Server)
- 🟢 Node.js + TypeScript
- 🚪 **Gateway Service**: Express/Fastify + 路由网关
- 🏠 **Host Service**: 核心业务逻辑
- 🗄️ 数据库: PostgreSQL/MongoDB
- 🔐 认证: JWT + OAuth2
- 📊 API: RESTful + GraphQL
- 🔄 消息队列: Redis/RabbitMQ

### 基础设施 (Infrastructure)
- 🐳 Docker + Docker Compose
- ☸️ Kubernetes (可选)
- 🔄 CI/CD: GitHub Actions
- 📊 监控: Prometheus + Grafana
- 📝 日志: ELK Stack

### 开发工具
- 📦 pnpm Workspaces
- 🔍 ESLint + Prettier
- 🧪 Jest + Testing Library
- 🚀 GitHub Actions
- 📋 Husky Git Hooks

## 🔧 开发指南

### 添加新的微服务

```bash
# 在 server/services 下创建新服务
mkdir server/services/new-service
cd server/services/new-service

# 初始化服务
pnpm init
```

### 环境变量配置

```bash
# 复制环境变量模板
cp .env.example .env

# 为不同服务配置环境变量
cp server/apps/gateway/.env.example server/apps/gateway/.env
cp server/apps/host/.env.example server/apps/host/.env
```

### 数据库迁移

```bash
# 运行数据库迁移
pnpm migrate:up

# 回滚迁移
pnpm migrate:down

# 生成新的迁移文件
pnpm migrate:generate
```

## 🚀 部署

### 开发环境部署

```bash
# 使用 Docker Compose
docker-compose -f docker-compose.dev.yml up -d
```

### 生产环境部署

```bash
# 构建生产镜像
docker-compose -f docker-compose.prod.yml build

# 部署到生产环境
docker-compose -f docker-compose.prod.yml up -d
```

### 云平台部署

#### Vercel (前端)
```bash
# 安装 Vercel CLI
npm i -g vercel

# 部署前端
cd client
vercel --prod
```

#### AWS/阿里云 (后端)
```bash
# 使用 Kubernetes 部署
kubectl apply -f infrastructure/k8s/
```

## 🏗️ 服务详情

### Gateway Service (网关服务)
- **端口**: 3001
- **功能**: API 路由、认证、限流、日志
- **技术**: Express.js + 中间件

### Host Service (主机服务)
- **端口**: 3002
- **功能**: 核心业务逻辑、数据处理
- **技术**: Fastify + TypeORM

### Client (前端应用)
- **端口**: 3000
- **功能**: 用户界面、交互逻辑
- **技术**: React + Vite

## 📊 监控和日志

### 应用监控
- **健康检查**: `/health` 端点
- **指标收集**: Prometheus
- **可视化**: Grafana 仪表板

### 日志管理
- **结构化日志**: Winston
- **日志聚合**: ELK Stack
- **错误追踪**: Sentry

## 🤝 贡献指南

1. Fork 本仓库
2. 创建特性分支 (`git checkout -b feature/amazing-feature`)
3. 提交更改 (`git commit -m 'feat: add amazing feature'`)
4. 推送到分支 (`git push origin feature/amazing-feature`)
5. 打开 Pull Request

### 提交规范

使用 [Conventional Commits](https://www.conventionalcommits.org/) 规范：

```
feat: 新功能
fix: 修复 bug
docs: 文档更新
style: 代码格式
refactor: 代码重构
test: 测试相关
chore: 构建过程或辅助工具的变动
perf: 性能优化
ci: CI/CD 相关
```

### 代码规范

- 使用 TypeScript 严格模式
- 遵循 ESLint 和 Prettier 配置
- 编写单元测试和集成测试
- 更新相关文档

## 🔒 安全

- JWT Token 认证
- CORS 配置
- 请求限流
- 输入验证
- SQL 注入防护
- XSS 防护

## 📄 许可证

本项目采用 [MIT](LICENSE) 许可证。

## 👨‍💻 作者

**IAMWHCZ**

- GitHub: [@IAMWHCZ](https://github.com/IAMWHCZ)
- Email: c2545481217@gmail.com

## 📞 支持

如果你喜欢这个项目，请给它一个 ⭐！

有问题？请创建一个 [Issue](https://github.com/IAMWHCZ/JackSite/issues)。

## 📚 相关文档

- [API 文档](./docs/api.md)
- [部署指南](./docs/deployment.md)
- [开发指南](./docs/development.md)
- [架构设计](./docs/architecture.md)

---

**[English](./README.en.md)** | 中文