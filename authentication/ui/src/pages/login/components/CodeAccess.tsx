import { useTranslation } from 'react-i18next';
import { useForm } from '@tanstack/react-form';
import { useState, useEffect } from 'react';
import { RotateCcw, Mail } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Checkbox } from '@/components/ui/checkbox';
import { cn } from '@/lib/utils';
import { useTheme } from '@/contexts/ThemeContext';

interface CodeFormData {
    account: string;
    smsCode: string;
    rememberMe: boolean;
}

export const CodeAccess = () => {
    const { t } = useTranslation('login');
    const [isLoading, setIsLoading] = useState(false);
    const [isSendingCode, setIsSendingCode] = useState(false);
    const [countdown, setCountdown] = useState(0);
    const { theme } = useTheme();

    // 短信验证码倒计时
    useEffect(() => {
        if (countdown > 0) {
            const timer = setTimeout(() => setCountdown(countdown - 1), 1000);
            return () => clearTimeout(timer);
        }
    }, [countdown]);

    const form = useForm({
        defaultValues: {
            account: '',
            smsCode: '',
            rememberMe: false,
        } as CodeFormData,
        onSubmit: async ({ value }: { value: CodeFormData }) => {
            setIsLoading(true);

            try {
                console.log('SMS login submitted with values:', value);
                // 这里可以调用短信验证码登录 API
                // await loginWithSmsCode(value)

                // 模拟登录请求
                await new Promise(resolve => setTimeout(resolve, 2000));
            } catch (error) {
                console.error('SMS login failed:', error);
            } finally {
                setIsLoading(false);
            }
        },
    });

    const handleSendSmsCode = async () => {
        const accountValue = form.getFieldValue('account');
        if (!accountValue || isSendingCode || countdown > 0) return;

        setIsSendingCode(true);

        try {
            console.log('Sending SMS code to:', accountValue);
            // 这里可以调用发送短信验证码 API
            // await sendSmsCode(accountValue)

            // 模拟发送短信验证码
            await new Promise(resolve => setTimeout(resolve, 1000));
            setCountdown(60); // 60秒倒计时
        } catch (error) {
            console.error('Failed to send SMS code:', error);
        } finally {
            setIsSendingCode(false);
        }
    };

    return (
        <form
            onSubmit={e => {
                e.preventDefault();
                form.handleSubmit();
            }}
            className="space-y-6"
        >
            {/* 账号输入 */}
            <form.Field
                name="account"
                validators={{
                    onChange: ({ value }: { value: string }) => {
                        if (!value) return t('auth.account_required');
                    },
                }}
            >
                {field => (
                    <div className="space-y-2 ml-3">
                        <Label
                            htmlFor="account"
                            className="text-sm font-medium text-slate-700 dark:text-slate-300"
                        >
                            {t('auth.account')}
                        </Label>
                        <div className="relative ">
                            <Mail className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 transform text-slate-400" />
                            <Input
                                id="account"
                                type="tel"
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

            {/* 验证码输入 */}
            <form.Field
                name="smsCode"
                validators={{
                    onChange: ({ value }: { value: string }) => {
                        if (!value) return t('auth.sms_code_required');
                        if (value.length !== 6) return t('auth.sms_code_length');
                        return undefined;
                    },
                }}
            >
                {field => (
                    <div className="space-y-2 ml-3">
                        <Label
                            htmlFor="smsCode"
                            className="text-sm font-medium text-slate-700 dark:text-slate-300"
                        >
                            {t('auth.sms_code')}
                        </Label>
                        <div className="flex gap-2">
                            <div className="relative flex-1">
                                <Input
                                    id="smsCode"
                                    type="text"
                                    placeholder={t('auth.sms_code_placeholder')}
                                    value={field.state.value}
                                    onChange={e => field.handleChange(e.target.value)}
                                    onBlur={field.handleBlur}
                                    className={cn(
                                        'h-12 border-slate-200 transition-colors focus:border-blue-500 dark:border-slate-700 dark:focus:border-blue-400',
                                        field.state.meta.errors.length > 0 && 'border-red-500 dark:border-red-400'
                                    )}
                                    maxLength={6}
                                    required
                                />
                            </div>
                            <Button
                                type="button"
                                variant="outline"
                                onClick={handleSendSmsCode}
                                disabled={!form.getFieldValue('account') || isSendingCode || countdown > 0}
                                className="h-12 whitespace-nowrap border-slate-200 px-4 hover:bg-slate-50 dark:border-slate-700 dark:hover:bg-slate-800"
                            >
                                {isSendingCode ? (
                                    <div className="flex items-center gap-2">
                                        <RotateCcw className="h-4 w-4 animate-spin" />
                                        {t('auth.sending')}
                                    </div>
                                ) : countdown > 0 ? (
                                    `${countdown}s`
                                ) : (
                                    t('auth.send_code')
                                )}
                            </Button>
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
                )}
            </form.Field>

            {/* 登录按钮 */}
            <form.Subscribe selector={state => [state.canSubmit, state.isSubmitting]}>
                {([canSubmit, isSubmitting]) => (
                    <Button
                        type="submit"
                        disabled={!canSubmit || isSubmitting || isLoading}
                        className={cn(
                            'h-12 w-full font-semibold rounded-lg transition-all duration-300',
                            theme === 'light'
                                ? 'bg-black text-white hover:bg-gray-900'
                                : 'bg-white text-black hover:bg-gray-50',
                            'hover:-translate-y-0.5 active:translate-y-0',
                            'focus:outline-none focus:ring-4',
                            theme === 'light'
                                ? 'focus:ring-gray-500/30'
                                : 'focus:ring-gray-400/30',
                            'disabled:cursor-not-allowed disabled:opacity-60 disabled:transform-none'
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
