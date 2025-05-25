import { StrictMode } from 'react';
import ReactDOM from 'react-dom/client';
import { RouterProvider, createRouter } from '@tanstack/react-router';
import { routeTree } from './routeTree.gen';
import '@/lib/i18n.ts';
import './styles.css';
import { ThemeProvider } from './contexts/ThemeContext';
import { TitleProvider } from './contexts/TitleContext';

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
if (rootElement && !rootElement.innerHTML) {
  const root = ReactDOM.createRoot(rootElement);
  root.render(
    <StrictMode>
      <TitleProvider>
        <ThemeProvider>
          <RouterProvider router={router} />
        </ThemeProvider>
      </TitleProvider>
    </StrictMode>
  );
}
