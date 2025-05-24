import { MainLayout } from '@/components/layouts/MainLayout'
import { DiagramPage } from '@/pages/diagram'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/(app)/diagram')({
    component: () => <MainLayout children={<DiagramPage />} />
})