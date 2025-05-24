import {
  AppBar,
  Box,
  IconButton,
  Toolbar,
  Typography,
  useTheme,
  Avatar,
  Menu,
  MenuItem,
  Tooltip,
} from "@mui/material";
import { ThemeToggle } from "../ThemeToggle";
import { LanguageToggle } from "../LanguageToggle";
import { useState } from "react";
import { PanelRightClose, PanelRightOpen } from "lucide-react";
import { useSystemStore } from "@/stores/system";
import { menuItems } from "@/config/menu";
import { useTranslation } from "react-i18next";

export const Header = () => {
  const theme = useTheme();
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const { isSideBarCollapsed, setSideBarCollapsed, breadcrumb } = useSystemStore();
  const { t } = useTranslation();

  const handleMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  return (
    <AppBar
      position="fixed"
      sx={{
        backgroundColor: theme.palette.background.paper,
        color: theme.palette.text.primary,
        boxShadow: 1,
        width: `calc(100% - ${isSideBarCollapsed ? 84 : 240}px)`,
        marginLeft: isSideBarCollapsed ? "64px" : "240px",
        transition: "width 0.2s ease-in-out, margin-left 0.2s ease-in-out",
        zIndex: theme.zIndex.drawer + 1,
      }}
    >
      <Toolbar>
        <Tooltip title={t(isSideBarCollapsed ? 'tooltip.expand' : 'tooltip.collapse')}>
          <IconButton
            color="inherit"
            onClick={() => setSideBarCollapsed(!isSideBarCollapsed)}
            edge="start"
            sx={{ mr: 2 }}
          >
            {!isSideBarCollapsed ? <PanelRightOpen /> : <PanelRightClose />}
          </IconButton>
        </Tooltip>

        <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
          {t(`breadcrumb.${breadcrumb}`)}
        </Typography>

        <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
          <ThemeToggle />
          <LanguageToggle />
          <Tooltip title={t('tooltip.account')}>
            <IconButton onClick={handleMenu} size="small" sx={{ ml: 2 }}>
              <Avatar sx={{ width: 32, height: 32 }}>A</Avatar>
            </IconButton>
          </Tooltip>
        </Box>

        <Menu
          id="menu-appbar"
          anchorEl={anchorEl}
          anchorOrigin={{
            vertical: "bottom",
            horizontal: "right",
          }}
          keepMounted
          transformOrigin={{
            vertical: "top",
            horizontal: "right",
          }}
          open={Boolean(anchorEl)}
          onClose={handleClose}
        >
          {menuItems.map(({ onPress, labelKey, icon: Icon }) => (
            <MenuItem
              key={labelKey}
              sx={{
                gap: 1,
              }}
              onClick={onPress}
            >
              {Icon && <Icon size={18} />}
              {t(labelKey)}
            </MenuItem>
          ))}
        </Menu>
      </Toolbar>
    </AppBar>
  );
};
