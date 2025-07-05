# 🚀 JackSite

A modern full-stack web application built with Monorepo architecture and microservices ecosystem.

## 📁 Project Structure

```
JackSite/
├── client/               # 🌐 Frontend client application
├── server/               # 🔧 Backend services cluster
│   ├── apps/
│   │   ├── gateway/      # 🚪 API Gateway service
│   │   └── host/         # 🏠 Host service
│   ├── docs/             # 📚 Backend documentation
│   ├── infrastructure/   # 🏗️ Infrastructure configuration
│   ├── services/         # 🔧 Microservices modules
│   │   └── src/
│   └── shared/           # 📦 Backend shared code
├── docker/               # 🐳 Docker configuration
│   └── .dockerignore
├── docs/                 # 📚 Project documentation
├── .github/              # 🤖 GitHub Actions configuration
└── scripts/              # 🛠️ Build and deployment scripts
```

## 🏗️ Architecture Design

### Microservices Architecture
- **Gateway Service** - API gateway for routing and authentication
- **Host Service** - Core business logic service
- **Infrastructure** - Infrastructure configuration and deployment
- **Shared** - Cross-service shared code and utilities

### Frontend-Backend Separation
- **Client** - Frontend single-page application
- **Server** - Backend microservices cluster

## ⚡ Quick Start

### Requirements

- Node.js >= 18.0.0
- pnpm >= 8.0.0
- Docker >= 20.0.0
- Docker Compose >= 2.0.0

### Local Development

```bash
# Clone the project
git clone https://github.com/IAMWHCZ/JackSite.git
cd JackSite

# Install dependencies
pnpm install

# Start development environment (all services)
pnpm dev

# Start services individually
pnpm dev:client     # Frontend: http://localhost:3000
pnpm dev:gateway    # Gateway: http://localhost:3001
pnpm dev:host       # Host: http://localhost:3002
```

### Docker Deployment

```bash
# Build all service images
docker-compose build

# Start all services
docker-compose up -d

# Check service status
docker-compose ps

# View logs
docker-compose logs -f
```

## 📋 Available Scripts

```bash
# Development
pnpm dev                  # Start all services
pnpm dev:client           # Start frontend only
pnpm dev:gateway          # Start gateway service only
pnpm dev:host            # Start host service only

# Build
pnpm build               # Build all services
pnpm build:client        # Build frontend only
pnpm build:server        # Build backend services only

# Testing
pnpm test                # Run all tests
pnpm test:client         # Test frontend only
pnpm test:server         # Test backend only

# Code Quality
pnpm lint                # Check all code style
pnpm lint:fix            # Auto-fix code style
pnpm type-check          # TypeScript type checking

# Docker
pnpm docker:build        # Build Docker images
pnpm docker:up           # Start Docker containers
pnpm docker:down         # Stop Docker containers
```

## 🛠️ Tech Stack

### Frontend (Client)
- ⚛️ React 18 + TypeScript
- ⚡ Vite build tool
- 🎨 Tailwind CSS / Ant Design
- 📱 Responsive design
- 🔄 React Query state management
- 🧭 React Router navigation

### Backend (Server)
- 🟢 Node.js + TypeScript
- 🚪 **Gateway Service**: Express/Fastify + routing gateway
- 🏠 **Host Service**: Core business logic
- 🗄️ Database: PostgreSQL/MongoDB
- 🔐 Authentication: JWT + OAuth2
- 📊 API: RESTful + GraphQL
- 🔄 Message Queue: Redis/RabbitMQ

### Infrastructure
- 🐳 Docker + Docker Compose
- ☸️ Kubernetes (optional)
- 🔄 CI/CD: GitHub Actions
- 📊 Monitoring: Prometheus + Grafana
- 📝 Logging: ELK Stack

### Development Tools
- 📦 pnpm Workspaces
- 🔍 ESLint + Prettier
- 🧪 Jest + Testing Library
- 🚀 GitHub Actions
- 📋 Husky Git Hooks

## 🔧 Development Guide

### Adding New Microservice

```bash
# Create new service under server/services
mkdir server/services/new-service
cd server/services/new-service

# Initialize service
pnpm init
```

### Environment Configuration

```bash
# Copy environment template
cp .env.example .env

# Configure environment for different services
cp server/apps/gateway/.env.example server/apps/gateway/.env
cp server/apps/host/.env.example server/apps/host/.env
```

### Database Migration

```bash
# Run database migration
pnpm migrate:up

# Rollback migration
pnpm migrate:down

# Generate new migration file
pnpm migrate:generate
```

## 🚀 Deployment

### Development Deployment

```bash
# Use Docker Compose
docker-compose -f docker-compose.dev.yml up -d
```

### Production Deployment

```bash
# Build production images
docker-compose -f docker-compose.prod.yml build

# Deploy to production
docker-compose -f docker-compose.prod.yml up -d
```

### Cloud Platform Deployment

#### Vercel (Frontend)
```bash
# Install Vercel CLI
npm i -g vercel

# Deploy frontend
cd client
vercel --prod
```

#### AWS/Alibaba Cloud (Backend)
```bash
# Deploy with Kubernetes
kubectl apply -f infrastructure/k8s/
```

## 🏗️ Service Details

### Gateway Service
- **Port**: 3001
- **Features**: API routing, authentication, rate limiting, logging
- **Technology**: Express.js + middlewares

### Host Service
- **Port**: 3002
- **Features**: Core business logic, data processing
- **Technology**: Fastify + TypeORM

### Client Application
- **Port**: 3000
- **Features**: User interface, interaction logic
- **Technology**: React + Vite

## 📊 Monitoring & Logging

### Application Monitoring
- **Health Check**: `/health` endpoint
- **Metrics Collection**: Prometheus
- **Visualization**: Grafana dashboard

### Log Management
- **Structured Logging**: Winston
- **Log Aggregation**: ELK Stack
- **Error Tracking**: Sentry

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'feat: add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Commit Convention

Follow [Conventional Commits](https://www.conventionalcommits.org/) specification:

```
feat: new feature
fix: bug fix
docs: documentation update
style: code formatting
refactor: code refactoring
test: testing related
chore: build process or auxiliary tools
perf: performance optimization
ci: CI/CD related
```

### Code Standards

- Use TypeScript strict mode
- Follow ESLint and Prettier configuration
- Write unit tests and integration tests
- Update relevant documentation

## 🔒 Security

- JWT Token authentication
- CORS configuration
- Request rate limiting
- Input validation
- SQL injection protection
- XSS protection

## 📄 License

This project is licensed under the [MIT](LICENSE) License.

## 👨‍💻 Author

**IAMWHCZ**

- GitHub: [@IAMWHCZ](https://github.com/IAMWHCZ)
- Email: c2545481217@gmail.com

## 📞 Support

If you like this project, please give it a ⭐!

Have questions? Please create an [Issue](https://github.com/IAMWHCZ/JackSite/issues).

## 📚 Related Documentation

- [API Documentation](./docs/api.md)
- [Deployment Guide](./docs/deployment.md)
- [Development Guide](./docs/development.md)
- [Architecture Design](./docs/architecture.md)

---

English | **[中文](./README.md)**