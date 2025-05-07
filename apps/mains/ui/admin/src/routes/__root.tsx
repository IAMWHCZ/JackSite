import { createRootRoute } from '@tanstack/react-router';
import { MainLayout } from '@/components/layouts/MainLayout';
import { NotFoundPage } from '@/pages/errors/404';

export const Route = createRootRoute({
  component: MainLayout,
  notFoundComponent:NotFoundPage
});
