import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import LanguageDetector from 'i18next-browser-languagedetector';

// 导入通用翻译文件
import zhCN from '@/assets/locales/zh_CN.json';
import enUS from '@/assets/locales/en_US.json';


i18n
    .use(LanguageDetector)
    .use(initReactI18next)
    .init({
        resources: {
            zh: {
                translation: zhCN     // 通用翻译
            },
            en: {
                translation: enUS
            }
        },
        fallbackLng: 'zh',
        interpolation: {
            escapeValue: false
        },
        ns: ['translation', 'gateway'], // 包含所有命名空间
        defaultNS: 'translation'        // 默认命名空间为通用翻译
    });

export default i18n;

