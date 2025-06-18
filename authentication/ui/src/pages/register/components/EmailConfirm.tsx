import useRegisterStore from '@/stores/register';
import { useField, useForm } from '@tanstack/react-form';
import type { RegisterForm } from '../models/register';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { useTranslation } from 'react-i18next';
import { Mail, UserLock } from 'lucide-react';
import { cn } from '@/lib/utils';
import { Button } from '@/components/ui/button';
import { useEffect, useRef } from 'react';
import { useCheckEmailIsBind } from '@/hooks/emails/useCheckEmailIsBind';

export const EmailConfirm = () => {
    const {
        register,
        setRegister,
        isSendCode,
        setIsSendCode,
        sendCodeCountDown,
        setSendCodeCountDown
    } = useRegisterStore();

    const timerRef = useRef<ReturnType<typeof setTimeout> | null>(null);

    const { t: r } = useTranslation('register');
    const form = useForm({
        defaultValues: {
            ...register,
            email: '',
            validationCode: '',
        } as RegisterForm,
        onSubmit: ({ value }) => {
            console.log('Form submitted with values:', value);
            setRegister({ ...value });
        },
    });

    const email = useField({
        form,
        name: 'email'
    })

    const { data: isBind, isLoading: isBindLoading, refetch: bindRefetch } = useCheckEmailIsBind(email.state.value ?? '')
    useEffect(() => {
        if (isSendCode) {
            handleSendCode();
        } else {
            if (timerRef.current) {
                clearInterval(timerRef.current);
                timerRef.current = null;
                setIsSendCode(false)
            }
        }
    }, [])



    const handleSendCode = async () => {
        setIsSendCode(true);

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
                        if (value === email.state.value) {
                            await bindRefetch();
                        }

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
                                loading={isBindLoading}
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
                                    onClick={handleSendCode}
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
        </form>
    );
};
