
import { GatewayPage } from '@/pages/gateway'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/gateway')({
  component: GatewayPage,
})

