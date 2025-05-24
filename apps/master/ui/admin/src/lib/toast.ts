import toast, { ToastOptions } from 'react-hot-toast';

// 正确扩展toast类型
declare module 'react-hot-toast' {
  interface DefaultToastOptions2 {
    warning?: ToastOptions;
  }
}

// 添加warning方法
const warning = (message: string, options = {}) => {
  return toast(message, {
    icon: '⚠️',
    style: {
      background: 'white',
      color: 'black',
    },
    ...options,
  });
};

// 扩展toast对象
const extendedToast = {
  ...toast,
  warning,
};

// 导出扩展后的toast
export default extendedToast;