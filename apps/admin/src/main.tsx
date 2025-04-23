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

const router = createRouter({ routeTree })

declare module '@tanstack/react-router' {
    interface Register {
        router: typeof router
    }
}

const queryClient = new QueryClient()

createRoot(document.getElementById('root')!).render(
    <QueryClientProvider client={queryClient}>
        <ThemeProvider>
            <DocumentTitle />
            <DialogsProvider>
                <RouterProvider router={router} />
            </DialogsProvider>
            <Toaster />
        </ThemeProvider>
    </QueryClientProvider>
)

