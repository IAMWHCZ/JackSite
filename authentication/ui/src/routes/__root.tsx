import { createRootRoute, Outlet } from '@tanstack/react-router';
import { TanStackRouterDevtools } from '@tanstack/react-router-devtools';
import { Content } from '@/layouts/external/Content';
import { useRouterState } from '@tanstack/react-router';
import { Navigation } from '@/layouts/external/Navigation';
import { Wrapper } from '@/layouts/full/Wrapper';
import { RouterProgress } from '@/components/RouterProgress';

export const Route = createRootRoute({
    component: RootComponent,
});

function RootComponent() {
    const router = useRouterState();

    const fullScreenList = ['/$404', '/login', '/register', '/reset-password'];
    const isFullScreen = router.matches.some(match => fullScreenList.includes(match.routeId));

    if (isFullScreen) {
        return <Wrapper />;
    }

    // 正常页面包含导航和内容容器
    return (
        <>
            <RouterProgress />
            <Navigation />
            <Content>
                <Outlet />
            </Content>
            <TanStackRouterDevtools />
        </>
    );
}
