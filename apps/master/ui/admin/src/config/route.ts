import { LayoutDashboard, LucideProps, User } from 'lucide-react';
import { ForwardRefExoticComponent, RefAttributes } from 'react';

export interface NavProps {
    labelKey: string;
    icon?: ForwardRefExoticComponent<
        Omit<LucideProps, 'ref'> & RefAttributes<SVGSVGElement>
    >;
    path: string;
}

export const navItems: NavProps[] = [
    { labelKey: 'home', icon: LayoutDashboard, path: '/' },
    { labelKey: 'user', icon: User, path: '/user' },
];
