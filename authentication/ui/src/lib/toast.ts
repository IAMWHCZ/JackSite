import { toast, type ExternalToast } from 'sonner';

/**
 * 显示成功提示
 * @param message 提示内容
 * @param options 额外配置选项
 */
export function ToastSuccess(message: string, options?: ExternalToast) {
  return toast.success(message, {
    duration: 3000,
    position: 'top-right',
    className: 'success-toast',
    ...options,
  });
}

/**
 * 显示错误提示
 * @param message 错误内容
 * @param options 额外配置选项
 */
export function ToastError(message: string, options?: ExternalToast) {
  return toast.error(message, {
    duration: 5000,
    position: 'top-right',
    className: 'error-toast',
    ...options,
  });
}

/**
 * 显示警告提示
 * @param message 警告内容
 * @param options 额外配置选项
 */
export function ToastWarning(message: string, options?: ExternalToast) {
  return toast.warning(message, {
    duration: 4000,
    position: 'top-right',
    className: 'warning-toast',
    ...options,
  });
}
