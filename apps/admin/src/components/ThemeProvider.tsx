import { ThemeProvider as MUIThemeProvider } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import { ReactNode, useEffect } from 'react';
import { useThemeStore } from '../hooks/useThemeMode';
import { darkTheme, lightTheme } from '../styles/theme';

interface ThemeProviderProps {
  children: ReactNode;
}

export function ThemeProvider({ children }: ThemeProviderProps) {
  const { mode } = useThemeStore();

  // 同步主题到 document
  useEffect(() => {
    document.documentElement.classList.remove('light', 'dark');
    document.documentElement.classList.add(mode);
  }, [mode]);

  return (
    <MUIThemeProvider theme={mode === 'dark' ? darkTheme : lightTheme}>
      <CssBaseline />
      {children}
    </MUIThemeProvider>
  );
}