import {  createRootRoute } from '@tanstack/react-router';
import RootLayout from "@/components/layouts/RootLayout";
import {NotFoundPage} from "@/pages/errors/404.tsx";

export const Route = createRootRoute({
    component: RootLayout,
    notFoundComponent: NotFoundPage,
});