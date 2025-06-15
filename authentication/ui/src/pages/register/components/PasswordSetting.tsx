import { useForm } from "@tanstack/react-form"
import type { RegisterForm } from "../models/register"
import { Label } from "@radix-ui/react-label"
import { useTranslation } from "react-i18next"
import { Input } from "@/components/ui/input"
import { cn } from "@/lib/utils"
import {  CheckCheck, Key, User } from "lucide-react"
import { Button } from "@/components/ui/button"
import useRegisterStore from "@/stores/register"
import { UserService } from "@/services/user.service"
import { PasswordInput } from "@/components/PasswordInput"
export const PasswordSetting = () => {
  const { t } = useTranslation('register')
  const { t: tCommon } = useTranslation('common')
const {setIsCompletePassword,register,setRegister} = useRegisterStore()
  const form = useForm({
    defaultValues:{...register} as RegisterForm,
    onSubmit: ({value}) => {
      console.log('Form submitted with values:', value)
      setRegister({...value})
      setIsCompletePassword(true)
    }
  })
  
  return (
    <form onSubmit={e=>{
      e.preventDefault()
      e.stopPropagation()
      form.handleSubmit()
    }}>
      <form.Field name="account"
      validators={{
          onBlurAsync: async ({ value }) => {
            if (!value || value.trim() === '') {
              return t('account_required') || '账号不能为空'
            }
            if (value.length <= 6) {
              return t('account_min_length') || '账号至少需要3个字符'
            }
            if (value.length >= 20) {
              return t('account_max_length') || '账号不能超过20个字符'
            }
            
          const response = await UserService.checkUserExists(value);
          if (!response) {
              return t('account_exists') || '账号已存在，请更换其他账号'
            }
            return undefined
          }
        }}
       children={
        ( field ) =>
          <div className="space-y-2">
            <Label htmlFor="account" className="text-sm font-medium text-slate-700 dark:text-slate-300">
              {t('account')}
            </Label>
            <div className="relative mt-1">
            <User className="absolute left-3 top-1/2 transform -translate-y-1/2 w-4 h-4 text-slate-400"/>
            <Input
            id="account"
            type="text"
            placeholder={t('account_placeholder')}
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
              <p className="text-sm text-red-500 dark:text-red-400 mt-1">
                {field.state.meta.errors[0]}
              </p>
            )}
          </div>
         }/>
      <form.Field name="password" 
      validators={{
        onBlur:({value})=>{
          if (!value || value.trim() === '') {
            return t('password_required') || '密码不能为空'
          }
          if (value.length < 6) {
            return t('password_min_length') || '密码至少需要6个字符'
          }
          if (value.length > 20) {
            return t('password_max_length') || '密码不能超过20个字符'
          }
          if (!/^[a-zA-Z0-9]+$/.test(value)) {
            return t('password_invalid_characters') || '密码只能包含字母和数字'
          }
          if (!/[a-z]/.test(value)) {
            return t('password_lowercase_required') || '密码必须包含至少一个小写字母'
          }
          return undefined          
        }
      }}
      children={(field)=>
        <div className="relative mt-1">
      <Label htmlFor="account" className="text-sm font-medium text-slate-700 dark:text-slate-300">
              {t('password')}
            </Label>    
            <div className="relative mt-1">
              <Key  className="absolute left-3 top-1/2 transform -translate-y-1/2 w-4 h-4 text-slate-400"/>
              <PasswordInput
                id="password"
                placeholder={t('password_placeholder')}
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
              <p className="text-sm text-red-500 dark:text-red-400 mt-1">
                {field.state.meta.errors[0]}
              </p>
            )}
        </div>
      }/>
      <form.Field  name="confirmPassword" 
      validators={{
        onBlur: ({ value }) => {
          const password = form.getFieldValue('password');
          if (!value || value.trim() === '') {
            return t('confirm_password_required') || '确认密码不能为空';
          }
          if (value !== password) {
            return t('passwords_do_not_match') || '两次输入的密码不匹配';
          }
          return undefined;
        }
      }}
      children={(field)=>
        <div className="relative mt-1">
          <Label htmlFor="confirmPassword" className="text-sm font-medium text-slate-700 dark:text-slate-300">
              {t('confirm_password')}
            </Label>
            <div className="relative mt-1">
          <CheckCheck className="absolute left-3 top-1/2 transform -translate-y-1/2 w-4 h-4 text-slate-400"/>
          <PasswordInput
            id="confirmPassword"
            placeholder={t('confirm_password_placeholder')}
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
          {
            field.state.meta.errors.length > 0 && (
              <p className="text-sm text-red-500 dark:text-red-400 mt-1">
                {field.state.meta.errors[0]}
              </p>
            ) 
          }          
        </div>
      } />
    <form.Subscribe 
        selector={(state) => [state.canSubmit, state.isSubmitting]}
        children={([canSubmit, isSubmitting]) => (
          <div className="mt-6 flex justify-center items-center">
            <Button 
              type="submit"
              className="w-full h-12"
              disabled={!canSubmit || isSubmitting}
            >
              {isSubmitting ? tCommon('submitting') || '提交中...' : tCommon('submit')}
            </Button>
          </div>
        )}
      />
    </form>
  )
}
