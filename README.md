# 🚀 JackSite

一个现代化的全栈 Web 应用，采用 Monorepo 架构和最新技术栈构建。

## 📁 项目结构

```
JackSite/
├── apps/
│   ├── web/              # 🌐 前端 Web 应用
│   └── api/              # 🔧 后端 API 服务
├── packages/
│   ├── shared/           # 📦 共享工具和类型
│   └── ui/               # 🎨 UI 组件库
├── .github/              # 🤖 GitHub Actions 配置
├── docs/                 # 📚 项目文档
└── scripts/              # 🛠️ 构建和部署脚本
```

## ⚡ 快速开始

### 环境要求

- Node.js >= 18.0.0
- pnpm >= 8.0.0

### 安装和运行

```bash
# 克隆项目
git clone https://github.com/IAMWHCZ/JackSite.git
cd JackSite

# 安装依赖
pnpm install

# 启动开发环境
pnpm dev

# 分别启动前后端
pnpm dev:web    # 前端: http://localhost:3000
pnpm dev:api    # 后端: http://localhost:3001
```

## 📋 可用脚本

```bash
# 开发
pnpm dev              # 启动所有应用
pnpm dev:web          # 仅启动前端
pnpm dev:api          # 仅启动后端

# 构建
pnpm build            # 构建所有应用
pnpm build:web        # 仅构建前端
pnpm build:api        # 仅构建后端

# 测试
pnpm test             # 运行所有测试
pnpm test:web         # 仅测试前端
pnpm test:api         # 仅测试后端

# 代码质量
pnpm lint             # 检查代码风格
pnpm lint:fix         # 自动修复代码风格
pnpm type-check       # TypeScript 类型检查
```

## 🛠️ 技术栈

### 前端 (Web)
- ⚛️ React 18 + TypeScript
- ⚡ Vite 构建工具
- 🎨 Tailwind CSS
- 📱 响应式设计
- 🔄 React Query 状态管理

### 后端 (API)
- 🟢 Node.js + Express/Fastify
- 📝 TypeScript
- 🗄️ PostgreSQL/MongoDB
- 🔐 JWT 认证
- 📊 RESTful API

### 开发工具
- 📦 pnpm Workspaces
- 🔍 ESLint + Prettier
- 🧪 Jest + Testing Library
- 🚀 GitHub Actions
- 📋 Husky Git Hooks

## 🔧 开发指南

### 添加新依赖

```bash
# 为特定应用添加依赖
pnpm --filter web add react-router-dom
pnpm --filter api add express

# 添加开发依赖
pnpm --filter web add -D @types/node
```

### 创建新包

```bash
# 在 packages 目录下创建新包
mkdir packages/new-package
cd packages/new-package
pnpm init
```

### 环境变量配置

复制 `.env.example` 文件并重命名为 `.env`，然后填入你的配置：

```bash
cp .env.example .env
```

## 🚀 部署

### Vercel 部署 (推荐)

```bash
# 安装 Vercel CLI
npm i -g vercel

# 部署
vercel --prod
```

### Docker 部署

```bash
# 构建镜像
docker build -t jacksite .

# 运行容器
docker run -p 3000:3000 jacksite
```

## 🤝 贡献指南

1. Fork 本仓库
2. 创建特性分支 (`git checkout -b feature/amazing-feature`)
3. 提交更改 (`git commit -m 'Add some amazing feature'`)
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
```

## 📄 许可证

本项目采用 [MIT](LICENSE) 许可证。

## 👨‍💻 作者

**IAMWHCZ**

- GitHub: [@IAMWHCZ](https://github.com/IAMWHCZ)
- Email: c2545481217@gmail.com

## 📞 支持

如果你喜欢这个项目，请给它一个 ⭐！

有问题？请创建一个 [Issue](https://github.com/IAMWHCZ/JackSite/issues)。