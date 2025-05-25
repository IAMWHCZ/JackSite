import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import LanguageDetector from 'i18next-browser-languagedetector';
import commonZh from '@/assets/languages/zhCN/common.json';
import commonEn from '@/assets/languages/enUs/common.json';
i18n
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    resources: {
      zh: {
        common: commonZh,
      },
      en: {
        common: commonEn,
      },
    },
    fallbackLng: 'zh',
    interpolation: {
      escapeValue: false,
    },
    defaultNS: 'common', // 默认命名空间改为 common
  });

export default i18n;
