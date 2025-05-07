import { IconButton, Tooltip } from "@mui/material";
import { Moon, Sun } from "lucide-react";
import { useThemeStore } from "../hooks/useThemeMode";
import { useTranslation } from "react-i18next";

export const ThemeToggle = () => {
  const { mode, toggleTheme } = useThemeStore();
  const { t } = useTranslation();

  const nextMode = mode === "light" ? "dark" : "light";

  return (
    <Tooltip title={t("tooltip.theme", { mode: nextMode })}>
      <IconButton onClick={toggleTheme} color="inherit">
        {mode === "light" ? <Moon /> : <Sun />}
      </IconButton>
    </Tooltip>
  );
};
