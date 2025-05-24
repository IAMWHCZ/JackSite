import DashboardIcon from '@mui/icons-material/Dashboard';
import PeopleIcon from '@mui/icons-material/People';
import SettingsIcon from '@mui/icons-material/Settings';

export const navigationConfig = [
  {
    title: '仪表盘',
    path: '/',
    icon: DashboardIcon,
  },
  {
    title: '用户管理',
    path: '/users',
    icon: PeopleIcon,
    children: [
      {
        title: '用户列表',
        path: '/users/list',
      },
      {
        title: '用户组',
        path: '/users/groups',
      },
    ],
  },
  {
    title: '系统设置',
    path: '/settings',
    icon: SettingsIcon,
  },
];