import { MainLayout } from '@/components/layouts/MainLayout'
import { LogPage } from '@/pages/log'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/(app)/log')({
    component: () => <MainLayout children={<LogPage />} />
})
