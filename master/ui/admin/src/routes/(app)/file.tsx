import { MainLayout } from '@/components/layouts/MainLayout'
import { FilePage } from '@/pages/file'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/(app)/file')({
  component: () => <MainLayout children={<FilePage />} />,
})
