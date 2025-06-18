import React, { useState } from 'react';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';
import { Eye, EyeOff } from 'lucide-react';
import { cn } from '@/lib/utils';

interface PasswordInputProps extends Omit<React.InputHTMLAttributes<HTMLInputElement>, 'type'> {
  showToggle?: boolean;
  leftIcon?: React.ReactNode;
  error?: boolean;
}

export const PasswordInput = React.forwardRef<HTMLInputElement, PasswordInputProps>(
  ({ className, showToggle = true, leftIcon, error, ...props }, ref) => {
    const [showPassword, setShowPassword] = useState(false);

    return (
      <div className="relative">
        {leftIcon && (
          <div className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 transform text-slate-400">
            {leftIcon}
          </div>
        )}
        <Input
          ref={ref}
          type={showPassword ? 'text' : 'password'}
          className={cn(
            'h-12 border-slate-200 transition-colors focus:border-blue-500 dark:border-slate-700 dark:focus:border-blue-400',
            leftIcon && 'pl-10',
            showToggle && 'pr-10',
            error && 'border-red-500 dark:border-red-400',
            className
          )}
          {...props}
        />
        {showToggle && (
          <Button
            type="button"
            variant="ghost"
            size="sm"
            className="absolute right-0 top-0 h-full px-3 py-2 hover:bg-transparent"
            onClick={() => setShowPassword(!showPassword)}
            tabIndex={-1}
          >
            {showPassword ? (
              <EyeOff className="h-4 w-4 text-slate-400" />
            ) : (
              <Eye className="h-4 w-4 text-slate-400" />
            )}
            <span className="sr-only">{showPassword ? '隐藏密码' : '显示密码'}</span>
          </Button>
        )}
      </div>
    );
  }
);

PasswordInput.displayName = 'PasswordInput';
