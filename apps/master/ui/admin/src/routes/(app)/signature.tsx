import { MainLayout } from '@/components/layouts/MainLayout'
import { SignaturePage } from '@/pages/signature'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/(app)/signature')({
  component: () => <MainLayout children={<SignaturePage />} />
})
