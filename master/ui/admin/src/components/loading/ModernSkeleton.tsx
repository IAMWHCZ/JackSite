import { Box, useTheme } from '@mui/material';
import { memo } from 'react';

interface SkeletonProps {
  variant?: 'text' | 'rectangular' | 'circular';
  width?: string | number;
  height?: string | number;
  animation?: 'pulse' | 'wave' | 'shimmer' | 'none';
  sx?: any;
}

export const ModernSkeleton = memo(({
  variant = 'rectangular',
  width = '100%',
  height = 20,
  animation = 'shimmer',
  sx = {}
}: SkeletonProps) => {
  const theme = useTheme();
  const isDark = theme.palette.mode === 'dark';
  
  // 基础样式
  const baseStyles = {
    display: 'block',
    width,
    height,
    backgroundColor: isDark ? 'rgba(255, 255, 255, 0.08)' : 'rgba(0, 0, 0, 0.06)',
    position: 'relative',
    overflow: 'hidden',
    ...sx
  };
  
  // 变体样式
  const variantStyles = {
    text: {},
    rectangular: { borderRadius: '4px' },
    circular: { borderRadius: '50%' }
  };
  
  // 动画样式
  const getAnimationStyles = () => {
    switch (animation) {
      case 'pulse':
        return {
          animation: 'pulse 1.5s ease-in-out 0.5s infinite'
        };
      case 'wave':
        return {
          '&::after': {
            content: '""',
            position: 'absolute',
            top: 0,
            left: 0,
            right: 0,
            bottom: 0,
            background: `linear-gradient(90deg, transparent, ${isDark ? 'rgba(255, 255, 255, 0.08)' : 'rgba(0, 0, 0, 0.06)'}, transparent)`,
            animation: 'wave 1.6s linear 0.5s infinite'
          }
        };
      case 'shimmer':
        return {
          backgroundImage: `linear-gradient(to right, ${isDark ? 'rgba(255, 255, 255, 0.05) 0%, rgba(255, 255, 255, 0.1) 20%, rgba(255, 255, 255, 0.05) 40%' : 'rgba(0, 0, 0, 0.05) 0%, rgba(0, 0, 0, 0.08) 20%, rgba(0, 0, 0, 0.05) 40%'})`,
          backgroundSize: '800px 100%',
          animation: 'shimmer 2s infinite linear'
        };
      default:
        return {};
    }
  };
  
  return (
    <Box
      sx={{
        ...baseStyles,
        ...variantStyles[variant],
        ...getAnimationStyles(),
        '@keyframes wave': {
          '0%': { transform: 'translateX(-100%)' },
          '100%': { transform: 'translateX(100%)' }
        },
        '@keyframes pulse': {
          '0%, 100%': { opacity: 1 },
          '50%': { opacity: 0.5 }
        },
        '@keyframes shimmer': {
          '0%': { backgroundPosition: '-468px 0' },
          '100%': { backgroundPosition: '468px 0' }
        }
      }}
    />
  );
});

