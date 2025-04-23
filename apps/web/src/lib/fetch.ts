import { toast } from 'react-hot-toast';
import NProgress from 'nprogress';

interface RequestOptions extends RequestInit {
  showProgress?: boolean;
  params?: Record<string, string>;
}

interface ResponseData<T = unknown> {
  code: number;
  data: T;
  message: string;
}

class HttpClient {
  private baseURL: string;

  constructor() {
    this.baseURL = (import.meta.env.VITE_API_BASE_URL as string) ?? 'http://localhost:3000';
  }

  private async request<T>(endpoint: string, options: RequestOptions = {}): Promise<T> {
    const {
      showProgress = true,
      params,
      headers: customHeaders,
      ...restOptions
    } = options;

    // 构建完整 URL
    const url = new URL(endpoint, this.baseURL);
    if (params) {
      Object.entries(params).forEach(([key, value]) => {
        url.searchParams.append(key, value);
      });
    }

    // 准备 headers
    const headers = new Headers(customHeaders);
    const token = localStorage.getItem('token');
    if (token) {
      headers.set('Authorization', `Bearer ${token}`);
    }
    if (!headers.has('Content-Type') && !(options.body instanceof FormData)) {
      headers.set('Content-Type', 'application/json');
    }

    try {
      if (showProgress) {
        NProgress.start();
      }

      const response = await fetch(url, {
        ...restOptions,
        headers
      });

      if (!response.ok) {
        switch (response.status) {
          case 401:
            toast.error('未登录或登录已过期');
            localStorage.removeItem('token');
            window.location.href = '/login';
            throw new Error('Unauthorized');
          case 403:
            toast.error('没有权限');
            throw new Error('Forbidden');
          case 404:
            toast.error('请求的资源不存在');
            throw new Error('Not Found');
          case 500:
            toast.error('服务器错误');
            throw new Error('Server Error');
          default:
            toast.error(`请求失败: ${response.statusText}`);
            throw new Error(`HTTP Error: ${response.status}`);
        }
      }

      const data = await response.json() as ResponseData<T>;

      if (data.code !== 200) {
        toast.error(data.message || '请求失败');
        throw new Error(data.message);
      }

      return data.data;
    } catch (error) {
      if (error instanceof Error) {
        if (error.name === 'AbortError') {
          toast.error('请求已取消');
        } else if (error.message === 'Failed to fetch') {
          toast.error('网络错误，请检查网络连接');
        } else {
          toast.error(error.message);
        }
      } else {
        toast.error('未知错误');
      }
      throw error;
    } finally {
      if (showProgress) {
        NProgress.done();
      }
    }
  }

  public async get<T>(
    endpoint: string,
    options: Omit<RequestOptions, 'body' | 'method'> = {}
  ): Promise<T> {
    return this.request<T>(endpoint, { ...options, method: 'GET' });
  }

  public async post<T>(
    endpoint: string,
    body?: unknown,
    options: Omit<RequestOptions, 'body' | 'method'> = {}
  ): Promise<T> {
    return this.request<T>(endpoint, {
      ...options,
      method: 'POST',
      body: body ? JSON.stringify(body) : undefined
    });
  }

  public async put<T>(
    endpoint: string,
    body?: unknown,
    options: Omit<RequestOptions, 'body' | 'method'> = {}
  ): Promise<T> {
    return this.request<T>(endpoint, {
      ...options,
      method: 'PUT',
      body: body ? JSON.stringify(body) : undefined
    });
  }

  public async delete<T>(
    endpoint: string,
    options: Omit<RequestOptions, 'body' | 'method'> = {}
  ): Promise<T> {
    return this.request<T>(endpoint, { ...options, method: 'DELETE' });
  }

  public async patch<T>(
    endpoint: string,
    body?: unknown,
    options: Omit<RequestOptions, 'body' | 'method'> = {}
  ): Promise<T> {
    return this.request<T>(endpoint, {
      ...options,
      method: 'PATCH',
      body: body ? JSON.stringify(body) : undefined
    });
  }
}

export const http = new HttpClient();

export default http;
