import { createFileRoute } from '@tanstack/react-router'
import SignaturePage from "@/pages/common/signature";


export const Route = createFileRoute('/signature')({
  component: SignaturePage,
})