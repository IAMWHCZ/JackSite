import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Lock, Smartphone } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { PasswordAccess } from './components/PasswordAccess';
import { CodeAccess } from './components/CodeAccess';
import WeChat from '@/assets/icons/wechat';
import QQ from '@/assets/icons/qq';
import { cn } from '@/lib/utils';
import { useTheme } from '@/contexts/ThemeContext';
import { useNavigate } from '@tanstack/react-router';

type LoginType = 'password' | 'sms';

export const LoginPage = () => {
    const { t } = useTranslation('login');
    const [loginType, setLoginType] = useState<LoginType>('password');
    const { theme } = useTheme();
    const navigate = useNavigate();
    const handleRegister = () => {
        navigate({ to: '/register' });
    };
    return (
        <div className="flex min-h-0 flex-1 items-center justify-center bg-gradient-to-br from-slate-50 to-slate-100 p-4 dark:from-gray-900 dark:via-black dark:to-gray-800">
            {/* 背景装饰 - 支持主题切换 */}
            <div className="absolute inset-0 overflow-hidden">
                <div className="absolute -right-40 -top-40 h-80 w-80 animate-pulse rounded-full bg-blue-400/20 blur-3xl dark:bg-white/5" />
                <div className="absolute -bottom-40 -left-40 h-80 w-80 animate-pulse rounded-full bg-purple-400/20 blur-3xl delay-1000 dark:bg-gray-300/5" />
                <div className="dark:from-white/3 dark:to-gray-400/3 absolute left-1/2 top-1/2 h-96 w-96 -translate-x-1/2 -translate-y-1/2 transform rounded-full bg-gradient-to-r from-blue-400/10 to-purple-400/10 opacity-50 blur-3xl" />
            </div>

            <Card className="animate-in fade-in-0 zoom-in-95 relative w-full max-w-md border-0 bg-white/80 shadow-2xl backdrop-blur-xl duration-500 dark:border dark:border-gray-800/50 dark:bg-black/80">
                <CardHeader className="space-y-2 pb-6">
                    <div className="mb-4 flex justify-center">
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
                        {t('auth.welcome_back')}
                    </CardTitle>
                    <CardDescription className="animate-in slide-in-from-top-4 duration-600 delay-400 text-center text-slate-600 dark:text-gray-400">
                        {t('auth.login_description')}
                    </CardDescription>
                </CardHeader>

                <CardContent>
                    <Tabs
                        value={loginType}
                        onValueChange={value => setLoginType(value as LoginType)}
                        className="w-full"
                    >
                        {/* 主题适配的 TabsList */}
                        <TabsList className="animate-in slide-in-from-top-4 duration-600 mb-6 grid w-full grid-cols-2 border-0 bg-slate-100/50 backdrop-blur-sm delay-500 dark:border dark:border-gray-800 dark:bg-gray-900/80">
                            <TabsTrigger
                                value="password"
                                className={cn(
                                    'flex items-center gap-2 transition-all duration-300 ease-in-out',
                                    'data-[state=active]:text-blue-600 dark:data-[state=active]:text-white',
                                    'text-slate-600 dark:text-gray-400',
                                    'hover:scale-[1.01] hover:text-slate-800 hover:shadow-md dark:hover:text-gray-200'
                                )}
                            >
                                <Lock
                                    className={cn(
                                        'h-4 w-4 transition-all duration-300',
                                        loginType === 'password'
                                            ? 'scale-110 text-blue-600 dark:text-white'
                                            : 'text-slate-600 dark:text-gray-400'
                                    )}
                                />
                                {t('auth.password_login')}
                            </TabsTrigger>
                            <TabsTrigger
                                value="sms"
                                className={cn(
                                    'flex items-center gap-2 transition-all duration-300 ease-in-out',
                                    'data-[state=active]:text-blue-600 dark:data-[state=active]:text-white',
                                    'text-slate-600 dark:text-gray-400',
                                    'hover:scale-[1.01] hover:text-slate-800 hover:shadow-md dark:hover:text-gray-200'
                                )}
                            >
                                <Smartphone
                                    className={cn(
                                        'h-4 w-4 transition-all duration-300',
                                        loginType === 'sms'
                                            ? 'scale-110 text-blue-600 dark:text-white'
                                            : 'text-slate-600 dark:text-gray-400'
                                    )}
                                />
                                {t('auth.sms_login')}
                            </TabsTrigger>
                        </TabsList>

                        {/* 左右滑动动画 */}
                        <div className="relative min-h-[300px] overflow-hidden">
                            <div
                                className={cn(
                                    'flex w-[200%] transition-transform duration-500 ease-in-out',
                                    loginType === 'password' ? 'transform-none' : '-translate-x-1/2'
                                )}
                            >
                                <div className="w-1/2 flex-shrink-0">
                                    <TabsContent value="password" className="mt-0 space-y-6">
                                        <PasswordAccess />
                                    </TabsContent>
                                </div>
                                <div className="w-1/2 flex-shrink-0">
                                    <TabsContent value="sms" className="mt-0 space-y-6">
                                        <CodeAccess />
                                    </TabsContent>
                                </div>
                            </div>
                        </div>

                        {/* 主题适配的分割线 */}
                        <div className="animate-in fade-in-0 slide-in-from-bottom-4 duration-600 relative delay-700">
                            <div className="absolute inset-0 flex items-center">
                                <div className="w-full border-t border-slate-200 dark:border-gray-800" />
                            </div>
                            <div className="relative flex justify-center text-sm">
                                <span className="bg-white px-2 text-slate-500 dark:bg-black dark:text-gray-500">
                                    {t('auth.or_continue_with')}
                                </span>
                            </div>
                        </div>

                        {/* 主题适配的第三方登录 */}
                        <div className="animate-in fade-in-0 slide-in-from-bottom-4 duration-600 delay-800 grid grid-cols-2 gap-3">
                            <Button
                                type="button"
                                variant="outline"
                                className={cn(
                                    'h-12 border-slate-200 dark:border-gray-800',
                                    'bg-slate-50 dark:bg-gray-900/50',
                                    'hover:bg-slate-100 dark:hover:bg-gray-800/80',
                                    'text-slate-700 dark:text-gray-300',
                                    'hover:text-slate-900 dark:hover:text-white',
                                    'transition-all duration-300 ease-in-out',
                                    'hover:scale-[1.02] hover:shadow-lg',
                                    'hover:border-green-300 dark:hover:border-gray-600',
                                    'active:scale-[0.98]'
                                )}
                            >
                                <WeChat className="transition-transform duration-300 hover:scale-110" />
                                <span className="ml-2">WeChat</span>
                            </Button>
                            <Button
                                type="button"
                                variant="outline"
                                className={cn(
                                    'h-12 border-slate-200 dark:border-gray-800',
                                    'bg-slate-50 dark:bg-gray-900/50',
                                    'hover:bg-slate-100 dark:hover:bg-gray-800/80',
                                    'text-slate-700 dark:text-gray-300',
                                    'hover:text-slate-900 dark:hover:text-white',
                                    'transition-all duration-300 ease-in-out',
                                    'hover:scale-[1.02] hover:shadow-lg',
                                    'hover:border-blue-300 dark:hover:border-gray-600',
                                    'active:scale-[0.98]'
                                )}
                            >
                                <QQ className="transition-transform duration-300 hover:scale-110" />
                                <span className="ml-2">QQ</span>
                            </Button>
                        </div>

                        {/* 主题适配的注册链接 */}
                        <div className="animate-in fade-in-0 slide-in-from-bottom-4 duration-600 mt-6 text-center delay-1000">
                            <span className="text-sm text-slate-600 dark:text-gray-500">
                                {t('auth.no_account')}
                            </span>
                            <button
                                onClick={handleRegister}
                                className="ml-1 text-sm font-medium text-blue-600 underline underline-offset-4 transition-all duration-300 hover:scale-105 hover:text-blue-700 hover:underline-offset-2 active:scale-95 dark:text-white dark:hover:text-gray-300"
                            >
                                {t('auth.sign_up')}
                            </button>
                        </div>
                    </Tabs>
                </CardContent>
            </Card>
        </div>
    );
};
