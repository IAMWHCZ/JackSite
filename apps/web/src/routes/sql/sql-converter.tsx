import SqlConverterPage from '@/pages/sql/sql-converter';
import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/sql/sql-converter')({
    component: SqlConverterPage
});