import { Box, Typography, useTheme } from '@mui/material';
import { memo } from 'react';

interface LoaderProps {
  size?: 'small' | 'medium' | 'large';
  variant?: 'spinner' | 'dots' | 'pulse' | 'progress';
  text?: string;
  color?: string;
  fullScreen?: boolean;
  transparent?: boolean;
}

export const ModernLoader = memo(({
  size = 'medium',
  variant = 'spinner',
  text,
  color,
  fullScreen = false,
  transparent = false
}: LoaderProps) => {
  const theme = useTheme();
  const isDark = theme.palette.mode === 'dark';

  // 确定尺寸
  const sizeMap = {
    small: { loader: 24, text: 12 },
    medium: { loader: 40, text: 14 },
    large: { loader: 64, text: 16 }
  };

  const loaderSize = sizeMap[size].loader;
  const textSize = sizeMap[size].text;

  // 确定颜色
  const loaderColor = color || theme.palette.primary.main;
  const textColor = isDark ? 'rgba(255, 255, 255, 0.87)' : 'rgba(0, 0, 0, 0.87)';

  // 渲染不同类型的加载器
  const renderLoader = () => {
    switch (variant) {
      case 'spinner':
        return (
          <Box
            sx={{
              width: loaderSize,
              height: loaderSize,
              borderRadius: '50%',
              border: `3px solid ${isDark ? 'rgba(255, 255, 255, 0.1)' : 'rgba(0, 0, 0, 0.1)'}`,
              borderTopColor: loaderColor,
              animation: 'spin 1s linear infinite',
              '@keyframes spin': {
                '0%': { transform: 'rotate(0deg)' },
                '100%': { transform: 'rotate(360deg)' }
              }
            }}
          />
        );

      case 'dots':
        return (
          <Box
            sx={{
              display: 'flex',
              gap: 1,
              alignItems: 'center',
              justifyContent: 'center'
            }}
          >
            {[0, 1, 2].map((i) => (
              <Box
                key={i}
                sx={{
                  width: loaderSize / 4,
                  height: loaderSize / 4,
                  borderRadius: '50%',
                  backgroundColor: loaderColor,
                  animation: 'bounce 1.4s infinite ease-in-out',
                  animationDelay: `${i * 0.16}s`,
                  '@keyframes bounce': {
                    '0%, 100%': {
                      transform: 'scale(0)'
                    },
                    '50%': {
                      transform: 'scale(1)'
                    }
                  }
                }}
              />
            ))}
          </Box>
        );

      case 'pulse':
        return (
          <Box
            sx={{
              width: loaderSize,
              height: loaderSize,
              borderRadius: '50%',
              backgroundColor: loaderColor,
              opacity: 0.6,
              animation: 'pulse 1.5s infinite ease-in-out',
              '@keyframes pulse': {
                '0%': {
                  transform: 'scale(0.8)',
                  opacity: 0.6
                },
                '50%': {
                  transform: 'scale(1)',
                  opacity: 0.2
                },
                '100%': {
                  transform: 'scale(0.8)',
                  opacity: 0.6
                }
              }
            }}
          />
        );

      case 'progress':
        return (
          <Box
            sx={{
              width: loaderSize * 3,
              height: loaderSize / 5,
              backgroundColor: isDark ? 'rgba(255, 255, 255, 0.1)' : 'rgba(0, 0, 0, 0.1)',
              borderRadius: loaderSize / 10,
              overflow: 'hidden',
              position: 'relative',
              '&::after': {
                content: '""',
                position: 'absolute',
                top: 0,
                left: 0,
                height: '100%',
                width: '30%',
                backgroundColor: loaderColor,
                borderRadius: loaderSize / 10,
                animation: 'progress 1.5s infinite ease-in-out',
              },
              '@keyframes progress': {
                '0%': {
                  left: '-30%'
                },
                '100%': {
                  left: '100%'
                }
              }
            }}
          />
        );

      default:
        return null;
    }
  };

  // 容器样式
  const containerStyles = {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    justifyContent: 'center',
    gap: 2,
    ...(fullScreen ? {
      position: 'fixed',
      top: 0,
      left: 0,
      right: 0,
      bottom: 0,
      zIndex: 9999,
      backgroundColor: transparent
        ? 'rgba(0, 0, 0, 0)'
        : isDark
          ? 'rgba(17, 24, 39, 0.8)'
          : 'rgba(249, 250, 251, 0.8)',
      backdropFilter: 'blur(4px)'
    } : {})
  };

  return (
    <Box sx={containerStyles}>
      {renderLoader()}

      {text && (
        <Typography
          variant={size === 'small' ? 'caption' : size === 'medium' ? 'body2' : 'body1'}
          sx={{
            color: textColor,
            fontSize: textSize,
            mt: 1,
            fontWeight: 500,
            textAlign: 'center'
          }}
        >
          {text}
        </Typography>
      )}
    </Box>
  );
});

// 导出一些预设的加载器
export const FullScreenLoader = memo(({ text }: { text?: string }) => (
  <ModernLoader fullScreen size="large" text={text} />
));

export const TableLoader = memo(({ text }: { text?: string }) => (
  <ModernLoader variant="dots" text={text || "加载数据中..."} />
));

export const ButtonLoader = memo(({ color }: { color?: string }) => (
  <ModernLoader size="small" color={color} />
));

export const InlineLoader = memo(({ text }: { text?: string }) => (
  <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
    <ModernLoader size="small" variant="dots" />
    {text && (
      <Typography variant="body2">{text}</Typography>
    )}
  </Box>
));

// 创建一个进度条加载器
export const ProgressLoader = memo(({ progress, text }: { progress: number; text?: string }) => {
  const theme = useTheme();
  const isDark = theme.palette.mode === 'dark';

  return (
    <Box sx={{ width: '100%', maxWidth: 400 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 1 }}>
        <Typography variant="body2">{text || '加载中...'}</Typography>
        <Typography variant="body2">{`${Math.round(progress)}%`}</Typography>
      </Box>

      <Box
        sx={{
          height: 8,
          backgroundColor: isDark ? 'rgba(255, 255, 255, 0.1)' : 'rgba(0, 0, 0, 0.1)',
          borderRadius: 4,
          overflow: 'hidden',
          position: 'relative',
        }}
      >
        <Box
          sx={{
            position: 'absolute',
            top: 0,
            left: 0,
            height: '100%',
            width: `${progress}%`,
            backgroundColor: theme.palette.primary.main,
            borderRadius: 4,
            transition: 'width 0.3s ease-in-out',
          }}
        />
      </Box>
    </Box>
  );
});
