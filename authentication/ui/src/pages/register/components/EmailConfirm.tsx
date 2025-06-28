import useRegisterStore from '@/stores/register';
import { useForm } from '@tanstack/react-form';
import type { RegisterForm } from '../models/register';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { useTranslation } from 'react-i18next';
import { Mail, UserLock } from 'lucide-react';
import { cn } from '@/lib/utils';
import { Button } from '@/components/ui/button';
import { useEffect, useRef } from 'react';
import { EmailService } from '@/services/email.service';
import { useMutation, useQuery } from '@tanstack/react-query';
import { SendEmailType } from '@/enums/email';
import { toast } from 'sonner';
import { UserService } from '@/services/user.service';
import { useNavigate } from '@tanstack/react-router';

export const EmailConfirm = () => {
    const {
        register,
        setRegister,
        isSendCode,
        setIsSendCode,
        sendCodeCountDown,
        setSendCodeCountDown
    } = useRegisterStore();
    const { data: isSend, isLoading: isSendLoading, refetch: sendRefetch }
        = useQuery(
            {
                queryKey: ['send-code', register.account],
                queryFn: async () =>
                    await EmailService.sendVerificationCode(form.state.values.email!, SendEmailType.registerUser,),
                enabled: false,
                retry: false
            }
        )
    const { data, isPending: registerLoading, mutateAsync } = useMutation({
        mutationKey: ['register-user'],
        mutationFn: UserService.RegisterUser
    })
    const timerRef = useRef<ReturnType<typeof setTimeout> | null>(null);
    const { t } = useTranslation()
    const { t: r } = useTranslation('register');
    const navigate = useNavigate();
    const form = useForm({
        defaultValues: {
            ...register,
            email: '',
            validationCode: '',
        } as RegisterForm,
        onSubmit: async ({ value }) => {
            setRegister({ ...value });
            await mutateAsync(register);
            if (!registerLoading) {
                console.log('data', data);
                if (data?.success) {
                    toast.success(r('register_success'));
                } else {
                    toast.error(data?.message || r('register_failed'));
                }
            }
            await navigate({
                to: '/login', replace: true, search: {
                    account: register.account
                }
            });
        },
    });

    useEffect(() => {
        // 只在 isSendCode 为 true 时启动计时器
        if (isSendCode && !timerRef.current) {
            timerRef.current = setInterval(() => {
                setSendCodeCountDown(prev => {
                    if (prev <= 1) {
                        setIsSendCode(false);
                        if (timerRef.current) {
                            clearInterval(timerRef.current);
                            timerRef.current = null;
                        }
                        return 120; // 重置为初始值
                    }
                    return prev - 1;
                });
            }, 1000);
        }
        // 在组件卸载时清理计时器
        return () => {
            if (timerRef.current) {
                clearInterval(timerRef.current);
                timerRef.current = null;
            }
        };
    }, [isSendCode]); // 添加 isSendCode 作为依赖

    const sendCode = async () => {
        setIsSendCode(true);
        await sendRefetch()
        timerRef.current = setInterval(() => {
            setSendCodeCountDown(prev => {
                if (prev <= 1) {
                    setIsSendCode(false);
                    if (timerRef.current) {
                        clearInterval(timerRef.current);
                        timerRef.current = null;
                    }
                    return 0;
                }
                return prev - 1;
            });
        }, 1000);
        if (isSend) {
            toast.success(r('validation_code_send'));
        }
    }


    return (
        <form
            onSubmit={e => {
                e.preventDefault();
                e.stopPropagation();
                form.handleSubmit();
            }}
        >
            <form.Field
                name="email"
                validators={{
                    onBlurAsync: async ({ value }) => {
                        if (!value || value.trim() === '') {
                            return r('email_required')
                        }

                        if (!value.isEmail()) {
                            return r('email_invalid');
                        }

                        const isBind = await EmailService.checkEmailBinding(value)

                        if (isBind) {
                            return r('email_exists');
                        }

                        return undefined
                    }
                }}
                children={field => (
                    <div className='w-full'>
                        <Label
                            htmlFor="email"
                            className="text-sm font-medium text-slate-700 dark:text-slate-300"
                        >
                            {r('email')}
                        </Label>
                        <div className="relative mt-1">
                            <Mail className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 transform text-slate-400" />
                            <Input
                                id="email"
                                type="email"
                                placeholder={r('email_placeholder')}
                                value={field.state.value}
                                onChange={e => {
                                    field.setValue(e.target.value);
                                }}
                                onBlur={field.handleBlur}
                                className={cn(
                                    'h-12 border-slate-200 pl-10 transition-colors focus:border-blue-500 dark:border-slate-700 dark:focus:border-blue-400',
                                    field.state.meta.errors.length > 0 && 'border-red-500 dark:border-red-400'
                                )}
                                required
                            />
                        </div>
                        {
                            field.state.meta.errors.length > 0 && (
                                <p className="mt-1 text-sm text-red-500 dark:text-red-400">
                                    {field.state.meta.errors[0]}
                                </p>
                            )
                        }
                    </div>
                )}
            />
            <form.Field
                name='validationCode'
                children={field =>
                    <div className='relative mt-4 w-full'>
                        <Label
                            htmlFor="validationCode"
                            className="text-sm font-medium text-slate-700 dark:text-slate-300"
                        >
                            {r('validation_code')}
                        </Label>
                        <div className="relative mt-1 w-full">
                            <div className='flex items-center gap-2 w-full'>
                                <div className="relative flex-1">
                                    <UserLock className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 transform text-slate-400 z-10" />
                                    <Input
                                        loading={isSendLoading}
                                        id="validationCode"
                                        type="text"
                                        placeholder={r('validation_code_placeholder')}
                                        value={field.state.value}
                                        onChange={e => {
                                            field.setValue(e.target.value);
                                        }}
                                        maxLength={6}
                                        className={cn(
                                            'h-12 w-full border-slate-200 pl-10 transition-colors focus:border-blue-500 dark:border-slate-700 dark:focus:border-blue-400',
                                            field.state.meta.errors.length > 0 && 'border-red-500 dark:border-red-400'
                                        )}
                                        required
                                    />
                                </div>
                                <Button
                                    type="button"
                                    onClick={sendCode}
                                    disabled={isSendCode}
                                    className={cn(
                                        'h-12 flex-shrink-0 whitespace-nowrap px-4 text-sm font-medium',
                                        'min-w-[100px]', // 设置最小宽度确保按钮不会太小
                                        isSendCode && 'cursor-not-allowed opacity-75'
                                    )}
                                >
                                    {!isSendCode ? r('validation_code_send') : sendCodeCountDown}
                                </Button>
                            </div>
                        </div>
                    </div>
                }
            />
            <form.Subscribe
                selector={state => [state.canSubmit, state.isSubmitting]}
                children={([canSubmit, isSubmitting]) => (
                    <div className="mt-6 flex items-center justify-center">
                        <Button loading={registerLoading} type="submit" className="h-12 w-full" disabled={!canSubmit || isSubmitting}>
                            {isSubmitting ? t('submitting') || '提交中...' : t('submit')}
                        </Button>
                    </div>
                )}
            />
        </form>
    );
};
