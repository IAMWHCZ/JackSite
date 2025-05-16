import { Moon, Sun } from 'lucide-react';
import { useEffect } from 'react';
import { useThemeStore } from '@/stores/modules/theme.ts';

export const ThemeToggle = () => {
  const { theme, setTheme } = useThemeStore();
  useEffect(() => {
    const root = window.document.documentElement;
    root.classList.remove('light', 'dark');
    console.log(theme);
    if (theme === 'system') {
      const systemTheme = window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
      root.classList.add(systemTheme);
    } else {
      root.classList.add(theme);
    }
  }, [theme]);

  useEffect(() => {
    if (theme !== 'system') return;

    const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');

    const handleChange = () => {
      const root = window.document.documentElement;
      root.classList.remove('light', 'dark');
      root.classList.add(mediaQuery.matches ? 'dark' : 'light');
    };

    mediaQuery.addEventListener('change', handleChange);
    return () => mediaQuery.removeEventListener('change', handleChange);
  }, [theme]);

  const toggleTheme = () => {
    const isDark = document.documentElement.classList.contains('dark');
    if (theme === 'system') {
      setTheme(isDark ? 'light' : 'dark');
    } else {
      setTheme('system');
    }
  };

  return (
    <button
      onClick={toggleTheme}
      className="p-2 rounded-lg hover:bg-secondary/80"
      aria-label="Toggle theme"
    >
      {document.documentElement.classList.contains('dark') ? (
        <Sun className="h-5 w-5"/>
      ) : (
        <Moon className="h-5 w-5"/>
      )}
    </button>
  );
};
