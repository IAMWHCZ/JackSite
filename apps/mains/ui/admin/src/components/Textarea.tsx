
import { forwardRef } from 'react';
import { TextField, TextFieldProps, styled, Box, Skeleton } from '@mui/material';

const StyledTextField = styled(TextField)(({ theme }) => ({
  '& .MuiInputBase-root': {
    transition: theme.transitions.create([
      'border-color',
      'background-color',
      'box-shadow',
    ]),
    '&:hover': {
      backgroundColor: theme.palette.mode === 'dark' 
        ? 'rgba(255, 255, 255, 0.05)' 
        : 'rgba(0, 0, 0, 0.02)',
    },
    '&.Mui-focused': {
      backgroundColor: theme.palette.mode === 'dark' 
        ? 'rgba(255, 255, 255, 0.05)' 
        : 'rgba(0, 0, 0, 0.02)',  
    },
    // 确保根元素填充容器高度
    height: '100%',
    display: 'flex',
    paddingRight:0
  },
  '& .MuiInputBase-input': {
    padding: theme.spacing(2),
    lineHeight: 1.5,
    // 添加滚动条样式
    overflow: 'auto !important',
    // 禁用自动增长
    resize: 'none',
    // 确保输入框填充容器
    height: '100% !important',
    // 滚动条样式
    '&::-webkit-scrollbar': {
      width: '8px',
      height: '8px',
    },
    '&::-webkit-scrollbar-track': {
      backgroundColor: 'transparent',
    },
    '&::-webkit-scrollbar-thumb': {
      backgroundColor: theme.palette.mode === 'dark' 
        ? 'rgba(255, 255, 255, 0.2)' 
        : 'rgba(0, 0, 0, 0.2)',
      borderRadius: '4px',
      '&:hover': {
        backgroundColor: theme.palette.mode === 'dark' 
          ? 'rgba(255, 255, 255, 0.3)' 
          : 'rgba(0, 0, 0, 0.3)',
      },
    },
  },
}));

export interface TextareaProps extends Omit<TextFieldProps, 'variant'> {
  /**
   * 是否自动调整高度
   * @default false
   */
  autoResize?: boolean;
  /**
   * 最大高度（仅在 autoResize 为 true 时生效）
   * @default '400px'
   */
  maxHeight?: number | string;
  /**
   * 是否显示加载状态
   * @default false
   */
  loading?: boolean;
}

export const Textarea = forwardRef<HTMLDivElement, TextareaProps>(
  ({ autoResize = false, maxHeight = 400, loading = false, ...props }, ref) => {
    const handleChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
      if (autoResize) {
        event.target.style.height = 'auto';
        const scrollHeight = event.target.scrollHeight;
        event.target.style.height = `${Math.min(
          scrollHeight,
          typeof maxHeight === 'number' ? maxHeight : parseInt(maxHeight)
        )}px`;
      }
      props.onChange?.(event);
    };

    if (loading) {
      return (
        <Box sx={{ position: 'relative', width: '100%', height: '100%' }}>
          <Skeleton 
            variant="rectangular" 
            width="100%" 
            height="100%" 
            animation="wave"
            sx={{ 
              borderRadius: 1,
              bgcolor: theme => theme.palette.mode === 'dark' 
                ? 'rgba(255, 255, 255, 0.05)' 
                : 'rgba(0, 0, 0, 0.05)'
            }} 
          />
        </Box>
      );
    }

    return (
      <StyledTextField
        {...props}
        ref={ref}
        multiline
        fullWidth
        variant="outlined"
        onChange={handleChange}
        sx={{
          height: '100%', // 确保组件填充容器高度
          display: 'flex',
          '& .MuiInputBase-root': {
            flex: 1,
          },
          ...props.sx,
          ...(autoResize && {
            '& .MuiInputBase-root': {
              height: 'auto',
              maxHeight,
            },
            '& .MuiInputBase-input': {
              overflow: 'auto',
              resize: 'none',
            },
          }),
        }}
      />
    );
  }
);

Textarea.displayName = 'Textarea';
