import { Box, Paper, useTheme } from '@mui/material';
import { memo } from 'react';
import { ModernSkeleton } from './ModernSkeleton';

interface TableLoadingProps {
    rows?: number;
    columns?: number;
    hasHeader?: boolean;
    hasActions?: boolean;
    hasFilters?: boolean;
    hasPagination?: boolean;
}

export const TableLoading = memo(({
    rows = 5,
    columns = 4,
    hasHeader = true,
    hasActions = true,
    hasFilters = true,
    hasPagination = true
}: TableLoadingProps) => {
    const theme = useTheme();
    const isDark = theme.palette.mode === 'dark';

    return (
        <Box sx={{ width: '100%' }}>
            {/* 表格标题和操作区 */}
            {hasHeader && (
                <Box sx={{
                    display: 'flex',
                    justifyContent: 'space-between',
                    alignItems: 'center',
                    mb: 2
                }}>
                    <ModernSkeleton width={180} height={32} />

                    {hasActions && (
                        <Box sx={{ display: 'flex', gap: 1 }}>
                            <ModernSkeleton width={100} height={36} />
                            <ModernSkeleton width={100} height={36} />
                        </Box>
                    )}
                </Box>
            )}

            {/* 过滤器区域 */}
            {hasFilters && (
                <Box sx={{
                    display: 'flex',
                    gap: 2,
                    mb: 3,
                    flexWrap: 'wrap'
                }}>
                    <ModernSkeleton width={200} height={40} />
                    <ModernSkeleton width={150} height={40} />
                    <ModernSkeleton width={120} height={40} />
                    <Box sx={{ flex: 1 }} />
                    <ModernSkeleton width={180} height={40} />
                </Box>
            )}

            {/* 表格 */}
            <Paper
                elevation={0}
                sx={{
                    width: '100%',
                    overflow: 'hidden',
                    border: `1px solid ${isDark ? 'rgba(255, 255, 255, 0.12)' : 'rgba(0, 0, 0, 0.12)'}`,
                    borderRadius: 1,
                    mb: 2
                }}
            >
                {/* 表头 */}
                <Box sx={{
                    display: 'flex',
                    p: 2,
                    borderBottom: `1px solid ${isDark ? 'rgba(255, 255, 255, 0.12)' : 'rgba(0, 0, 0, 0.12)'}`,
                    backgroundColor: isDark ? 'rgba(255, 255, 255, 0.05)' : 'rgba(0, 0, 0, 0.02)'
                }}>
                    {Array(columns).fill(0).map((_, i) => (
                        <Box
                            key={i}
                            sx={{
                                flex: i === 0 ? 0.5 : 1,
                                px: 1
                            }}
                        >
                            <ModernSkeleton
                                width={i === columns - 1 ? '50%' : '80%'}
                                height={20}
                            />
                        </Box>
                    ))}

                    {/* 操作列 */}
                    {hasActions && (
                        <Box sx={{ width: 100, textAlign: 'right' }}>
                            <ModernSkeleton width={60} height={20} sx={{ ml: 'auto' }} />
                        </Box>
                    )}
                </Box>

                {/* 表格行 */}
                {Array(rows).fill(0).map((_, rowIndex) => (
                    <Box
                        key={rowIndex}
                        sx={{
                            display: 'flex',
                            p: 2,
                            borderBottom: rowIndex < rows - 1 ? `1px solid ${isDark ? 'rgba(255, 255, 255, 0.12)' : 'rgba(0, 0, 0, 0.12)'}` : 'none',
                            '&:hover': {
                                backgroundColor: isDark ? 'rgba(255, 255, 255, 0.05)' : 'rgba(0, 0, 0, 0.02)'
                            }
                        }}
                    >
                        {Array(columns).fill(0).map((_, colIndex) => (
                            <Box
                                key={colIndex}
                                sx={{
                                    flex: colIndex === 0 ? 0.5 : 1,
                                    px: 1
                                }}
                            >
                                <ModernSkeleton
                                    width={(() => {
                                        // 为不同列生成不同宽度，使其看起来更自然
                                        if (colIndex === 0) return '60%';
                                        if (colIndex === columns - 1) return '40%';
                                        return `${70 + Math.random() * 20}%`;
                                    })()}
                                    height={16}
                                    animation={rowIndex % 2 === 0 ? 'shimmer' : 'wave'}
                                />
                            </Box>
                        ))}

                        {/* 操作按钮 */}
                        {hasActions && (
                            <Box sx={{
                                width: 100,
                                display: 'flex',
                                justifyContent: 'flex-end',
                                gap: 1
                            }}>
                                <ModernSkeleton variant="circular" width={32} height={32} />
                                <ModernSkeleton variant="circular" width={32} height={32} />
                            </Box>
                        )}
                    </Box>
                ))}
            </Paper>

            {/* 分页 */}
            {hasPagination && (
                <Box sx={{
                    display: 'flex',
                    justifyContent: 'space-between',
                    alignItems: 'center',
                    mt: 2
                }}>
                    <ModernSkeleton width={100} height={20} />

                    <Box sx={{ display: 'flex', gap: 1 }}>
                        <ModernSkeleton variant="circular" width={36} height={36} />
                        <ModernSkeleton variant="circular" width={36} height={36} />
                        <ModernSkeleton variant="circular" width={36} height={36} />
                        <ModernSkeleton variant="circular" width={36} height={36} />
                    </Box>
                </Box>
            )}
        </Box>
    );
});