// 创建一个现代化的应用骨架屏
export const ModernAppSkeleton = memo(() => {
  const theme = useTheme();
  const isDark = theme.palette.mode === 'dark';
  
  return (
    <Box sx={{ 
      display: 'flex', 
      height: '100vh', 
      overflow: 'hidden',
      backgroundColor: isDark ? '#111827' : '#f9fafb',
    }}>
      {/* 侧边栏骨架 */}
      <Box
        sx={{
          width: 240,
          backgroundColor: isDark ? '#1f2937' : '#ffffff',
          height: '100%',
          position: 'fixed',
          left: 0,
          top: 0,
          borderRight: `1px solid ${isDark ? 'rgba(255,255,255,0.1)' : 'rgba(0,0,0,0.1)'}`,
          padding: 2,
          boxSizing: 'border-box',
        }}
      >
        {/* Logo 区域 */}
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, mb: 4 }}>
          <ModernSkeleton variant="circular" width={40} height={40} />
          <ModernSkeleton width={120} height={24} />
        </Box>
        
        {/* 菜单项 */}
        <Box sx={{ mt: 4 }}>
          {Array(8).fill(0).map((_, i) => (
            <Box key={i} sx={{ mb: 3, display: 'flex', alignItems: 'center', gap: 2 }}>
              <ModernSkeleton variant="circular" width={24} height={24} />
              <ModernSkeleton width={i % 3 === 0 ? '60%' : '80%'} height={16} />
            </Box>
          ))}
        </Box>
      </Box>
      
      {/* 主内容区骨架 */}
      <Box sx={{ ml: '240px', flex: 1, p: 3, boxSizing: 'border-box' }}>
        {/* 头部 */}
        <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 4, alignItems: 'center' }}>
          <Box>
            <ModernSkeleton width={180} height={32} sx={{ mb: 1 }} />
            <ModernSkeleton width={240} height={16} />
          </Box>
          <Box sx={{ display: 'flex', gap: 2 }}>
            <ModernSkeleton variant="circular" width={40} height={40} />
            <ModernSkeleton variant="circular" width={40} height={40} />
          </Box>
        </Box>
        
        {/* 统计卡片 */}
        <Box sx={{ 
          display: 'grid', 
          gridTemplateColumns: { xs: '1fr', sm: 'repeat(2, 1fr)', md: 'repeat(4, 1fr)' }, 
          gap: 3, 
          mb: 4 
        }}>
          {Array(4).fill(0).map((_, i) => (
            <Box 
              key={i} 
              sx={{ 
                p: 3, 
                borderRadius: 2, 
                backgroundColor: isDark ? '#1f2937' : '#ffffff',
                boxShadow: '0 1px 3px rgba(0,0,0,0.1)',
              }}
            >
              <ModernSkeleton width={80} height={16} sx={{ mb: 2 }} />
              <ModernSkeleton width={100} height={32} sx={{ mb: 1 }} />
              <ModernSkeleton width="60%" height={12} />
            </Box>
          ))}
        </Box>
        
        {/* 图表区域 */}
        <Box 
          sx={{ 
            p: 3, 
            borderRadius: 2, 
            backgroundColor: isDark ? '#1f2937' : '#ffffff',
            boxShadow: '0 1px 3px rgba(0,0,0,0.1)',
            mb: 4,
          }}
        >
          <ModernSkeleton width={120} height={24} sx={{ mb: 3 }} />
          <ModernSkeleton height={240} />
        </Box>
        
        {/* 表格 */}
        <Box 
          sx={{ 
            p: 3, 
            borderRadius: 2, 
            backgroundColor: isDark ? '#1f2937' : '#ffffff',
            boxShadow: '0 1px 3px rgba(0,0,0,0.1)',
          }}
        >
          <ModernSkeleton width={150} height={24} sx={{ mb: 3 }} />
          
          {/* 表头 */}
          <Box sx={{ display: 'flex', mb: 2 }}>
            {Array(4).fill(0).map((_, i) => (
              <ModernSkeleton 
                key={i} 
                width={`${i === 0 ? 40 : 100 / (4 - (i === 0 ? 1 : 0))}%`} 
                height={16} 
                sx={{ mr: 2 }} 
              />
            ))}
          </Box>
          
          {/* 表格行 */}
          {Array(5).fill(0).map((_, i) => (
            <Box key={i} sx={{ display: 'flex', py: 2, borderBottom: `1px solid ${isDark ? 'rgba(255,255,255,0.1)' : 'rgba(0,0,0,0.1)'}` }}>
              {Array(4).fill(0).map((_, j) => (
                <ModernSkeleton 
                  key={j} 
                  width={`${j === 0 ? 40 : 100 / (4 - (j === 0 ? 1 : 0))}%`} 
                  height={16} 
                  sx={{ mr: 2 }} 
                  animation={i % 2 === 0 ? 'shimmer' : 'wave'}
                />
              ))}
            </Box>
          ))}
        </Box>
      </Box>
    </Box>
  );
});