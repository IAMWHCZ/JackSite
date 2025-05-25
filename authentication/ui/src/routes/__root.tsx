import { createRootRoute, Outlet } from '@tanstack/react-router';
import { TanStackRouterDevtools } from '@tanstack/react-router-devtools';
import { Content } from '@/layouts/Content';
import { useRouterState } from '@tanstack/react-router';
import { Navigation } from '@/layouts/Navigation';

export const Route = createRootRoute({
  component: RootComponent,
});

function RootComponent() {
  const router = useRouterState();
  const is404 = router.matches.some(match => match.routeId === '/$404');
  
  // 如果是404页面，直接渲染Outlet，不包含导航和内容容器
  if (is404) {
    return <Outlet />;
  }
  
  // 正常页面包含导航和内容容器
  return (
    <>
      <Navigation />
      <Content>
        <Outlet />
      </Content>
      <TanStackRouterDevtools />
    </>
  );
}
