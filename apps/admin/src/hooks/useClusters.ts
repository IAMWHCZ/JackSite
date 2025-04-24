import { GatewayApi } from "@/apis/gateway";
import { ApiResponse } from "@/lib/http";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

export function useClusters() {
    const queryClient = useQueryClient();
    const queryKey = ['gateway-clusters'];

    return {
        // 查询集群列表
        list: useQuery({
            queryKey,
            queryFn: GatewayApi.clusters.getList
        }),
        // 创建集群
        create: useMutation({
            mutationFn: GatewayApi.clusters.create,
            onSuccess: () => queryClient.invalidateQueries({ queryKey })
        }),
        // 更新集群
        update: useMutation({
            mutationFn: GatewayApi.clusters.update,
            onSuccess: () => queryClient.invalidateQueries({ queryKey })
        }),
        // 删除集群
        delete: useMutation({
            mutationFn: GatewayApi.clusters.delete,
            onSuccess: () => queryClient.invalidateQueries({ queryKey })
        }),
        // 重载配置
        reload: useMutation<ApiResponse<[]>, Error, void>({
            mutationFn: () => GatewayApi.reload(2),
            onSuccess: () => queryClient.invalidateQueries({ queryKey })
        })
    };
}