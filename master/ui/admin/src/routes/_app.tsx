import { MainLayout } from '@/components/layouts/MainLayout';
import { createFileRoute, Outlet } from '@tanstack/react-router';

// 这是应用页面的布局容器
export const Route = createFileRoute('/_app')({
  component: () => (
    <MainLayout>
      <Outlet />
    </MainLayout>
  )
});