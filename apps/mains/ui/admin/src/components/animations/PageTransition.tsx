import { motion } from 'framer-motion';
import { Box } from '@mui/material';

const pageVariants = {
  initial: {
    opacity: 0,
    x: -30, // 稍微增加距离
  },
  enter: {
    opacity: 1,
    x: 0,
    transition: {
      duration: 0.4, // 增加持续时间
      ease: [0.4, 0, 0.2, 1], // 使用更平滑的缓动函数
    },
  },
  exit: {
    opacity: 0,
    x: 30, // 稍微增加距离
    transition: {
      duration: 0.3, // 增加持续时间
      ease: [0.4, 0, 1, 1], // 使用更平滑的缓动函数
    },
  },
};

interface PageTransitionProps {
  children: React.ReactNode;
}

export const PageTransition = ({ children }: PageTransitionProps) => {
  return (
    <motion.div
      initial="initial"
      animate="enter"
      exit="exit"
      variants={pageVariants}
      style={{ height: '100%' }}
    >
      <Box height="100%">
        {children}
      </Box>
    </motion.div>
  );
};

