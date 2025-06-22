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
      toast.error('❌ Request Error:', {
        description: `❌ ${error.message || '请求失败'}`,
      });
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
        toast.error('System Error', {
          description: `❌ ${data.message || '请求失败'}`,
        });
        (error as any).code = data.code;
        (error as any).response = response;
        throw error;
      }
    },
    error => {
      toast.error('System Error', {
        description: `❌ ${error.message || '请求失败'}`,
      });
      const config = error.config as CustomAxiosRequestConfig;

      // 如果配置了跳过错误处理，直接抛出
      if (config?.skipErrorHandler) {
        return Promise.reject(error);
      }

      // 处理不同类型的错误
      if (error.response) {
        const { status, data } = error.response;

        switch (status) {
          case 401:
            // 未授权，清除token并跳转到登录页
            localStorage.removeItem('token');
            sessionStorage.removeItem('token');
            location.href = '/login';
            break;
          case 403:
            toast.error('System Error', {
              description: `❌ Error 403: ${data?.message || '无权限访问'}`,
            });
            break;
          case 404:
            toast.error('System Error', {
              description: `❌ Error 404: ${data?.message || '资源不存在'}`,
            });
            break;
          case 500:
            toast.error('System Error', {
              description: `❌ Error 500: ${data?.message || '服务器内部错误'}`,
            });
            break;
          default:
            toast.error('System Error', {
              description: `❌ Error ${status}: ${data?.message || '请求失败'}`,
            });
        }

        // 显示错误消息
        if (config?.showErrorMessage !== false) {
          const message = data?.message || `请求失败 (${status})`;
          toast.error('System Error', {
            description: `❌ Error Message:${message}`,
          });
        }
      } else if (error.request) {
        toast.error('System Error', {
          description: '🌐 Network Error:网络连接失败，请检查网络设置',
        });
      } else {
        toast.error('System Error', {
          description: `⚠️ Unknown Error: ${error.message}`,
        });
      }

      return Promise.reject(error);
    }
  );

  return instance;
};

export const apiClient = createAxiosInstance();
