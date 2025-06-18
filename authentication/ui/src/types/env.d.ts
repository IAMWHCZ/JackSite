/// <reference types="vite/client" />

interface ImportMetaEnv {
  // ==================== 应用配置 ====================
  /** 应用运行环境：开发环境、生产环境或测试环境 */
  readonly VITE_APP_ENV: 'development' | 'production' | 'test';

  /** 应用名称，用于页面标题、错误报告等场景 */
  readonly VITE_APP_NAME: string;

  /** 应用版本号，用于版本控制和缓存策略 */
  readonly VITE_APP_VERSION: string;

  // ==================== API 配置 ====================
  /** API 服务器基础URL地址 */
  readonly VITE_API_BASE_URL: string;

  /** API 请求超时时间（毫秒），防止请求长时间挂起 */
  readonly VITE_API_TIMEOUT: string;

  // ==================== 认证配置 ====================
  /** 存储访问令牌的本地存储键名 */
  readonly VITE_AUTH_TOKEN_KEY: string;

  /** 存储刷新令牌的本地存储键名 */
  readonly VITE_AUTH_REFRESH_TOKEN_KEY: string;

  /** 访问令牌过期时间（毫秒），用于自动刷新逻辑 */
  readonly VITE_AUTH_TOKEN_EXPIRES_IN: string;

  /** Cookie 域名配置，用于跨子域名共享认证状态 */
  readonly VITE_AUTH_COOKIE_DOMAIN: string;

  // ==================== 第三方登录配置 ====================
  /** Google OAuth 客户端ID，用于 Google 第三方登录 */
  readonly VITE_GOOGLE_CLIENT_ID: string;

  /** GitHub OAuth 应用ID，用于 GitHub 第三方登录 */
  readonly VITE_GITHUB_CLIENT_ID: string;

  // ==================== 调试配置 ====================
  /** 是否启用调试模式，影响日志输出和错误处理 */
  readonly VITE_DEBUG_MODE: string;

  /** 是否显示开发者工具面板（如表单状态调试信息） */
  readonly VITE_SHOW_DEVTOOLS: string;

  /** 日志记录级别，控制控制台输出的详细程度 */
  readonly VITE_LOG_LEVEL: 'debug' | 'info' | 'warn' | 'error';

  // ==================== 国际化配置 ====================
  /** 应用默认语言代码（如 zh-CN, en-US） */
  readonly VITE_DEFAULT_LANGUAGE: string;

  /** 语言包加载失败时的备用语言代码 */
  readonly VITE_FALLBACK_LANGUAGE: string;

  // ==================== 主题配置 ====================
  /** 应用默认主题模式：light（浅色）、dark（深色）或 system（跟随系统） */
  readonly VITE_DEFAULT_THEME: string;

  /** 用户主题偏好设置在本地存储中的键名 */
  readonly VITE_THEME_STORAGE_KEY: string;

  // ==================== 功能开关 ====================
  /** 是否启用 PWA（渐进式 Web 应用）功能 */
  readonly VITE_ENABLE_PWA: string;

  /** 是否启用用户行为分析和统计功能 */
  readonly VITE_ENABLE_ANALYTICS: string;

  /** 是否启用错误监控和自动报告功能 */
  readonly VITE_ENABLE_ERROR_REPORTING: string;

  // ==================== 安全配置 ====================
  /** 是否启用安全 Cookie（仅在 HTTPS 下传输），可选配置 */
  readonly VITE_SECURE_COOKIES?: string;

  /** Cookie SameSite 属性配置，防止 CSRF 攻击，可选配置 */
  readonly VITE_SAME_SITE_COOKIES?: string;

  // ==================== 性能配置 ====================
  /** 是否启用资源压缩功能，可选配置 */
  readonly VITE_ENABLE_COMPRESSION?: string;

  /** 静态资源缓存最大存活时间（秒），可选配置 */
  readonly VITE_CACHE_MAX_AGE?: string;
}

/**
 * 扩展 ImportMeta 接口，提供类型安全的环境变量访问
 * 使用方式：import.meta.env.VITE_APP_NAME
 */
interface ImportMeta {
  readonly env: ImportMetaEnv;
}
