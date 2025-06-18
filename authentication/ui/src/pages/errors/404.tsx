import { useTranslation } from 'react-i18next';
import { Button } from '@/components/ui/button';
import { Link } from '@tanstack/react-router';
import { useTheme } from '@/contexts/ThemeContext';
import { cn } from '@/lib/utils';
import { Home } from 'lucide-react';
export function NotFoundPage() {
  const { t } = useTranslation('common');
  const { theme } = useTheme();
  const isDark = theme === 'dark';

  return (
    <div
      className={cn(
        'flex min-h-screen w-full items-center justify-center',
        isDark ? 'bg-gray-900' : 'white'
      )}
    >
      <div className="flex w-full max-w-6xl flex-col items-center justify-center gap-12 px-4 md:flex-row md:gap-20">
        {/* 左侧：大号404 */}
        <div className="relative">
          <h1
            className={cn(
              'text-[180px] font-bold leading-none tracking-tight md:text-[220px]',
              isDark ? 'text-gray-800' : 'text-gray-200'
            )}
          >
            404
          </h1>
          <div
            className={cn(
              'absolute left-0 top-0 flex h-full w-full items-center justify-center',
              'text-[180px] font-bold leading-none tracking-tight md:text-[220px]',
              isDark ? 'text-gray-700 opacity-50' : 'text-gray-300 opacity-50'
            )}
          >
            404
          </div>
        </div>

        {/* 右侧：文字和按钮 */}
        <div className={cn('max-w-md text-left', isDark ? 'text-white' : 'text-gray-900')}>
          <h2 className="text-3xl font-semibold">{t('pageNotFound', '页面未找到')}</h2>

          <div className={cn('my-6 h-1 w-20', isDark ? 'bg-gray-700' : 'bg-gray-300')}></div>

          <p className={cn('mb-8 text-lg', isDark ? 'text-gray-400' : 'text-gray-500')}>
            {t('pageNotFoundMessage', '您请求的页面不存在或已被移除。')}
          </p>

          <Link to="/">
            <Button
              className={cn(
                'rounded-md px-8 py-6 text-base transition-all',
                isDark
                  ? 'border border-gray-700 bg-gray-800 text-white hover:bg-gray-700'
                  : 'border border-gray-200 bg-white text-gray-900 hover:bg-gray-100',
                'shadow-sm hover:shadow'
              )}
            >
              <Home size={18} className="mr-2" />
              {t('backToHome', '返回首页')}
            </Button>
          </Link>
        </div>
      </div>

      {/* 背景元素 */}
      <div className="absolute inset-0 -z-10 overflow-hidden">
        <div
          className={cn(
            'absolute inset-0 opacity-[0.03]',
            isDark
              ? 'bg-[radial-gradient(#ffffff_1px,transparent_1px)] [background-size:40px_40px]'
              : 'bg-[radial-gradient(#000000_1px,transparent_1px)] [background-size:40px_40px]'
          )}
        ></div>
      </div>
    </div>
  );
}
