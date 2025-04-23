import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/diagram')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/diagram"!</div>
}
