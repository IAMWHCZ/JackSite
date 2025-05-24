import { MainLayout } from '@/components/layouts/MainLayout'
import { BlogPage } from '@/pages/blog'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/(app)/blog')({
    component: () => <MainLayout children={<BlogPage />} />
})