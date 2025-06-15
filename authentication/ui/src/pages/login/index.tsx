import { useState } from 'react'
import { useTranslation } from 'react-i18next'
import { Lock, Smartphone } from 'lucide-react'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { PasswordAccess } from './components/PasswordAccess'
import { CodeAccess } from './components/CodeAccess'
import WeChat from '@/assets/icons/wechat'
import QQ from '@/assets/icons/qq'
import { cn } from '@/lib/utils'
import { useTheme } from '@/contexts/ThemeContext'
import { useNavigate } from '@tanstack/react-router'

type LoginType = 'password' | 'sms'

export const LoginPage = () => {
  const { t } = useTranslation('login')
  const [loginType, setLoginType] = useState<LoginType>('password')
  const {theme} = useTheme()
  const navigate = useNavigate()
  const handleRegister = () => {
    navigate({to: '/register'})
  }
  return (
    <div className="min-h-[95vh] flex items-center justify-center bg-gradient-to-br from-slate-50 to-slate-100 dark:from-gray-900 dark:via-black dark:to-gray-800 p-4">
      {/* 背景装饰 - 支持主题切换 */}
      <div className="absolute inset-0 overflow-hidden">
        <div className="absolute -top-40 -right-40 w-80 h-80 bg-blue-400/20 dark:bg-white/5 rounded-full blur-3xl animate-pulse" />
        <div className="absolute -bottom-40 -left-40 w-80 h-80 bg-purple-400/20 dark:bg-gray-300/5 rounded-full blur-3xl animate-pulse delay-1000" />
        <div className="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 w-96 h-96 bg-gradient-to-r from-blue-400/10 to-purple-400/10 dark:from-white/3 dark:to-gray-400/3 rounded-full blur-3xl opacity-50" />
      </div>

      <Card className="relative w-full max-w-md shadow-2xl border-0 bg-white/80 dark:bg-black/80 backdrop-blur-xl animate-in fade-in-0 zoom-in-95 duration-500 dark:border dark:border-gray-800/50">
        <CardHeader className="space-y-2 pb-6">
          <div className="flex justify-center mb-4">
              <img src="/logo.png" alt="Logo" className="w-16 h-16" />
          </div>
          <CardTitle className={
            cn(
              "text-2xl font-bold text-center",
              theme === 'light'?"bg-gradient-to-r bg-black text-white dark:from-white"
              :"g-gradient-to-r bg-white text-black dark:from-white",
              " dark:to-gray-300 bg-clip-text text-transparent animate-in slide-in-from-top-4 duration-600 delay-300"
              )
          }>
            {t('auth.welcome_back')}
          </CardTitle>
          <CardDescription className="text-center text-slate-600 dark:text-gray-400 animate-in slide-in-from-top-4 duration-600 delay-400">
            {t('auth.login_description')}
          </CardDescription>
        </CardHeader>

        <CardContent>
          <Tabs value={loginType} onValueChange={(value) => setLoginType(value as LoginType)} className="w-full">
            {/* 主题适配的 TabsList */}
            <TabsList className="grid w-full grid-cols-2 mb-6 bg-slate-100/50 dark:bg-gray-900/80 border-0 dark:border dark:border-gray-800 backdrop-blur-sm animate-in slide-in-from-top-4 duration-600 delay-500">
              <TabsTrigger 
                value="password" 
                className={cn(
                  "flex items-center gap-2 transition-all duration-300 ease-in-out",
                  "data-[state=active]:text-blue-600 dark:data-[state=active]:text-white",
                  "text-slate-600 dark:text-gray-400",
                  "hover:scale-[1.01] hover:shadow-md hover:text-slate-800 dark:hover:text-gray-200"
                )}
              >
                <Lock className={cn(
                  "w-4 h-4 transition-all duration-300",
                  loginType === 'password' 
                    ? "text-blue-600 dark:text-white scale-110" 
                    : "text-slate-600 dark:text-gray-400"
                )} />
                {t('auth.password_login')}
              </TabsTrigger>
              <TabsTrigger 
                value="sms" 
                className={cn(
                  "flex items-center gap-2 transition-all duration-300 ease-in-out",
                  "data-[state=active]:text-blue-600 dark:data-[state=active]:text-white",
                  "text-slate-600 dark:text-gray-400",
                  "hover:scale-[1.01] hover:shadow-md hover:text-slate-800 dark:hover:text-gray-200"
                )}
              >
                <Smartphone className={cn(
                  "w-4 h-4 transition-all duration-300",
                  loginType === 'sms' 
                    ? "text-blue-600 dark:text-white scale-110" 
                    : "text-slate-600 dark:text-gray-400"
                )} />
                {t('auth.sms_login')}
              </TabsTrigger>
            </TabsList>

            {/* 左右滑动动画 */}
            <div className="relative overflow-hidden min-h-[300px]">
              <div 
                className={cn(
                  "flex transition-transform duration-500 ease-in-out w-[200%]",
                  loginType === 'password' ? "transform-none" : "-translate-x-1/2"
                )}
              >
                <div className="w-1/2 flex-shrink-0">
                  <TabsContent value="password" className="space-y-6 mt-0">
                    <PasswordAccess />
                  </TabsContent>
                </div>
                <div className="w-1/2 flex-shrink-0">
                  <TabsContent value="sms" className="space-y-6 mt-0">
                    <CodeAccess />
                  </TabsContent>
                </div>
              </div>
            </div>

            {/* 主题适配的分割线 */}
            <div className="relative animate-in fade-in-0 slide-in-from-bottom-4 duration-600 delay-700">
              <div className="absolute inset-0 flex items-center">
                <div className="w-full border-t border-slate-200 dark:border-gray-800" />
              </div>
              <div className="relative flex justify-center text-sm">
                <span className="px-2 bg-white dark:bg-black text-slate-500 dark:text-gray-500">
                  {t('auth.or_continue_with')}
                </span>
              </div>
            </div>

            {/* 主题适配的第三方登录 */}
            <div className="grid grid-cols-2 gap-3 animate-in fade-in-0 slide-in-from-bottom-4 duration-600 delay-800">
              <Button
                type="button"
                variant="outline"
                className={cn(
                  "h-12 border-slate-200 dark:border-gray-800",
                  "bg-slate-50 dark:bg-gray-900/50",
                  "hover:bg-slate-100 dark:hover:bg-gray-800/80",
                  "text-slate-700 dark:text-gray-300",
                  "hover:text-slate-900 dark:hover:text-white",
                  "transition-all duration-300 ease-in-out",
                  "hover:scale-[1.02] hover:shadow-lg",
                  "hover:border-green-300 dark:hover:border-gray-600",
                  "active:scale-[0.98]"
                )}
              >
                <WeChat className="transition-transform duration-300 hover:scale-110" />
                <span className="ml-2">WeChat</span>
              </Button>
              <Button
                type="button"
                variant="outline"
                className={cn(
                  "h-12 border-slate-200 dark:border-gray-800",
                  "bg-slate-50 dark:bg-gray-900/50",
                  "hover:bg-slate-100 dark:hover:bg-gray-800/80",
                  "text-slate-700 dark:text-gray-300",
                  "hover:text-slate-900 dark:hover:text-white",
                  "transition-all duration-300 ease-in-out",
                  "hover:scale-[1.02] hover:shadow-lg",
                  "hover:border-blue-300 dark:hover:border-gray-600",
                  "active:scale-[0.98]"
                )}
              >
                <QQ className="transition-transform duration-300 hover:scale-110" />
                <span className="ml-2">QQ</span>
              </Button>
            </div>

            {/* 主题适配的注册链接 */}
            <div className="mt-6 text-center animate-in fade-in-0 slide-in-from-bottom-4 duration-600 delay-1000">
              <span className="text-sm text-slate-600 dark:text-gray-500">
                {t('auth.no_account')}
              </span>
              <button onClick={handleRegister} className="ml-1 text-sm text-blue-600 hover:text-blue-700 dark:text-white dark:hover:text-gray-300 font-medium transition-all duration-300 hover:scale-105 active:scale-95 underline underline-offset-4 hover:underline-offset-2">
                {t('auth.sign_up')}
              </button>
            </div>
          </Tabs>
        </CardContent>
      </Card>
    </div>
  )
}