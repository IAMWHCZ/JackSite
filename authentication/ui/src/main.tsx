import { StrictMode } from 'react';
import ReactDOM from 'react-dom/client';
import { RouterProvider, createRouter } from '@tanstack/react-router';
import { routeTree } from './routeTree.gen';
import '@/lib/i18n.ts';
import './styles.css';
import { ThemeProvider } from '@/contexts/ThemeContext';
import { TitleProvider } from '@/contexts/TitleContext';
import { Toaster } from '@/components/ui/sonner';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import '@/extensions/string';

declare module '@tanstack/react-router' {
    interface Register {
        router: typeof router;
    }
}

const router = createRouter({
    routeTree,
    context: {},
    defaultPreload: 'intent',
    scrollRestoration: true,
    defaultStructuralSharing: true,
    defaultPreloadStaleTime: 0,
});

const rootElement = document.getElementById('app');
const queryClient = new QueryClient();

if (rootElement && !rootElement.innerHTML) {
    const root = ReactDOM.createRoot(rootElement);
    root.render(
        <StrictMode>
            <QueryClientProvider client={queryClient}>
                <TitleProvider>
                    <ThemeProvider>
                        <RouterProvider router={router} />
                        <Toaster position="top-right" />
                    </ThemeProvider>
                </TitleProvider>
            </QueryClientProvider>
        </StrictMode>
    );
}
