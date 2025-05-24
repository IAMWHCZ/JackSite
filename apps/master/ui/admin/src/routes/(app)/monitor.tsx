import { MainLayout } from '@/components/layouts/MainLayout'
import { MonitorPage } from '@/pages/monitor'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/(app)/monitor')({
  component: () => <MainLayout children={<MonitorPage />} />
})
