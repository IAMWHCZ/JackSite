import { createRoot } from 'react-dom/client';
import './index.css';
import { createRouter, RouterProvider } from "@tanstack/react-router";
import { routeTree } from "./routeTree.gen.ts";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ThemeProvider } from '@/components/ThemeProvider';
import { DocumentTitle } from '@/components/DocumentTitle';
import '@/lib/i18n';
import { Toaster } from 'react-hot-toast';
import { DialogsProvider } from '@toolpad/core/useDialogs';
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { zhCN, enUS } from 'date-fns/locale';
import { useTranslation } from 'react-i18next';

const router = createRouter({ routeTree })

declare module '@tanstack/react-router' {
    interface Register {
        router: typeof router
    }
}

const queryClient = new QueryClient()

// 创建一个包装组件来处理动态区域设置
const LocalizedProvider = ({ children }: { children: React.ReactNode }) => {
    const { i18n } = useTranslation();
    
    // 根据当前语言选择区域设置
    const locale = i18n.language === 'zh' ? zhCN : enUS;
    
    return (
        <LocalizationProvider dateAdapter={AdapterDateFns} adapterLocale={locale}>
            {children}
        </LocalizationProvider>
    );
};

createRoot(document.getElementById('root')!).render(
    <QueryClientProvider client={queryClient}>
        <ThemeProvider>
            <DocumentTitle />
            <LocalizedProvider>
                <DialogsProvider>
                    <RouterProvider router={router} />
                </DialogsProvider>
            </LocalizedProvider>
            <Toaster />
        </ThemeProvider>
    </QueryClientProvider>
)

