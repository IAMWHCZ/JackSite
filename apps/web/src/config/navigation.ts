export interface NavItem {
    title: string;
    href?: string;
    disabled?: boolean;
    external?: boolean;
    icon?: string;
    children?: NavItem[];
}

export const navigationConfig: NavItem[] = [
    {
        title: '首页',
        href: '/',
    },
    {
        title: '个人博客',
        href: '/blog',
    },
    {
        title: 'SQL工具',
        children: [
            {
                title: 'SQL转ER图',
                href: '/sql/sql-to-diagram',
            },
            {
                title: 'SQL转换器',
                href: '/sql/sql-converter',
            }
        ]
    },
    {
        title: '开发工具',
        children: [
            {
                title: 'JSON格式化', 
                href: '/json/json-format',
            }
        ]
    },
    {
        title: '日常工具',
        children: [
            {
                title: '电子签名',
                href: '/signature',
            }
        ]
    },
];