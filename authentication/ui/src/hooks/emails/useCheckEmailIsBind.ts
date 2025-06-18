import { ApiService } from '@/services/api.service';
import { useQuery, type UseQueryOptions } from '@tanstack/react-query';

export function useCheckEmailIsBind(
  email: string,
  options?: UseQueryOptions<boolean, Error>
) {
  return useQuery({
    queryKey: ['email', 'bind', email],
    queryFn: async () => {
      const response = await ApiService.get<boolean>(`/email/is-bind/${email}`, {
        preventCache: true,
      });
      return response.data ?? false; // 从 ApiResult 中提取 data
    },
    enabled: !!email && email.length > 0,
    staleTime: 1000 * 60 * 5, // 5分钟
    retry: 2,
    ...options,
  });
}
