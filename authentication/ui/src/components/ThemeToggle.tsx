import { useTheme } from '@/contexts/ThemeContext';
import { useTranslation } from 'react-i18next';
import { Button } from '@/components/ui/button';
import { Moon, Sun } from 'lucide-react';
import { cn } from '@/lib/utils';

interface ThemeToggleProps {
  className?: string;
  showText?: boolean;
}

export const ThemeToggle = ({ className = '', showText = false }: ThemeToggleProps) => {
  const { theme, toggleTheme } = useTheme();
  const { t } = useTranslation('common');

  const isDark = theme === 'dark';

  return (
    <Button
      variant="ghost"
      size={showText ? 'default' : 'icon'}
      onClick={toggleTheme}
      className={cn(
        showText ? '' : 'rounded-full',
        isDark ? 'text-yellow-300 hover:bg-gray-800' : 'text-gray-700 hover:bg-gray-100',
        className
      )}
      aria-label={t('toggleTheme', '切换主题')}
    >
      {isDark ? (
        <>
          <Sun className={cn('h-5 w-5', showText && 'mr-2')} />
          {showText && <span>{t('lightMode', '浅色模式')}</span>}
        </>
      ) : (
        <>
          <Moon className={cn('h-5 w-5', showText && 'mr-2')} />
          {showText && <span>{t('darkMode', '深色模式')}</span>}
        </>
      )}
    </Button>
  );
};
