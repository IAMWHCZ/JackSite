import { useTranslation } from 'react-i18next';
import { useTheme } from '@/contexts/ThemeContext';

import { Globe } from 'lucide-react';
import { cn } from '@/lib/utils';
import { Button } from '@/components/ui/button';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';

interface LanguageSelectorProps {
  className?: string;
  dropdownPosition?: 'top' | 'bottom';
  fullWidth?: boolean;
  showText?: boolean;
}

export const LanguageSelector = ({
  className = '',
  dropdownPosition = 'bottom',
  fullWidth = false,
  showText = false,
}: LanguageSelectorProps) => {
  const { i18n, t } = useTranslation('common');
  const { theme } = useTheme();
  const isDark = theme === 'dark';

  const changeLanguage = (value: string) => {
    i18n.changeLanguage(value);
  };

  const currentLanguage = i18n.language || 'zh';
  const isZh = currentLanguage === 'zh';

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button
          variant="ghost"
          size={showText ? 'default' : 'icon'}
          className={cn(
            showText ? '' : 'rounded-full',
            isDark ? 'text-gray-200 hover:bg-gray-800' : 'text-gray-700 hover:bg-gray-100',
            fullWidth && 'w-full justify-start',
            className
          )}
          aria-label={t('selectLanguage', '选择语言')}
        >
          <Globe className={cn('h-5 w-5', showText && 'mr-2')} />
          {showText && (isZh ? '中文' : 'English')}
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent
        align="end"
        side={dropdownPosition}
        className={cn(isDark ? 'border-gray-800 bg-gray-900' : 'border-gray-200 bg-white')}
      >
        <DropdownMenuItem
          onClick={() => changeLanguage('zh')}
          className={cn(
            isDark ? 'text-gray-200 hover:bg-gray-800' : 'text-gray-700 hover:bg-gray-100',
            isZh && (isDark ? 'bg-gray-800' : 'bg-gray-100')
          )}
        >
          中文
        </DropdownMenuItem>
        <DropdownMenuItem
          onClick={() => changeLanguage('en')}
          className={cn(
            isDark ? 'text-gray-200 hover:bg-gray-800' : 'text-gray-700 hover:bg-gray-100',
            !isZh && (isDark ? 'bg-gray-800' : 'bg-gray-100')
          )}
        >
          English
        </DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>
  );
};
