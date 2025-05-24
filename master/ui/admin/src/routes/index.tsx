import { MainLayout } from '@/components/layouts/MainLayout'
import { DashboardPage } from '@/pages/dashborad'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/')({
    component: () => <MainLayout children={<DashboardPage />} />
})
