import {createFileRoute} from '@tanstack/react-router';
import SqlToDiagramPage from "@/pages/sql/sql-to-diagram";

export const Route = createFileRoute('/sql/sql-to-diagram')({
    component: SqlToDiagramPage
});