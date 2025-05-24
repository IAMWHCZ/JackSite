import { Box } from '@mui/material';
import { useLocation } from '@tanstack/react-router';
import { AnimatePresence } from 'framer-motion';
import { memo, useMemo, Suspense, lazy } from 'react';
import { Sidebar } from './Sidebar';
import { Header } from './Header';
import { useSystemStore } from '@/stores/system';
import { PageTransition } from '../animations/PageTransition';
import { TableLoading } from '../loading/TableLoading';

const DRAWER_WIDTH = 240;
const COLLAPSED_WIDTH = 84;

// 使用 memo 包装 Sidebar 组件以避免不必要的重新渲染
const MemoizedSidebar = memo(Sidebar);
// 使用 memo 包装 Header 组件以避免不必要的重新渲染
const MemoizedHeader = memo(Header);

// 懒加载非关键组件
const LazyFooter = lazy(() => import('./Footer').then(module => ({ default: module.Footer })));

// 创建一个内容组件来处理路由变化
const ContentArea = memo(({ children, pathname }: { children: React.ReactNode, pathname: string }) => {
  return (
    <AnimatePresence mode="wait">
      <PageTransition key={pathname}>
        <Box
          sx={{
            height: '100%',
            overflow: 'auto',  // 添加滚动条
            '& > *': {  // 确保所有直接子元素也遵循这个规则
              maxWidth: '100%',
              overflow: 'auto',
            }
          }}
        >
          {children}
        </Box>
      </PageTransition>
    </AnimatePresence>
  );
});

export const MainLayout = memo(({ children }: { children?: React.ReactNode }) => {
  const { isSideBarCollapsed } = useSystemStore();
  const location = useLocation();

  // 使用 useMemo 记忆化主要内容区域样式
  const mainContentStyle = useMemo(() => ({
    flexGrow: 1,
    marginLeft: isSideBarCollapsed ? `${COLLAPSED_WIDTH}px` : `${DRAWER_WIDTH}px`,
    transition: 'margin-left 0.2s ease-in-out',
    minHeight: '100vh',
    display: 'flex',
    flexDirection: 'column',
  }), [isSideBarCollapsed]);

  // 使用 useMemo 记忆化内容容器样式
  const contentContainerStyle = useMemo(() => ({
    flexGrow: 1,
    p: 1,
    mt: 8,
    overflow: 'hidden',
    display: 'flex',
    flexDirection: 'column',
    minHeight: 0,
  }), []);

  return (
    <Box sx={{ display: 'flex' }}>
      {/* 侧边栏 */}
      <MemoizedSidebar />

      {/* 主内容区 */}
      <Box sx={mainContentStyle}>
        {/* 顶部导航栏 */}
        <MemoizedHeader />

        {/* 内容区域 */}
        <Box sx={contentContainerStyle}>
          <ContentArea pathname={location.pathname}>
            <Suspense fallback={<TableLoading />}>
              {children}
            </Suspense>
          </ContentArea>
        </Box>

        {/* 懒加载页脚 */}
        <Suspense fallback={null}>
          <LazyFooter />
        </Suspense>
      </Box>
    </Box>
  );
});
