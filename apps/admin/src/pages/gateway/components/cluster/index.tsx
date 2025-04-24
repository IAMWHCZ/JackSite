import { Box, Button, Typography } from '@mui/material'
import { Plus, RefreshCw } from 'lucide-react'
import { useTranslation } from 'react-i18next';
import { ClusterTable } from './ClusterTable';
import { ClusterDialog } from './ClusterDialog';
import { useGatewayStore } from '@/stores/gateway';
import { useClusters } from '@/hooks/useClusters';
import toast from 'react-hot-toast';

export const ClusterContainer = () => {
    const { t } = useTranslation();
    const { setOpenClusterDialog,setSelectedCluster } = useGatewayStore()
    const { reload } = useClusters()
    
    const handleReloadConfig = async () => {
        const res = await reload.mutateAsync()
        if(res.isFailure){
            toast.error(res.errors)
            return
        }
    };

    return (
        <Box p={3}>
            <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
                <Typography variant="h5" component="h1">
                    {t('gateway.title.clusters')}
                </Typography>
                <Box display="flex" gap={2}>
                    <Button
                        variant="contained"
                        startIcon={<Plus size={20} />}
                        onClick={() => {
                            setSelectedCluster(null!)
                            setOpenClusterDialog(true)
                        }}
                    >
                        {t('gateway.actions.addCluster')}
                    </Button>
                    <Button
                        variant="outlined"
                        startIcon={<RefreshCw size={20} className={reload.isPending ? 'animate-spin' : ''} />}
                        onClick={handleReloadConfig}
                        disabled={reload.isPending}
                    >
                        {reload.isPending ? t('gateway.loading.reloading') : t('gateway.clusters.reload')}
                    </Button>
                </Box>
            </Box>
            <ClusterTable />

            <ClusterDialog />
        </Box>
    )
}
