import { create } from "zustand";

interface Props {
    openClusterDialog: boolean;
    setOpenClusterDialog: (open: boolean) => void;
    openRouteDialog: boolean;
    setOpenRouteDialog: (open: boolean) => void;
    setSelectedRoute: (routeId: string) => void;
    selectedRoute: string;
    setSelectedCluster: (clusterId: string) => void;
    selectedCluster: string;
}

export const useGatewayStore = create<Props>((set)=>({
    openClusterDialog: false,
    setOpenClusterDialog: (open) => set({openClusterDialog: open}),
    openRouteDialog: false,
    setOpenRouteDialog: (open) => set({openRouteDialog: open}),
    setSelectedRoute: (routeId) => set({selectedRoute: routeId}),
    selectedRoute: '',
    setSelectedCluster: (clusterId) => set({selectedCluster: clusterId}),
    selectedCluster: '',
}))