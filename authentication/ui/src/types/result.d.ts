import type { AxiosRequestConfig } from 'axios';

// 定义后端统一返回结果类型
export interface ApiResult<T = any> {
  success: boolean;
  code: number;
  message: string;
  timestamp: number;
  data?: T;
}

// 扩展AxiosRequestConfig，添加自定义配置
export interface CustomAxiosRequestConfig extends AxiosRequestConfig {
  skipErrorHandler?: boolean; // 是否跳过全局错误处理
  showSuccessMessage?: boolean; // 是否显示成功消息
  showErrorMessage?: boolean; // 是否显示错误消息
  preventCache?: boolean; // 是否添加请求时间戳，防止缓存
}
