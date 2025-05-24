import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import LanguageDetector from 'i18next-browser-languagedetector';

// 导入翻译文件
import zhCommon from '@/assets/locales/zh_CN/common.json';
import enCommon from '@/assets/locales/en_US/common.json';
import zhLogin from '@/assets/locales/zh_CN/login.json';
import enLogin from '@/assets/locales/en_US/login.json';

i18n.use(LanguageDetector)
    .use(initReactI18next)
    .init({
        resources: {
            zh: {
                common: zhCommon,
                login: zhLogin
            },
            en: {
                common: enCommon,
                login: enLogin
            },
        },
        fallbackLng: 'zh',
        interpolation: {
            escapeValue: false,
        },
        ns: ['common', 'gateway'], // 命名空间要与 resources 中的键名匹配
        defaultNS: 'common', // 默认命名空间改为 common
    });

export default i18n;
