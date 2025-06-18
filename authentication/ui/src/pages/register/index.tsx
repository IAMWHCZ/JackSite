import { useTheme } from '@/contexts/ThemeContext';
import { useTranslation } from 'react-i18next';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { cn } from '@/lib/utils';
import { PasswordSetting } from './components/PasswordSetting';
import { EmailConfirm } from './components/EmailConfirm';
import useRegisterStore from '@/stores/register';
import { ChevronLeft } from 'lucide-react';
import { Link, useNavigate } from '@tanstack/react-router';
import { Tooltip, TooltipContent, TooltipTrigger } from '@/components/ui/tooltip';

export const RegisterPage = () => {
    const { t } = useTranslation('register');
    const { t: tCommon } = useTranslation('common');
    const { isCompletePassword, setIsCompletePassword, setRegister } = useRegisterStore();
    const { theme } = useTheme();
    const navigate = useNavigate();
    const handleBack = () => {
        if (isCompletePassword) {
            setIsCompletePassword(false);
        } else {
            navigate({
                to: '/login',
            });
            setRegister(null!);
        }
    };
    return (
        <div className="flex h-full items-center justify-center bg-gradient-to-br from-slate-50 to-slate-100 p-4 dark:from-gray-900 dark:via-black dark:to-gray-800">
            {/* 背景装饰 - 支持主题切换 */}
            <div className="absolute inset-0 overflow-hidden">
                <div className="absolute -right-40 -top-40 h-80 w-80 animate-pulse rounded-full bg-blue-400/20 blur-3xl dark:bg-white/5" />
                <div className="absolute -bottom-40 -left-40 h-80 w-80 animate-pulse rounded-full bg-purple-400/20 blur-3xl delay-1000 dark:bg-gray-300/5" />
                <div className="dark:from-white/3 dark:to-gray-400/3 absolute left-1/2 top-1/2 h-96 w-96 -translate-x-1/2 -translate-y-1/2 transform rounded-full bg-gradient-to-r from-blue-400/10 to-purple-400/10 opacity-50 blur-3xl" />
            </div>

            <Card className="
            animate-in fade-in-0 zoom-in-95 relative w-full max-w-md border-0 bg-white/80 shadow-2xl backdrop-blur-xl duration-500
            dark:border dark:border-gray-800/50 dark:bg-black/80">
                <CardHeader className="space-y-2 pb-6">
                    <Link
                        to="."
                        onClick={e => {
                            e.preventDefault();
                            e.stopPropagation();
                            handleBack();
                        }}
                    >
                        <Tooltip>
                            <TooltipTrigger>
                                <ChevronLeft />
                            </TooltipTrigger>
                            <TooltipContent>{tCommon('back')}</TooltipContent>
                        </Tooltip>
                    </Link>

                    <div className="flex justify-center">
                        <img src="/logo.png" alt="Logo" className="h-16 w-16" />
                    </div>
                    <CardTitle
                        className={cn(
                            'text-center text-2xl font-bold',
                            theme === 'light'
                                ? 'bg-black bg-gradient-to-r text-white dark:from-white'
                                : 'g-gradient-to-r bg-white text-black dark:from-white',
                            ' animate-in slide-in-from-top-4 duration-600 bg-clip-text text-transparent delay-300 dark:to-gray-300'
                        )}
                    >
                        {t('title')}
                    </CardTitle>
                    <CardDescription className="animate-in slide-in-from-top-4 duration-600 delay-400 text-center text-slate-600 dark:text-gray-400">
                        {t('description')}
                    </CardDescription>
                </CardHeader>
                <CardContent>{!isCompletePassword ? <PasswordSetting /> : <EmailConfirm />}</CardContent>
            </Card>
        </div>
    );
};
