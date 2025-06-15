import { useTranslation } from "react-i18next"
import { useForm } from '@tanstack/react-form'
import { useState } from 'react'
import { Eye, EyeOff, Lock, Mail } from 'lucide-react'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Checkbox } from '@/components/ui/checkbox'
import { cn } from '@/lib/utils'
import { useTheme } from "@/contexts/ThemeContext"

interface PasswordFormData {
  account: string
  password: string
  rememberMe: boolean
}

export const PasswordAccess = () => {
  const { t } = useTranslation('login')
  const [showPassword, setShowPassword] = useState(false)
  const [isLoading, setIsLoading] = useState(false)
  const { theme } = useTheme()
  const form = useForm({
    defaultValues: {
      account: '',
      password: '',
      rememberMe: false
    } as PasswordFormData,
    onSubmit: async ({ value }) => {

      setIsLoading(true)
      try {
        console.log('Form submitted with values:', value)
        // 这里可以调用登录 API
        // await loginWithPassword(value)
        
        // 模拟登录请求
        await new Promise(resolve => setTimeout(resolve, 2000))
        
      } catch (error) {
        console.error('Login failed:', error)
      } finally {
        setIsLoading(false)
      }
    }
  })

  return (
    <form
      onSubmit={(e) => {
        e.preventDefault()
        form.handleSubmit()
      }}
      className="space-y-6"
    >
      {/* 账户输入 */}
      <form.Field
        name="account"
        validators={{
          onChange: ({ value }) => {
            if (!value) return t('auth.account_required')
            if (value.includes('@')) {
              // 邮箱验证
              const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
              if (!emailRegex.test(value)) return t('auth.invalid_email')
            } else {
              // 手机号验证
              const phoneRegex = /^1[3-9]\d{9}$/
              if (!phoneRegex.test(value)) return t('auth.invalid_phone')
            }
            return undefined
          }
        }}
      >
        {(field) => (
          <div className="space-y-2">
            <Label htmlFor="account" className="text-sm font-medium text-slate-700 dark:text-slate-300">
              {t('auth.account')}
            </Label>
            <div className="relative">
              <Mail className="absolute left-3 top-1/2 transform -translate-y-1/2 w-4 h-4 text-slate-400" />
              <Input
                id="account"
                type="text"
                placeholder={t('auth.account_placeholder')}
                value={field.state.value}
                onChange={(e) => field.handleChange(e.target.value)}
                onBlur={field.handleBlur}
                className={cn(
                  "pl-10 h-12 border-slate-200 dark:border-slate-700 focus:border-blue-500 dark:focus:border-blue-400 transition-colors",
                  field.state.meta.errors.length > 0 && "border-red-500 dark:border-red-400"
                )}
                required
              />
            </div>
            {field.state.meta.errors.length > 0 && (
              <p className="text-sm text-red-600 dark:text-red-400">
                {field.state.meta.errors[0]}
              </p>
            )}
          </div>
        )}
      </form.Field>

      {/* 密码输入 */}
      <form.Field
        name="password"
        validators={{
          onChange: ({ value }) => {
            if (!value) return t('auth.password_required')
            if (value.length < 6) return t('auth.password_min_length')
            return undefined
          }
        }}
      >
        {(field) => (
          <div className="space-y-2">
            <Label htmlFor="password" className="text-sm font-medium text-slate-700 dark:text-slate-300">
              {t('auth.password')}
            </Label>
            <div className="relative">
              <Lock className="absolute left-3 top-1/2 transform -translate-y-1/2 w-4 h-4 text-slate-400" />
              <Input
                id="password"
                type={showPassword ? 'text' : 'password'}
                placeholder={t('auth.password_placeholder')}
                value={field.state.value}
                onChange={(e) => field.handleChange(e.target.value)}
                onBlur={field.handleBlur}
                className={cn(
                  "pl-10 pr-10 h-12 border-slate-200 dark:border-slate-700 focus:border-blue-500 dark:focus:border-blue-400 transition-colors",
                  field.state.meta.errors.length > 0 && "border-red-500 dark:border-red-400"
                )}
                required
              />
              <button
                type="button"
                onClick={() => setShowPassword(!showPassword)}
                className="absolute right-3 top-1/2 transform -translate-y-1/2 text-slate-400 hover:text-slate-600 dark:hover:text-slate-300 transition-colors"
              >
                {showPassword ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
              </button>
            </div>
            {field.state.meta.errors.length > 0 && (
              <p className="text-sm text-red-600 dark:text-red-400">
                {field.state.meta.errors[0]}
              </p>
            )}
          </div>
        )}
      </form.Field>

      {/* 记住我选项 */}
      <form.Field name="rememberMe">
        {(field) => (
          <div className="flex items-center justify-between">
            <div className="flex items-center space-x-2">
              <Checkbox
                id="rememberMe"
                checked={field.state.value}
                onCheckedChange={(checked) => field.handleChange(checked as boolean)}
              />
              <Label 
                htmlFor="rememberMe" 
                className="text-sm text-slate-600 dark:text-slate-400 cursor-pointer"
              >
                {t('auth.remember_me')}
              </Label>
            </div>
            <button
              type="button"
              className="text-sm text-blue-600 hover:text-blue-700 dark:text-blue-400 dark:hover:text-blue-300 font-medium transition-colors"
            >
              {t('auth.forgot_password')}
            </button>
          </div>
        )}
      </form.Field>

      {/* 登录按钮 */}
      <form.Subscribe
        selector={(state) => [state.canSubmit, state.isSubmitting]}
      >
        {([canSubmit, isSubmitting]) => (
          <Button
            type="submit"
            disabled={!canSubmit || isSubmitting || isLoading}
            className={cn(
              "w-full h-10 text-white font-medium",
              theme === 'light'?"bg-gradient-to-r bg-black text-white"
              :"bg-gradient-to-r bg-white text-black",
              "hover:from-blue-600 hover:to-purple-700",
              "focus:ring-4 focus:ring-blue-500/25",
              "hover:from-blue-600 hover:to-purple-700",
              "focus:ring-4 focus:ring-blue-500/25",
              "disabled:opacity-50 disabled:cursor-not-allowed",
              "transition-all duration-200 transform hover:scale-[1.02] active:scale-[0.98]"
            )}
          >
            {isSubmitting || isLoading ? (
              <div className="flex items-center space-x-2">
                <div className="w-4 h-4 border-2 border-white/30 border-t-white rounded-full animate-spin" />
                <span>{t('auth.signing_in')}</span>
              </div>
            ) : (
              <span>{t('auth.sign_in')}</span>
            )}
          </Button>
        )}
      </form.Subscribe>
    </form>
  )
}