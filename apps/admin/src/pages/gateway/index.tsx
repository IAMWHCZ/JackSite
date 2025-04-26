
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import {
  Box,
  Tab,
  Tabs, Button
} from '@mui/material';
import { useMutation } from '@tanstack/react-query';
import { GatewayApi } from '@/apis/gateway';
import toast from 'react-hot-toast';
import { useClusters } from '@/hooks/useClusters';
import { useRoutes } from '@/hooks/useRoutes';
import { RouteEditor } from './components/RouteEditor';
import { ClusterEditor } from './components/ClusterEditor';

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
        value === 'cluster' ? await cluster.getJson.refetch() : await routes.getJson.refetch()
      }
    } catch (error) {
      toast.error(t('common.error.unknown'));
      console.error('Reload failed:', error);
    }
  }
  return (
    <Box p={3} height={"100%"}>            
      <Box sx={{ 
        display: 'flex', 
        gap: 2,  // 组件之间的间距
        flexDirection: 'row'  // 水平排列
      }}>
        <Box sx={{ flex: 1 }}>  {/* 占据等量空间 */}
          <RouteEditor />
        </Box>
        <Box sx={{ flex: 1 }}>
          <ClusterEditor />
        </Box>
      </Box>
    </Box>
  );
};







