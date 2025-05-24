import { MainLayout } from '@/components/layouts/MainLayout'
import { UserPage } from '@/pages/user'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/(app)/user')({
    component: () => <MainLayout children={<UserPage />} />
})