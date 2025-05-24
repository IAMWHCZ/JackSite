import { Box, Skeleton } from '@mui/material';
import React from 'react';

export const AppSkeleton = () => {
  return (
    <Box sx={{ display: 'flex', height: '100vh', overflow: 'hidden' }}>
      {/* 侧边栏骨架 */}
      <Box
        sx={{
          width: 240,
          backgroundColor: '#111',
          height: '100%',
          position: 'fixed',
          left: 0,
          top: 0,
        }}
      >
        {/* Logo 区域 */}
        <Box sx={{ p: 2, display: 'flex', alignItems: 'center', gap: 2 }}>
          <Skeleton variant="circular" width={40} height={40} sx={{ bgcolor: 'rgba(255,255,255,0.1)' }} />
          <Skeleton variant="text" width={120} height={30} sx={{ bgcolor: 'rgba(255,255,255,0.1)' }} />
        </Box>
        
        {/* 菜单项 */}
        <Box sx={{ mt: 2, px: 2 }}>
          {Array(8).fill(0).map((_, i) => (
            <Box key={i} sx={{ mb: 2, display: 'flex', alignItems: 'center', gap: 2 }}>
              <Skeleton variant="circular" width={24} height={24} sx={{ bgcolor: 'rgba(255,255,255,0.1)' }} />
              <Skeleton variant="text" width={150} height={24} sx={{ bgcolor: 'rgba(255,255,255,0.1)' }} />
            </Box>
          ))}
        </Box>
      </Box>
      
      {/* 主内容区骨架 */}
      <Box sx={{ ml: '240px', flex: 1, p: 3 }}>
        {/* 头部 */}
        <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 4 }}>
          <Skeleton variant="text" width={200} height={40} />
          <Box sx={{ display: 'flex', gap: 2 }}>
            <Skeleton variant="circular" width={40} height={40} />
            <Skeleton variant="circular" width={40} height={40} />
          </Box>
        </Box>
        
        {/* 内容卡片 */}
        <Box sx={{ display: 'grid', gridTemplateColumns: 'repeat(3, 1fr)', gap: 3, mb: 4 }}>
          {Array(3).fill(0).map((_, i) => (
            <Skeleton key={i} variant="rectangular" height={120} />
          ))}
        </Box>
        
        {/* 表格 */}
        <Skeleton variant="rectangular" height={400} />
      </Box>
    </Box>
  );
};