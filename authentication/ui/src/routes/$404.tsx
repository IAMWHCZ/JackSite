import { NotFoundPage } from '@/pages/errors/404';
import { createFileRoute } from '@tanstack/react-router';


export const Route = createFileRoute('/$404')({
  component: NotFoundPage,
});

