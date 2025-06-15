class Environment {
  // 应用信息
  static get APP_ENV() {
    return import.meta.env.VITE_APP_ENV
  }

  static get APP_NAME() {
    return import.meta.env.VITE_APP_NAME
  }

  static get APP_VERSION() {
    return import.meta.env.VITE_APP_VERSION
  }

  // API 配置
  static get API_BASE_URL() {
    return import.meta.env.VITE_API_BASE_URL
  }

  static get API_TIMEOUT() {
    return parseInt(import.meta.env.VITE_API_TIMEOUT)
  }

  // 认证配置
  static get AUTH_TOKEN_KEY() {
    return import.meta.env.VITE_AUTH_TOKEN_KEY
  }

  static get AUTH_REFRESH_TOKEN_KEY() {
    return import.meta.env.VITE_AUTH_REFRESH_TOKEN_KEY
  }

  static get AUTH_TOKEN_EXPIRES_IN() {
    return parseInt(import.meta.env.VITE_AUTH_TOKEN_EXPIRES_IN)
  }

  static get AUTH_COOKIE_DOMAIN() {
    return import.meta.env.VITE_AUTH_COOKIE_DOMAIN
  }

  // 第三方登录
  static get GOOGLE_CLIENT_ID() {
    return import.meta.env.VITE_GOOGLE_CLIENT_ID
  }

  static get GITHUB_CLIENT_ID() {
    return import.meta.env.VITE_GITHUB_CLIENT_ID
  }

  // 调试配置
  static get DEBUG_MODE() {
    return import.meta.env.VITE_DEBUG_MODE === 'true'
  }

  static get SHOW_DEVTOOLS() {
    return import.meta.env.VITE_SHOW_DEVTOOLS === 'true'
  }

  static get LOG_LEVEL() {
    return import.meta.env.VITE_LOG_LEVEL
  }

  // 国际化
  static get DEFAULT_LANGUAGE() {
    return import.meta.env.VITE_DEFAULT_LANGUAGE
  }

  static get FALLBACK_LANGUAGE() {
    return import.meta.env.VITE_FALLBACK_LANGUAGE
  }

  // 主题
  static get DEFAULT_THEME() {
    return import.meta.env.VITE_DEFAULT_THEME
  }

  static get THEME_STORAGE_KEY() {
    return import.meta.env.VITE_THEME_STORAGE_KEY
  }

  // 功能开关
  static get ENABLE_PWA() {
    return import.meta.env.VITE_ENABLE_PWA === 'true'
  }

  static get ENABLE_ANALYTICS() {
    return import.meta.env.VITE_ENABLE_ANALYTICS === 'true'
  }

  static get ENABLE_ERROR_REPORTING() {
    return import.meta.env.VITE_ENABLE_ERROR_REPORTING === 'true'
  }

  // 安全配置
  static get SECURE_COOKIES() {
    return import.meta.env.VITE_SECURE_COOKIES === 'true'
  }

  static get SAME_SITE_COOKIES() {
    return import.meta.env.VITE_SAME_SITE_COOKIES || 'lax'
  }

  // 工具方法
  static isDevelopment() {
    return this.APP_ENV === 'development'
  }

  static isProduction() {
    return this.APP_ENV === 'production'
  }

  static isTest() {
    return this.APP_ENV === 'test'
  }
}

export default Environment