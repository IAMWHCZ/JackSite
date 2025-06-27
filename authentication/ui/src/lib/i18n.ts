import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import LanguageDetector from 'i18next-browser-languagedetector';
import commonCN from '@/assets/languages/zhCN/common.json';
import commonEN from '@/assets/languages/enUs/common.json';
import LoginCN from '@/assets/languages/zhCN/login.json';
import LoginEN from '@/assets/languages/enUs/login.json';
import RegisterEN from '@/assets/languages/enUs/register.json';
import RegisterCN from '@/assets/languages/zhCN/register.json';
import userCn from '@/assets/languages/zhCN/user.json';
import userEn from '@/assets/languages/enUS/user.json';
i18n
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    resources: {
      zh: {
        common: commonCN,
        login: LoginCN,
        register: RegisterCN,
        user: userCn,
      },
      en: {
        common: commonEN,
        login: LoginEN,
        register: RegisterEN,
        user: userEn,
      },
    },
    fallbackLng: 'zh',
    interpolation: {
      escapeValue: false,
    },
    defaultNS: 'common', // 默认命名空间改为 common
  });

export default i18n;
