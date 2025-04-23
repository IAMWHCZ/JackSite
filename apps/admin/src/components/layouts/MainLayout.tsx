import { Box } from '@mui/material';
import { Outlet, useLocation } from '@tanstack/react-router';
import { AnimatePresence } from 'framer-motion';
import { Sidebar } from './Sidebar';
import { Header } from './Header';
import { useSystemStore } from '@/stores/system';
import { PageTransition } from '../animations/PageTransition';

const DRAWER_WIDTH = 240;
const COLLAPSED_WIDTH = 64;

export const MainLayout = () => {
  const { isSideBarCollapsed } = useSystemStore();
  const location = useLocation();

  return (
    <Box sx={{ display: 'flex', minHeight: '100vh' }}>
      <Sidebar />
      <Box
        component="main"
        sx={{
          flexGrow: 1,
          marginLeft: isSideBarCollapsed ? `${COLLAPSED_WIDTH}px` : `${DRAWER_WIDTH}px`,
          transition: 'margin-left 0.2s ease-in-out',
          minHeight: '100vh',
          display: 'flex',
          flexDirection: 'column',
        }}
      >
        <Header />
        <Box
          sx={{
            flexGrow: 1,
            p: 1,
            mt:8,
            overflow: 'hidden', // 防止动画溢出
            display: 'flex',
            flexDirection: 'column',
            minHeight: 0, // 关键：确保flex子元素不会溢出
          }}
        >
          <AnimatePresence mode="wait">
            <PageTransition key={location.pathname}>
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
                <Outlet />
              </Box>
            </PageTransition>
          </AnimatePresence>
        </Box>
      </Box>
    </Box>
  );
};



