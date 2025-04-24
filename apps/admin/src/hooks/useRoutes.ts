import { GatewayApi } from "@/apis/gateway";
import { ApiResponse } from "@/lib/http";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

export function useRoutes() {
    const queryClient = useQueryClient();
    const queryKey = ['gateway-routes'];

    return {
        // 查询路由列表
        list: useQuery({
            queryKey,
            queryFn: GatewayApi.routes.getList
        }),

        // 创建路由
        create: useMutation({
            mutationFn: GatewayApi.routes.create,
            onSuccess: () => queryClient.invalidateQueries({ queryKey })
        }),

        // 更新路由
        update: useMutation({
            mutationFn: GatewayApi.routes.update,
            onSuccess: () => queryClient.invalidateQueries({ queryKey })
        }),

        // 删除路由
        delete: useMutation({
            mutationFn: GatewayApi.routes.delete,
            onSuccess: () => queryClient.invalidateQueries({ queryKey })
        }),
        // 重载配置
        reload: useMutation<ApiResponse<[]>, Error, void>({
            mutationFn: () => GatewayApi.reload(1),
            onSuccess: () => queryClient.invalidateQueries({ queryKey })
        })
    };
}