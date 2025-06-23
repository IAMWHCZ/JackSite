import type { ApiResult, CustomAxiosRequestConfig } from '@/types/result';
import axios, { type AxiosInstance, type AxiosResponse } from 'axios';
import { toast } from 'sonner';

const createAxiosInstance = (): AxiosInstance => {
  const instance = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL || '/api',
    timeout: 10000,
    headers: {
      'Content-Type': 'application/json',
    },
  });

  // 请求拦截器
  instance.interceptors.request.use(
    config => {
      // 添加认证token
      const token = localStorage.getItem('token') || sessionStorage.getItem('token');
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }

      // 添加请求时间戳（可选，用于防止缓存）
      if (config.method === 'get' && (config as CustomAxiosRequestConfig).preventCache) {
        config.params = {
          ...config.params,
          _t: Date.now(),
        };
      }
      return config;
    },
    error => {
      return Promise.reject(error);
    }
  );

  // 响应拦截器
  instance.interceptors.response.use(
    (response: AxiosResponse<ApiResult>) => {
      const { data } = response;
      const config = response.config as CustomAxiosRequestConfig;

      // 检查业务状态码
      if (data.success) {
        // 成功时显示消息（可选）
        if (config.showSuccessMessage && data.message) {
          toast.error('Success', {
            description: data.message,
          });
        }
        return response;
      } else {
        // 业务失败
        const error = new Error(data.message || '请求失败');
        (error as any).code = data.code;
        (error as any).response = response;
        throw error;
      }
    },
    error => {
      const config = error.config as CustomAxiosRequestConfig;

      // 如果配置了跳过错误处理，直接抛出
      if (config?.skipErrorHandler) {
        return Promise.reject(error);
      }

      // 处理不同类型的错误
      if (error.response) {
        const { status, data } = error.response;

        switch (status) {
          case 400:
            toast.error('', {
              position: 'top-center',
              description: `${data?.Message || '无权限访问'}`,
            });
            break;
          case 401:
            // 未授权，清除token并跳转到登录页
            localStorage.removeItem('token');
            sessionStorage.removeItem('token');
            location.href = '/login';
            break;
          case 403:
            toast.error('', {
              description: `❌ Error 403: ${data?.message || '无权限访问'}`,
            });
            break;
          case 404:
            toast.error('', {
              description: `❌ Error 404: ${data?.message || '资源不存在'}`,
            });
            break;
          case 500:
            toast.error('', {
              description: `❌ Error 500: ${data?.message || '服务器内部错误'}`,
            });
            break;
          default:
            toast.error('', {
              description: `❌ Error ${status}: ${data?.message || '请求失败'}`,
            });
        }

        // 显示错误消息
        if (config?.showErrorMessage !== false) {
          const message = data?.message || `请求失败 (${status})`;
          throw new Error(message);
        }
      } else if (error.request) {
        throw new Error('请求未响应，请检查网络连接或服务器状态');
      } else {
        throw new Error(`请求配置错误: ${error.message}`);
      }

      return Promise.reject(error);
    }
  );

  return instance;
};

export const apiClient = createAxiosInstance();
