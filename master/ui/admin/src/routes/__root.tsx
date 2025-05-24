import { NotFoundPage } from '@/pages/errors/404';
import { createRootRoute, Outlet } from '@tanstack/react-router';
import { useSystemStore } from '@/stores/system';
import { navItems } from '@/config/route';
import { ErrorPage } from '@/pages/errors';

// 根路由只提供一个出口和开发工具
export const Route = createRootRoute({
  component: () => (
    <>
      <Outlet />
    </>
  ),
  notFoundComponent: NotFoundPage,
  errorComponent: ErrorPage,
  beforeLoad: ({ location }) => {
    // 获取当前路径
    const path = location.pathname;

    // 从路径中提取路由段
    const routeSegment = path.split('/').filter(Boolean)[0] || 'home';

    // 查找匹配的导航项以获取更准确的面包屑
    const matchedNavItem = navItems.find(item =>
      item.path === path || item.path === `/${routeSegment}`
    );

    // 使用匹配的导航项的面包屑或默认为路由段
    const breadcrumb = matchedNavItem?.breadcrumb || routeSegment;

    // 使用 store 更新面包屑
    const { setBreadcrumb } = useSystemStore.getState();
    setBreadcrumb(breadcrumb);

    // 可以在这里执行其他操作，如分析跟踪
    if (import.meta.env.DEV) {
    }

    return true;
  }
});
