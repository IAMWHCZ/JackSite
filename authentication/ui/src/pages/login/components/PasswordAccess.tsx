import { useTranslation } from 'react-i18next';
import { useForm } from '@tanstack/react-form';
import { useState } from 'react';
import { Eye, EyeOff, Lock, Mail } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Checkbox } from '@/components/ui/checkbox';
import { cn } from '@/lib/utils';
import { useTheme } from '@/contexts/ThemeContext';

interface PasswordFormData {
    account: string;
    password: string;
    rememberMe: boolean;
}

export const PasswordAccess = () => {
    const { t } = useTranslation('login');
    const [showPassword, setShowPassword] = useState(false);
    const [isLoading, setIsLoading] = useState(false);
    const { theme } = useTheme();
    const form = useForm({
        defaultValues: {
            account: '',
            password: '',
            rememberMe: false,
        } as PasswordFormData,
        onSubmit: async ({ value }) => {
            setIsLoading(true);
            try {
                console.log('Form submitted with values:', value);
                // 这里可以调用登录 API
                // await loginWithPassword(value)

                // 模拟登录请求
                await new Promise(resolve => setTimeout(resolve, 2000));
            } catch (error) {
                console.error('Login failed:', error);
            } finally {
                setIsLoading(false);
            }
        },
    });

    return (
        <form
            onSubmit={e => {
                e.preventDefault();
                form.handleSubmit();
            }}
            className="space-y-6"
        >
            {/* 账户输入 */}
            <form.Field
                name="account"
                validators={{
                    onChange: ({ value }) => {
                        if (!value) return t('auth.account_required');
                        if (value.includes('@')) {
                            // 邮箱验证
                            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                            if (!emailRegex.test(value)) return t('auth.invalid_email');
                        } else {
                            // 手机号验证
                            const phoneRegex = /^1[3-9]\d{9}$/;
                            if (!phoneRegex.test(value)) return t('auth.invalid_phone');
                        }
                        return undefined;
                    },
                }}
            >
                {field => (
                    <div className="space-y-2">
                        <Label
                            htmlFor="account"
                            className="text-sm font-medium text-slate-700 dark:text-slate-300"
                        >
                            {t('auth.account')}
                        </Label>
                        <div className="relative">
                            <Mail className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 transform text-slate-400" />
                            <Input
                                id="account"
                                type="text"
                                placeholder={t('auth.account_placeholder')}
                                value={field.state.value}
                                onChange={e => field.handleChange(e.target.value)}
                                onBlur={field.handleBlur}
                                className={cn(
                                    'h-12 border-slate-200 pl-10 transition-colors focus:border-blue-500 dark:border-slate-700 dark:focus:border-blue-400',
                                    field.state.meta.errors.length > 0 && 'border-red-500 dark:border-red-400'
                                )}
                                required
                            />
                        </div>
                        {field.state.meta.errors.length > 0 && (
                            <p className="text-sm text-red-600 dark:text-red-400">{field.state.meta.errors[0]}</p>
                        )}
                    </div>
                )}
            </form.Field>

            {/* 密码输入 */}
            <form.Field
                name="password"
                validators={{
                    onChange: ({ value }) => {
                        if (!value) return t('auth.password_required');
                        if (value.length < 6) return t('auth.password_min_length');
                        return undefined;
                    },
                }}
            >
                {field => (
                    <div className="space-y-2">
                        <Label
                            htmlFor="password"
                            className="text-sm font-medium text-slate-700 dark:text-slate-300"
                        >
                            {t('auth.password')}
                        </Label>
                        <div className="relative">
                            <Lock className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 transform text-slate-400" />
                            <Input
                                id="password"
                                type={showPassword ? 'text' : 'password'}
                                placeholder={t('auth.password_placeholder')}
                                value={field.state.value}
                                onChange={e => field.handleChange(e.target.value)}
                                onBlur={field.handleBlur}
                                className={cn(
                                    'h-12 border-slate-200 pl-10 pr-10 transition-colors focus:border-blue-500 dark:border-slate-700 dark:focus:border-blue-400',
                                    field.state.meta.errors.length > 0 && 'border-red-500 dark:border-red-400'
                                )}
                                required
                            />
                            <button
                                type="button"
                                onClick={() => setShowPassword(!showPassword)}
                                className="absolute right-3 top-1/2 -translate-y-1/2 transform text-slate-400 transition-colors hover:text-slate-600 dark:hover:text-slate-300"
                            >
                                {showPassword ? <EyeOff className="h-4 w-4" /> : <Eye className="h-4 w-4" />}
                            </button>
                        </div>
                        {field.state.meta.errors.length > 0 && (
                            <p className="text-sm text-red-600 dark:text-red-400">{field.state.meta.errors[0]}</p>
                        )}
                    </div>
                )}
            </form.Field>

            {/* 记住我选项 */}
            <form.Field name="rememberMe">
                {field => (
                    <div className="flex items-center justify-between">
                        <div className="flex items-center space-x-2">
                            <Checkbox
                                id="rememberMe"
                                checked={field.state.value}
                                onCheckedChange={checked => field.handleChange(checked as boolean)}
                            />
                            <Label
                                htmlFor="rememberMe"
                                className="cursor-pointer text-sm text-slate-600 dark:text-slate-400"
                            >
                                {t('auth.remember_me')}
                            </Label>
                        </div>
                        <button
                            type="button"
                            className="text-sm font-medium text-blue-600 transition-colors hover:text-blue-700 dark:text-blue-400 dark:hover:text-blue-300"
                        >
                            {t('auth.forgot_password')}
                        </button>
                    </div>
                )}
            </form.Field>

            {/* 登录按钮 */}
            <form.Subscribe selector={state => [state.canSubmit, state.isSubmitting]}>
                {([canSubmit, isSubmitting]) => (
                    <Button
                        type="submit"
                        disabled={!canSubmit || isSubmitting || isLoading}
                        className={cn(
                            'h-12 w-full font-semibold rounded-lg shadow-lg transition-all duration-300',
                            theme === 'light'
                                ? 'bg-black text-white shadow-black/20 hover:bg-gray-900 hover:shadow-black/30'
                                : 'bg-white text-black shadow-gray-900/20 hover:bg-gray-50 hover:shadow-gray-900/30',
                            'hover:shadow-xl hover:-translate-y-0.5 active:translate-y-0 active:shadow-md',
                            'focus:outline-none focus:ring-4',
                            theme === 'light'
                                ? 'focus:ring-gray-500/30'
                                : 'focus:ring-gray-400/30',
                            'disabled:cursor-not-allowed disabled:opacity-60 disabled:transform-none disabled:shadow-none'
                        )}
                    >
                        {isSubmitting || isLoading ? (
                            <div className="flex items-center justify-center space-x-3">
                                <div className={cn(
                                    "h-5 w-5 animate-spin rounded-full border-2 border-opacity-30",
                                    theme === 'light'
                                        ? "border-white border-t-white/90"
                                        : "border-black border-t-black/90"
                                )} />
                                <span className="text-sm tracking-wide">{t('auth.signing_in')}</span>
                            </div>
                        ) : (
                            <span className="text-sm tracking-wide">{t('auth.sign_in')}</span>
                        )}
                    </Button>
                )}
            </form.Subscribe>
        </form>
    );
};
