import { createFileRoute } from '@tanstack/react-router'
import JsonFormatPage from "@/pages/json";

export const Route = createFileRoute('/json/json-format')({
  component: JsonFormatPage,
})


