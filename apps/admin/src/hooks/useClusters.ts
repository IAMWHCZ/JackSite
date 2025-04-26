import { GatewayApi } from "@/apis/gateway";
import { ApiResponse } from "@/lib/http";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

export function useClusters() {
    const queryClient = useQueryClient();
    const queryKey =['gateway-cluster-json'];

    return {
        // 查询集群列表
        list: useQuery({
            queryKey:['gateway-cluster-list'],
            queryFn: GatewayApi.clusters.getList,
            enabled: false 
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
        }),
        // 获取集群配置
        getJson: useQuery({
            queryKey,
            queryFn: GatewayApi.clusters.getJson
        }),
        // 更新集群配置
        updateJson: useMutation({
            mutationFn: GatewayApi.clusters.updateJson,
            onSuccess: () => queryClient.invalidateQueries({ queryKey })
        })
    };
}