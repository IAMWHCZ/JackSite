import { GatewayApi } from "@/apis/gateway";
import { ApiResponse } from "@/lib/http";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

export function useRoutes() {
    const queryClient = useQueryClient();
    const queryKey = ['gateway-route-json'];

    return {
        // 查询路由列表
        list: useQuery({
            queryKey:['gateway-route-list'],
            queryFn: GatewayApi.routes.getList,
            enabled: false 
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
        }),
        // 获取路由配置
        getJson: useQuery({
            queryKey,
            queryFn: GatewayApi.routes.getJson
        }),
        // 更新路由配置
        updateJson: useMutation({
            mutationFn: GatewayApi.routes.updateJson,
            onSuccess: () => queryClient.invalidateQueries({ queryKey })
        })
    };
}