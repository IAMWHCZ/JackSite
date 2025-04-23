import axios, { AxiosError, AxiosInstance, AxiosRequestConfig, AxiosResponse } from 'axios';
import { toast } from 'react-hot-toast';

export interface RequestOptions extends AxiosRequestConfig {
  showError?: boolean;
  showSuccess?: boolean;
}

export interface ApiResponse<T = object> {
  isSuccess: boolean;
  isFailure: boolean;
  value: T;
  message: string;
  errors:string;
}

const BASE_URL = import.meta.env.VITE_API_BASE_URL;

class HttpClient {
  private instance: AxiosInstance;

  constructor() {
    this.instance = axios.create({
      baseURL: BASE_URL,
      timeout: 10000,
      headers: {
        'Content-Type': 'application/json',
      },
    });

    // 开发环境打印请求信息
    if (import.meta.env.DEV) {
      this.instance.interceptors.request.use(config => {
        return config;
      });
    }

    this.setupInterceptors();
  }

  private setupInterceptors() {
    // 请求拦截器
    this.instance.interceptors.request.use(
      (config) => {
        const token = localStorage.getItem('token');
        if (token) {
          config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
      },
      (error) => {
        return Promise.reject(error);
      }
    );

    // 响应拦截器
    this.instance.interceptors.response.use(
      (response: AxiosResponse<ApiResponse>) => {
        const { data } = response;
        if(data.isFailure){
          toast.error(data.message || '请求失败');
          return Promise.reject(new Error(data.message));
        }
        return response;
      },
      (error: AxiosError<ApiResponse>) => {
        const options = error.config as RequestOptions;

        if (options.showError !== false) {
          // 处理网络错误
          if (!error.response) {
            toast.error('网络错误，请检查网络连接');
            return Promise.reject(error);
          }

          // 处理 HTTP 状态码
          switch (error.response.status) {
            case 401:
              toast.error('未登录或登录已过期');
              localStorage.removeItem('token');
              window.location.href = '/login';
              break;
            case 403:
              toast.error('没有权限');
              break;
            case 404:
              toast.error('请求的资源不存在');
              break;
            case 500:
              toast.error('服务器错误');
              break;
            default:
              toast.error(
                error.response.data?.message || 
                `请求失败 (${error.response.status})`
              );
          }
        }

        return Promise.reject(error);
      }
    );
  }

  public async request<T = any>(
    config: RequestOptions
  ): Promise<ApiResponse<T>> {
    try {
      const response = await this.instance.request<ApiResponse<T>>(config);
      return response.data;
    } catch (error) {
      throw error;
    }
  }

  public async get<T = any>(
    url: string,
    config?: RequestOptions
  ): Promise<ApiResponse<T>> {
    return await this.request<T>({ ...config, method: 'GET', url });
  }

  public async post<T = any>(
    url: string,
    data?: any,
    config?: RequestOptions
  ): Promise<ApiResponse<T>> {
    return await this.request<T>({ ...config, method: 'POST', url, data });
  }

  public async put<T = any>(
    url: string,
    data?: any,
    config?: RequestOptions
  ): Promise<ApiResponse<T>> {
    return await this.request<T>({ ...config, method: 'PUT', url, data });
  }

  public async delete<T = any>(
    url: string,
    config?: RequestOptions
  ): Promise<ApiResponse<T>> {
    return await this.request<T>({ ...config, method: 'DELETE', url });
  }

  public async patch<T = any>(
    url: string,
    data?: any,
    config?: RequestOptions
  ): Promise<ApiResponse<T>> {
    return this.request<T>({ ...config, method: 'PATCH', url, data });
  }
}

export const http = new HttpClient();

// 导出基础URL，方便其他地方使用
export const apiBaseUrl = BASE_URL;

export default http;
