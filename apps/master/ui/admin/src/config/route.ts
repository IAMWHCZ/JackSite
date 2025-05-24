import { Cctv, ChartNoAxesCombined, FileClock, FileText, LayoutDashboard, LucideProps, Newspaper, ShieldUser, Signature, User } from 'lucide-react';
import { ForwardRefExoticComponent, RefAttributes } from 'react';

export interface NavProps {
    labelKey: string;
    icon: ForwardRefExoticComponent<
        Omit<LucideProps, 'ref'> & RefAttributes<SVGSVGElement>
    >;
    path: string;
    breadcrumb: string;
}

export const navItems: NavProps[] = [
    { labelKey: 'home', icon: LayoutDashboard, path: '/', breadcrumb: 'home' },
    { labelKey: 'user', icon: User, path: '/user', breadcrumb: 'user' },
    { labelKey: 'role', icon: ShieldUser, path: '/role', breadcrumb: 'role' },
    { labelKey: 'blog', icon: Newspaper, path: '/blog', breadcrumb: 'blog' },
    { labelKey: 'monitor', icon: Cctv, path: '/monitor', breadcrumb: 'monitor' },
    { labelKey: 'diagram', icon: ChartNoAxesCombined, path: '/diagram', breadcrumb: 'diagram' },
    { labelKey: 'signature', icon: Signature, path: '/signature', breadcrumb: 'signature' },
    { labelKey: 'file', icon: FileText, path: '/file', breadcrumb: 'file' },
    { labelKey: 'log', icon: FileClock, path: '/log', breadcrumb: 'log' }
];
