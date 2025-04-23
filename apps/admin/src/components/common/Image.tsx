import { useState, useEffect, forwardRef } from 'react';
import {
  Box,
  Dialog,
  DialogContent,
  IconButton,
  Skeleton,
  styled,
  Tooltip,
  useTheme,
} from '@mui/material';
import { ZoomIn, X, AlertCircle } from 'lucide-react';
import { useTranslation } from 'react-i18next';

// 样式化组件
const ImageContainer = styled(Box)(({ theme }) => ({
  position: 'relative',
  display: 'inline-block',
  overflow: 'hidden',
  '&:hover .image-overlay': {
    opacity: 1,
  },
}));

const ImageOverlay = styled(Box)(({ theme }) => ({
  position: 'absolute',
  top: 0,
  left: 0,
  right: 0,
  bottom: 0,
  backgroundColor: 'rgba(0, 0, 0, 0.3)',
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
  opacity: 0,
  transition: theme.transitions.create('opacity'),
  cursor: 'pointer',
}));

const ErrorContainer = styled(Box)(({ theme }) => ({
  display: 'flex',
  flexDirection: 'column',
  alignItems: 'center',
  justifyContent: 'center',
  padding: theme.spacing(2),
  backgroundColor: theme.palette.mode === 'light' 
    ? theme.palette.grey[100] 
    : theme.palette.grey[900],
  color: theme.palette.error.main,
  width: '100%',
  height: '100%',
  minHeight: 200,
}));

export interface ImageProps extends React.ImgHTMLAttributes<HTMLImageElement> {
  /** 图片链接 */
  src: string;
  /** 替代文本 */
  alt: string;
  /** 是否启用预览 */
  preview?: boolean;
  /** 加载失败时的替代图片 */
  fallback?: string;
  /** 是否懒加载 */
  lazy?: boolean;
  /** 加载时的占位尺寸 */
  width?: number | string;
  height?: number | string;
  /** 自定义样式 */
  sx?: any;
  /** 自定义错误提示 */
  errorMessage?: string;
}

export const Image = forwardRef<HTMLImageElement, ImageProps>(({
  src,
  alt,
  preview = false,
  fallback = 'https://placehold.co/600x400?text=Image+Not+Found',
  lazy = false,
  width,
  height,
  sx,
  errorMessage,
  ...props
}, ref) => {
  const [isLoading, setIsLoading] = useState(true);
  const [hasError, setHasError] = useState(false);
  const [showPreview, setShowPreview] = useState(false);
  const [imageSrc, setImageSrc] = useState(src);
  const theme = useTheme();
  const { t } = useTranslation();

  useEffect(() => {
    setImageSrc(src);
    setIsLoading(true);
    setHasError(false);
  }, [src]);

  const handleLoad = () => {
    setIsLoading(false);
    setHasError(false);
  };

  const handleError = () => {
    setIsLoading(false);
    setHasError(true);
    setImageSrc(fallback);
  };

  const handlePreviewOpen = () => {
    if (preview && !hasError) {
      setShowPreview(true);
    }
  };

  return (
    <>
      <ImageContainer sx={{ width, height, ...sx }}>
        {/* 加载状态 */}
        {isLoading && (
          <Box position="absolute" width="100%" height="100%">
            <Skeleton
              variant="rectangular"
              width="100%"
              height="100%"
              animation="wave"
            />
          </Box>
        )}

        {/* 错误状态 */}
        {hasError && (
          <ErrorContainer>
            <AlertCircle size={24} />
            <Box mt={1} textAlign="center">
              {errorMessage || t('image.loadError', 'Image failed to load')}
            </Box>
          </ErrorContainer>
        )}

        {/* 图片 */}
        <Box
          component="img"
          ref={ref}
          src={imageSrc}
          alt={alt}
          loading={lazy ? 'lazy' : 'eager'}
          onLoad={handleLoad}
          onError={handleError}
          sx={{
            width: '100%',
            height: '100%',
            objectFit: 'cover',
            display: isLoading ? 'none' : 'block',
          }}
          {...props}
        />

        {/* 预览遮罩 */}
        {preview && !hasError && !isLoading && (
          <ImageOverlay className="image-overlay" onClick={handlePreviewOpen}>
            <Tooltip title={t('image.preview', 'Preview')}>
              <IconButton
                size="small"
                sx={{
                  color: 'white',
                  '&:hover': {
                    backgroundColor: 'rgba(255, 255, 255, 0.2)',
                  },
                }}
              >
                <ZoomIn />
              </IconButton>
            </Tooltip>
          </ImageOverlay>
        )}
      </ImageContainer>

      {/* 预览对话框 */}
      <Dialog
        open={showPreview}
        onClose={() => setShowPreview(false)}
        maxWidth="lg"
        fullWidth
      >
        <DialogContent sx={{ position: 'relative', p: 0, overflow: 'hidden' }}>
          <IconButton
            onClick={() => setShowPreview(false)}
            sx={{
              position: 'absolute',
              right: 8,
              top: 8,
              color: 'white',
              backgroundColor: 'rgba(0, 0, 0, 0.5)',
              '&:hover': {
                backgroundColor: 'rgba(0, 0, 0, 0.7)',
              },
            }}
          >
            <X />
          </IconButton>
          <Box
            component="img"
            src={imageSrc}
            alt={alt}
            sx={{
              width: '100%',
              height: 'auto',
              display: 'block',
            }}
          />
        </DialogContent>
      </Dialog>
    </>
  );
});

Image.displayName = 'Image';