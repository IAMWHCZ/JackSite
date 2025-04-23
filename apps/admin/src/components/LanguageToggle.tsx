import { IconButton, Tooltip, CircularProgress } from "@mui/material";
import { useTranslation } from "react-i18next";
import { Languages } from "lucide-react";
import { useState } from "react";
import { toast } from "react-hot-toast";

export const LanguageToggle = () => {
  const { i18n, t } = useTranslation();
  const [isLoading, setIsLoading] = useState(false);

  const toggleLanguage = async () => {
    try {
      setIsLoading(true);
      const nextLang = i18n.language === 'en' ? 'zh' : 'en';
      
      // 使用 Promise 包装语言切换
      await i18n.changeLanguage(nextLang);
      
      // 可选: 保存用户语言偏好
      localStorage.setItem('preferred-language', nextLang);
      
    } catch (error) {
      console.error('Failed to change language:', error);
      toast.error(t('error.languageChange'));
    } finally {
      setIsLoading(false);
    }
  };

  const nextLang = i18n.language === 'en' ? '中文' : 'English';

  return (
    <Tooltip title={isLoading ? t('loading.language') : t('tooltip.language', { lang: nextLang })}>
      <span> {/* 使用 span 包装是因为在加载时按钮被禁用,Tooltip 需要可用的子元素 */}
        <IconButton 
          onClick={toggleLanguage} 
          color="inherit"
          disabled={isLoading}
          sx={{
            position: 'relative',
            '& .loading-spinner': {
              position: 'absolute',
              top: '50%',
              left: '50%',
              marginTop: '-12px',
              marginLeft: '-12px',
            }
          }}
        >
          {isLoading ? (
            <div className="w-full h-full items-center justify-center">
              <CircularProgress 
                size={100} 
                className="loading-spinner"
                sx={{
                  color: (theme) => 
                    theme.palette.mode === 'dark' ? 'grey.300' : 'grey.800'
                }}
              />
            </div>
          ) : (
            <Languages />
          )}
        </IconButton>
      </span>
    </Tooltip>
  );
};
