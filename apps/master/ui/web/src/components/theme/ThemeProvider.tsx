import { createContext, ReactNode, useContext, useEffect, useState } from 'react';

type Theme = 'dark' | 'light' | 'system'

interface ThemeProviderProps {
    children: ReactNode;
    defaultTheme: Theme;  // 修改这里：将 string 改为 Theme
    enableSystem?: boolean;
}

const ThemeProviderContext = createContext<{
    theme: Theme
    setTheme: (theme: Theme) => void
      }>({
        theme: 'system',
        setTheme: () => null,
      });

export function ThemeProvider({
  children,
  defaultTheme
}: ThemeProviderProps) {
  const [theme, setTheme] = useState<Theme>(defaultTheme);

  useEffect(() => {
    const root = window.document.documentElement;
    root.classList.remove('light', 'dark');

    if (theme === 'system') {
      const systemTheme = window.matchMedia('(prefers-color-scheme: dark)').matches
        ? 'dark'
        : 'light';
      root.classList.add(systemTheme);
      return;
    }

    root.classList.add(theme);
  }, [theme]);

  useEffect(() => {
    const savedTheme = localStorage.getItem('theme') as Theme;
    if (savedTheme) {
      setTheme(savedTheme);
    }
  }, []);

  const value = {
    theme,
    setTheme: (theme: Theme) => {
      localStorage.setItem('theme', theme);
      setTheme(theme);
    },
  };

  return (
    <ThemeProviderContext.Provider value={value}>
      {children}
    </ThemeProviderContext.Provider>
  );
}

export const useTheme = () => {
  const context = useContext(ThemeProviderContext);
  if (context === undefined) {
    throw new Error('useTheme must be used within a ThemeProvider');
  }
  return context;
};
