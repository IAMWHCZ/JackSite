import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/signature')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/signature"!</div>
}
