import { ApiService } from "@/services/api.service";
import { useQuery, type UseQueryOptions } from "@tanstack/react-query";

/**
 * @param username 用户名
 * @param options 可选的查询选项
 * @returns 返回一个查询对象，包含用户是否存在的信息
 */
export function useCheckUserExists(
  username: string,
  options?: UseQueryOptions<boolean, Error>
) {
  return useQuery({
    queryKey: ['user', 'exists', username],
    queryFn: async () => {
      const response = await ApiService.get<boolean>(`/user/exists/${username}`, {
        preventCache: true
      });
      return response.data ?? false; // 从 ApiResult 中提取 data
    },
    enabled: !!username && username.length > 0,
    staleTime: 1000 * 60 * 5, // 5分钟
    retry: 2,
    ...options,
  });
}