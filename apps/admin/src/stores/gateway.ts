import { ClusterConfig, RouteConfig } from "@/types/gateway";
import { create } from "zustand";

interface Props {
    openClusterDialog: boolean;
    setOpenClusterDialog: (open: boolean) => void;
    openRouteDialog: boolean;
    setOpenRouteDialog: (open: boolean) => void;
    setSelectedRoute: (routeId: RouteConfig) => void;
    selectedRoute: RouteConfig;
    setSelectedCluster: (cluster:ClusterConfig) => void;
    selectedCluster: ClusterConfig;
}

export const useGatewayStore = create<Props>((set)=>({
    openClusterDialog: false,
    setOpenClusterDialog: (open) => set({openClusterDialog: open}),
    openRouteDialog: false,
    setOpenRouteDialog: (open) => set({openRouteDialog: open}),
    setSelectedRoute: (routeId) => set({selectedRoute: routeId}),
    selectedRoute: null!,
    setSelectedCluster: (clusterId) => set({selectedCluster: clusterId}),
    selectedCluster: null!,
}))