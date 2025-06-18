import { apiClient } from '@/lib/axios';
import type { ApiResult, CustomAxiosRequestConfig } from '@/types/result';

export class ApiService {
  /**
   * GET请求
   */
  static async get<T = any>(url: string, config?: CustomAxiosRequestConfig): Promise<ApiResult<T>> {
    const response = await apiClient.get<ApiResult<T>>(url, config);
    return response.data;
  }

  /**
   * POST请求
   */
  static async post<T = any>(
    url: string,
    data?: any,
    config?: CustomAxiosRequestConfig
  ): Promise<ApiResult<T>> {
    const response = await apiClient.post<ApiResult<T>>(url, data, config);
    return response.data;
  }

  /**
   * PUT请求
   */
  static async put<T = any>(
    url: string,
    data?: any,
    config?: CustomAxiosRequestConfig
  ): Promise<ApiResult<T>> {
    const response = await apiClient.put<ApiResult<T>>(url, data, config);
    return response.data;
  }

  /**
   * DELETE请求
   */
  static async delete<T = any>(url: string, config?: CustomAxiosRequestConfig): Promise<ApiResult<T>> {
    const response = await apiClient.delete<ApiResult<T>>(url, config);
    return response.data;
  }

  /**
   * PATCH请求
   */
  static async patch<T = any>(
    url: string,
    data?: any,
    config?: CustomAxiosRequestConfig
  ): Promise<ApiResult<T>> {
    const response = await apiClient.patch<ApiResult<T>>(url, data, config);
    return response.data;
  }

  /**
   * 获取原始响应（包含完整的ApiResult）
   */
  static async getRaw<T = any>(
    url: string,
    config?: CustomAxiosRequestConfig
  ): Promise<ApiResult<T>> {
    const response = await apiClient.get<ApiResult<T>>(url, config);
    return response.data;
  }

  /**
   * 上传文件
   */
  static async upload<T = any>(
    url: string,
    file: File,
    onProgress?: (progress: number) => void,
    config?: CustomAxiosRequestConfig
  ): Promise<ApiResult<T>> {
    const formData = new FormData();
    formData.append('file', file);

    const response = await apiClient.post<ApiResult<T>>(url, formData, {
      ...config,
      headers: {
        'Content-Type': 'multipart/form-data',
        ...config?.headers,
      },
      onUploadProgress: progressEvent => {
        if (onProgress && progressEvent.total) {
          const progress = Math.round((progressEvent.loaded * 100) / progressEvent.total);
          onProgress(progress);
        }
      },
    });

    return response.data;
  }
}

// 导出默认实例
export default apiClient;
