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
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import dayjs from 'dayjs';
import 'dayjs/locale/zh-cn';
import 'dayjs/locale/en';
import { useTranslation } from 'react-i18next';
import { StrictMode, Suspense } from 'react';
import { ModernAppSkeleton } from './components/loading/ModernSkeleton';

// 创建查询客户端
const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            staleTime: 1000 * 60 * 5, // 5分钟
            retry: 1,
        },
    },
});

// 创建路由
const router = createRouter({
    routeTree,
    defaultPreload: 'intent',
    defaultPreloadStaleTime: 1000 * 60 * 5, // 5分钟
});

// 声明类型扩展
declare module '@tanstack/react-router' {
    interface Register {
        router: typeof router;
    }
}

// 本地化日期选择器提供者
const LocalizedProvider = ({ children }: { children: React.ReactNode }) => {
    const { i18n } = useTranslation();
    const locale = i18n.language === 'zh' ? 'zh-cn' : 'en';
    // 设置 dayjs 语言
    dayjs.locale(locale);

    return (
        <LocalizationProvider dateAdapter={AdapterDayjs}>
            {children}
        </LocalizationProvider>
    );
};

// 使用 React 19 的 createRoot API
createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <QueryClientProvider client={queryClient}>
            <ThemeProvider>
                <DocumentTitle />
                <LocalizedProvider>
                    <DialogsProvider>
                        <Suspense fallback={<ModernAppSkeleton />}>
                            <RouterProvider router={router} />
                        </Suspense>
                    </DialogsProvider>
                </LocalizedProvider>
                <Toaster />
            </ThemeProvider>
        </QueryClientProvider>
    </StrictMode >
);
