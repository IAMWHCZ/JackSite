import { useState } from 'react';
import { Link, useNavigate } from '@tanstack/react-router';
import { useTranslation } from 'react-i18next';
import { useTheme } from '@/contexts/ThemeContext';
import { ThemeToggle } from '../../components/ThemeToggle';
import { LanguageSelector } from '../../components/LanguageSelector';
import { Button } from '../../components/ui/button';
import { Bell, Menu, X, User, LogOut, Settings } from 'lucide-react';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import { cn } from '@/lib/utils';
import '@/fonts/styles/Navigation.css';
import { NavigationItems, type NavigationItemProp } from '@/configs/navigation';

export const Navigation = () => {
  const { t } = useTranslation('common');
  const { theme } = useTheme();
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [selectedKey, setSelectedKey] = useState<NavigationItemProp['key']>(1); // 管理当前选中的
  const navigate = useNavigate();
  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen);
  };
  const handleItemClick = (key: NavigationItemProp['key']) => {
    setSelectedKey(key);
  };
  const handleLogout = () => {
    localStorage.clear();
    sessionStorage.clear();
    navigate({ to: '/login' });
  };
  const isDark = theme === 'dark';

  return (
    <nav
      className={cn(
        'sticky top-0 z-40 w-full border-b shadow-sm',
        isDark ? 'border-gray-800 bg-gray-900 text-white' : 'border-gray-200 bg-white text-gray-800'
      )}
    >
      <div className="w-full px-4 sm:px-6 lg:px-8">
        <div className="flex h-16 w-full items-center justify-between">
          {/* Logo and desktop navigation */}
          <div className="flex items-center">
            <Link to="/" className="flex flex-shrink-0 items-center">
              <span
                className={cn(
                  'app-title ml-2 text-xl font-bold',
                  isDark ? 'text-white' : 'text-gray-800'
                )}
              >
                {t('appName', '认证系统')}
              </span>
            </Link>

            <div className="hidden sm:ml-6 sm:flex sm:space-x-4">
              {NavigationItems.map(item => (
                <Link
                  onClick={() => handleItemClick(item.key)}
                  key={item.key}
                  to={item.to}
                  className={cn(
                    'rounded-md px-3 py-2 text-sm font-medium',
                    item.key === selectedKey &&
                      (isDark
                        ? 'bg-gray-800 text-white hover:bg-gray-700'
                        : 'bg-gray-100 text-gray-900 hover:bg-gray-200')
                  )}
                >
                  {t(item.label)}
                </Link>
              ))}
            </div>
          </div>

          {/* Desktop right side menu */}
          <div className="hidden sm:flex sm:items-center sm:space-x-3">
            {/* Theme toggle */}
            <ThemeToggle />

            {/* Language selector */}
            <LanguageSelector
              className={cn(
                isDark
                  ? 'bg-gray-800 text-gray-200 hover:bg-gray-700'
                  : 'bg-gray-100 text-gray-700 hover:bg-gray-200'
              )}
            />

            {/* Notifications */}
            <Button
              variant="ghost"
              size="icon"
              className={cn(
                'rounded-full',
                isDark ? 'text-gray-200 hover:bg-gray-800' : 'text-gray-700 hover:bg-gray-100'
              )}
            >
              <Bell className="h-5 w-5" />
              <span className="sr-only">{t('notifications', '通知')}</span>
            </Button>

            {/* User menu */}
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Button
                  variant="ghost"
                  className={cn(
                    'h-8 w-8 rounded-full p-0',
                    isDark ? 'hover:bg-gray-800' : 'hover:bg-gray-100'
                  )}
                >
                  <span className="sr-only">{t('openUserMenu', '打开用户菜单')}</span>
                  <img
                    className="h-8 w-8 rounded-full"
                    src="https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80"
                    alt=""
                  />
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent
                align="end"
                className={cn(isDark ? 'border-gray-800 bg-gray-900' : 'border-gray-200 bg-white')}
              >
                <DropdownMenuLabel className={isDark ? 'text-gray-200' : 'text-gray-700'}>
                  {t('myAccount', '我的账户')}
                </DropdownMenuLabel>
                <DropdownMenuSeparator className={isDark ? 'bg-gray-800' : 'bg-gray-200'} />
                <DropdownMenuItem
                  className={
                    isDark ? 'text-gray-200 hover:bg-gray-800' : 'text-gray-700 hover:bg-gray-100'
                  }
                >
                  <User className="mr-2 h-4 w-4" />
                  <span>{t('navigation.profile', '个人资料')}</span>
                </DropdownMenuItem>
                <DropdownMenuItem
                  className={
                    isDark ? 'text-gray-200 hover:bg-gray-800' : 'text-gray-700 hover:bg-gray-100'
                  }
                >
                  <Settings className="mr-2 h-4 w-4" />
                  <span>{t('settings', '设置')}</span>
                </DropdownMenuItem>
                <DropdownMenuSeparator className={isDark ? 'bg-gray-800' : 'bg-gray-200'} />
                <DropdownMenuItem
                  className={
                    isDark ? 'text-gray-200 hover:bg-gray-800' : 'text-gray-700 hover:bg-gray-100'
                  }
                  onClick={handleLogout}
                >
                  <LogOut className="mr-2 h-4 w-4" />
                  <span>{t('signOut', '退出登录')}</span>
                </DropdownMenuItem>
              </DropdownMenuContent>
            </DropdownMenu>
          </div>

          {/* Mobile menu button */}
          <div className="flex sm:hidden">
            <Button
              variant="ghost"
              size="icon"
              onClick={toggleMenu}
              className={cn(
                'rounded-md',
                isDark
                  ? 'text-gray-300 hover:bg-gray-800 hover:text-white'
                  : 'text-gray-500 hover:bg-gray-100 hover:text-gray-700'
              )}
            >
              <span className="sr-only">{t('openMainMenu', '打开主菜单')}</span>
              {isMenuOpen ? <X className="h-6 w-6" /> : <Menu className="h-6 w-6" />}
            </Button>
          </div>
        </div>
      </div>

      {/* Mobile menu */}
      {isMenuOpen && (
        <div className="sm:hidden">
          <div className={cn('space-y-1 px-2 pb-3 pt-2', isDark ? 'bg-gray-900' : 'bg-white')}>
            <Link
              to="/"
              className={cn(
                'block rounded-md px-3 py-2 text-base font-medium',
                isDark ? 'bg-gray-800 text-white' : 'bg-gray-100 text-gray-900'
              )}
            >
              {t('home', '首页')}
            </Link>
            <Link
              to="/"
              className={cn(
                'block rounded-md px-3 py-2 text-base font-medium',
                isDark
                  ? 'text-gray-300 hover:bg-gray-800 hover:text-white'
                  : 'text-gray-600 hover:bg-gray-100 hover:text-gray-900'
              )}
            >
              {t('dashboard', '控制台')}
            </Link>
            <Link
              to="/profile"
              className={cn(
                'block rounded-md px-3 py-2 text-base font-medium',
                isDark
                  ? 'text-gray-300 hover:bg-gray-800 hover:text-white'
                  : 'text-gray-600 hover:bg-gray-100 hover:text-gray-900'
              )}
            >
              {t('profile', '个人资料')}
            </Link>
          </div>

          {/* Mobile profile section */}
          <div
            className={cn(
              'border-t px-4 pb-3 pt-4',
              isDark ? 'border-gray-800 bg-gray-900' : 'border-gray-200 bg-white'
            )}
          >
            <div className="flex items-center">
              <div className="flex-shrink-0">
                <img
                  className="h-10 w-10 rounded-full"
                  src="https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80"
                  alt=""
                />
              </div>
              <div className="ml-3">
                <div
                  className={cn('text-base font-medium', isDark ? 'text-white' : 'text-gray-800')}
                >
                  用户名
                </div>
                <div
                  className={cn('text-sm font-medium', isDark ? 'text-gray-400' : 'text-gray-500')}
                >
                  user@example.com
                </div>
              </div>
            </div>
            <div className="mt-3 space-y-1">
              <Button
                variant="ghost"
                className={cn(
                  'w-full justify-start px-4 py-2 text-left text-base font-medium',
                  isDark
                    ? 'text-gray-300 hover:bg-gray-800 hover:text-white'
                    : 'text-gray-600 hover:bg-gray-100 hover:text-gray-900'
                )}
              >
                <User className="mr-3 h-5 w-5" />
                {t('profile', '个人资料')}
              </Button>

              <Button
                variant="ghost"
                className={cn(
                  'w-full justify-start px-4 py-2 text-left text-base font-medium',
                  isDark
                    ? 'text-gray-300 hover:bg-gray-800 hover:text-white'
                    : 'text-gray-600 hover:bg-gray-100 hover:text-gray-900'
                )}
              >
                <Settings className="mr-3 h-5 w-5" />
                {t('settings', '设置')}
              </Button>

              <Button
                variant="ghost"
                className={cn(
                  'w-full justify-start px-4 py-2 text-left text-base font-medium',
                  isDark
                    ? 'text-gray-300 hover:bg-gray-800 hover:text-white'
                    : 'text-gray-600 hover:bg-gray-100 hover:text-gray-900'
                )}
              >
                <LogOut className="mr-3 h-5 w-5" />
                {t('signOut', '退出登录')}
              </Button>

              {/* Theme toggle in mobile menu */}
              <div className="px-4 py-2">
                <ThemeToggle showText={true} className="w-full justify-start" />
              </div>

              {/* Language selector in mobile menu */}
              <div className="px-4 py-2">
                <LanguageSelector />
              </div>
            </div>
          </div>
        </div>
      )}
    </nav>
  );
};
