import { useEffect } from 'react';
import { useTranslation } from 'react-i18next';

export const DocumentTitle = () => {
  const { t, i18n } = useTranslation();

  useEffect(() => {
    document.title = t('title');
  }, [t, i18n.language]); // 当语言改变时更新标题

  return null;
};