
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import {
  Box,
  Tab,
  Tabs, Button
} from '@mui/material';
import { RouteContainer } from './components/route';
import { ClusterContainer } from './components/cluster';
import { RefreshCw } from "lucide-react";
import { useMutation } from '@tanstack/react-query';
import { GatewayApi } from '@/apis/gateway';
import toast from 'react-hot-toast';
import { useClusters } from '@/hooks/useClusters';
import { useRoutes } from '@/hooks/useRoutes';

export const GatewayPage = () => {
  const { t } = useTranslation();
  const [value, setValue] = useState<'route' | 'cluster'>('route');
  const cluster = useClusters()
  const routes = useRoutes()
  const handleChange = (_: React.SyntheticEvent, newValue: 'route' | 'cluster') => {
    setValue(newValue);
  };
  const reload = useMutation({
    mutationFn: () => GatewayApi.reload(3)
  })
  const handleReload = async () => {
    try {
      const res = await reload.mutateAsync();
      if (res.isFailure) {
        toast.error(res.errors);
        return;
      }
      if (res.isSuccess) {
        value === 'cluster' ? await cluster.list.refetch() : await routes.list.refetch()
      }
    } catch (error) {
      toast.error(t('common.error.unknown'));
      console.error('Reload failed:', error);
    }
  }
  return (
    <Box p={3}>
      <Box className={"flex flex-row justify-between items-center w-full"} >
        <Tabs
          value={value}
          onChange={handleChange}
          textColor="primary"
          indicatorColor="primary"
          sx={{
            mb: 3,
            '& .MuiTab-root': {
              fontSize: '1.2rem',
              fontWeight: 500,
              py: 1.5,
              textTransform: 'none',
            },
            '& .Mui-selected': {
              fontWeight: 600,
            },
            '& .MuiTabs-indicator': {
              height: 3,
            },
          }}
        >
          <Tab
            label={t('gateway.routes.title')}
            value="route"
            sx={{ minHeight: 56 }}
          />
          <Tab
            label={t('gateway.clusters.title')}
            value="cluster"
            sx={{ minHeight: 56 }}
          />
        </Tabs>
        <Button
          onClick={handleReload}
          loading={reload.isPending}
          variant="outlined"
          startIcon={<RefreshCw size={20} />}>
          {t('gateway.reload')}
        </Button>
      </Box>
      {
        value === 'route' ?
          <RouteContainer />
          :
          <ClusterContainer />
      }
    </Box>
  );
};







