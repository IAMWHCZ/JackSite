import { MainLayout } from '@/components/layouts/MainLayout'
import { RolePage } from '@/pages/roles'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/(app)/role')({
  component: () => <MainLayout children={<RolePage />} />
})