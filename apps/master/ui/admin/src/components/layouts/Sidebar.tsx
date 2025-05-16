import {
    List,
    ListItem,
    ListItemIcon,
    ListItemText,
    ListItemButton,
    Box,
    Divider,
    Tooltip,
} from '@mui/material';
import { useNavigate } from '@tanstack/react-router';
import { Image } from '../common/Image';
import { useSystemStore } from '@/stores/system';
import { navItems } from '@/config/route';
import { useTranslation } from 'react-i18next';
import { Airplay } from 'lucide-react';

const DRAWER_WIDTH = 240;
const COLLAPSED_WIDTH = 64;

const sidebarStyle = {
    backgroundColor: 'black',
    color: '#f9fafb',
    borderRight: '1px solid rgba(255, 255, 255, 0.12)',
};

const menuItemStyle = {
    color: '#f9fafb',
    '&:hover': {
        backgroundColor: 'rgba(255, 255, 255, 0.08)',
    },
    '&.Mui-selected': {
        backgroundColor: 'rgba(255, 255, 255, 0.12)',
        '&:hover': {
            backgroundColor: 'rgba(255, 255, 255, 0.16)',
        },
    },
};

export const Sidebar = () => {
    const navigate = useNavigate();
    const { isSideBarCollapsed } = useSystemStore();
    const { t } = useTranslation();
    return (
        <Box
            component="aside"
            sx={{
                position: 'fixed',
                left: 0,
                top: 0,
                bottom: 0,
                width: isSideBarCollapsed ? COLLAPSED_WIDTH : DRAWER_WIDTH,
                transition: 'width 0.2s ease-in-out',
                ...sidebarStyle,
                display: 'flex',
                flexDirection: 'column',
                overflow: 'hidden',
            }}
        >
            {/* Logo区域 */}
            <Box
                sx={{
                    height: 64,
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'start',
                    p: 2,
                    borderBottom: '1px solid rgba(255, 255, 255, 0.12)',
                    gap: 2,
                    overflow: 'hidden',
                }}
            >
                <Image width={40} height={40} src="/logo.png" alt="logo" />
                {!isSideBarCollapsed && (
                    <span className="text-white text-xl">JACKSITE</span>
                )}
            </Box>

            <Divider sx={{ borderColor: 'rgba(255, 255, 255, 0.12)' }} />

            {/* 菜单列表 */}
            <List sx={{ flex: 1, overflow: 'auto' }}>
                {navItems.map(({ labelKey, icon: Icon, path }) => (
                    <ListItem key={labelKey} disablePadding>
                        {!isSideBarCollapsed ? (
                            <ListItemButton
                                onClick={() => navigate({ to: path })}
                                sx={{
                                    ...menuItemStyle,
                                    minHeight: 48,
                                    justifyContent: isSideBarCollapsed
                                        ? 'center'
                                        : 'initial',
                                    px: 2.5,
                                }}
                            >
                                <ListItemIcon
                                    sx={{
                                        color: 'inherit',
                                        minWidth: 0,
                                        mr: isSideBarCollapsed ? 0 : 3,
                                        justifyContent: 'center',
                                        transition:
                                            'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)',
                                        flexShrink: 0,
                                    }}
                                >
                                    {Icon ? <Icon /> : <Airplay />}
                                </ListItemIcon>
                                {!isSideBarCollapsed && (
                                    <ListItemText
                                        sx={{
                                            whiteSpace: 'nowrap',
                                            opacity: isSideBarCollapsed ? 0 : 1,
                                            transform: isSideBarCollapsed
                                                ? 'translateX(-20px)'
                                                : 'translateX(0)',
                                            transition:
                                                'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)',
                                        }}
                                        primary={t(`nav.${labelKey}`)}
                                    />
                                )}
                            </ListItemButton>
                        ) : (
                            <Tooltip
                                title={t(`nav.${labelKey}`)}
                                placement="right"
                                arrow
                                disableInteractive
                            >
                                <ListItemButton
                                    onClick={() => navigate({ to: path })}
                                    sx={{
                                        ...menuItemStyle,
                                        minHeight: 48,
                                        justifyContent: isSideBarCollapsed
                                            ? 'center'
                                            : 'initial',
                                        px: 2.5,
                                    }}
                                >
                                    <ListItemIcon
                                        sx={{
                                            color: 'inherit',
                                            minWidth: 0,
                                            mr: isSideBarCollapsed ? 0 : 3,
                                            justifyContent: 'center',
                                            transition:
                                                'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)',
                                            flexShrink: 0,
                                        }}
                                    >
                                        {Icon ? <Icon /> : <Airplay />}
                                    </ListItemIcon>
                                    {!isSideBarCollapsed && (
                                        <ListItemText
                                            sx={{
                                                whiteSpace: 'nowrap',
                                                opacity: isSideBarCollapsed
                                                    ? 0
                                                    : 1,
                                                transform: isSideBarCollapsed
                                                    ? 'translateX(-20px)'
                                                    : 'translateX(0)',
                                                transition:
                                                    'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)',
                                            }}
                                            primary={t(`nav.${labelKey}`)}
                                        />
                                    )}
                                </ListItemButton>
                            </Tooltip>
                        )}
                    </ListItem>
                ))}
            </List>
        </Box>
    );
};
