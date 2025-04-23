import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import {
    Box,
    Typography,
    Button
} from '@mui/material';
import { Plus, RefreshCw } from 'lucide-react';
import type { RouteConfig } from '@/types/gateway';
import { RouteTable } from './RouteTable';
import { RouteDialog } from './RouteDialog';
import { useRoutes } from '@/hooks/useRoutes';
import toast from 'react-hot-toast';

export const RouteContainer = () => {
    const { t } = useTranslation();
    const [isDialogOpen, setIsDialogOpen] = useState(false);
    const [selectedRoute, setSelectedRoute] = useState<string | null>(null);
    const [routes, setRoutes] = useState<Record<string, RouteConfig>>({});
    const { reload } = useRoutes()

    const handleCloseDialog = () => {
        setIsDialogOpen(false);
        setSelectedRoute(null);
    };

    const handleSaveRoute = (data: RouteConfig) => {
        setRoutes(prev => ({
            ...prev,
            [selectedRoute || `route-${Date.now()}`]: data,
        }));
        handleCloseDialog();
    };

    
    const handleReloadConfig = async () => {
        const res = await reload.mutateAsync()
        if (res.isFailure) {
            toast.error(res.errors)
        }
    };
    return (
        <Box p={3}>
            {/* 标题和操作按钮 */}
            <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
                <Typography variant="h5" component="h1">
                    {t('gateway.title.routes')}
                </Typography>
                <Box display="flex" gap={2}>
                    <Button
                        variant="contained"
                        startIcon={<Plus size={20} />}
                        onClick={() => setIsDialogOpen(true)}
                    >
                        {t('gateway.actions.add')}
                    </Button>
                    <Button
                        variant="outlined"
                        startIcon={<RefreshCw size={20} className={reload.isPending ? 'animate-spin' : ''} />}
                        onClick={handleReloadConfig}
                        disabled={reload.isPending}
                    >
                        {reload.isPending ? t('gateway.loading.reloading') : t('gateway.routes.reload')}
                    </Button>
                </Box>
            </Box>

            {/* 路由表格 */}
            <RouteTable
            />

            {/* 路由编辑对话框 */}
            <RouteDialog
                open={isDialogOpen}
                onClose={handleCloseDialog}
                onSubmit={handleSaveRoute}
                initialData={selectedRoute ? routes[selectedRoute] : undefined}
                clusters={['cluster1', 'cluster2']} // 模拟集群数据
            />
        </Box>
    );
};


