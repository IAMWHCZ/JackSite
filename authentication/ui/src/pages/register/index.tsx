import { useTheme } from "@/contexts/ThemeContext"
import { useTranslation } from "react-i18next"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { cn } from "@/lib/utils"
import { PasswordSetting } from "./components/PasswordSetting"
import { EmailConfirm } from "./components/EmailConfirm"
import useRegisterStore from "@/stores/register"
import { ChevronLeft } from "lucide-react"
import { Link, useNavigate } from "@tanstack/react-router"
import { Tooltip, TooltipContent, TooltipTrigger } from "@/components/ui/tooltip"

export const RegisterPage = () => {
  const { t } = useTranslation('register')
  const { t: tCommon } = useTranslation('common')
  const {isCompletePassword,setIsCompletePassword,setRegister} = useRegisterStore()
  const {theme} = useTheme()
  const navigate = useNavigate()
  const handleBack = () => {
    if(isCompletePassword){
      setIsCompletePassword(false)
    }else{
      navigate({
        to:'/login'
      })
      setRegister(null!)
    }
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
          <Link to="." onClick={e=>{
            e.preventDefault()
            e.stopPropagation()
            handleBack()
          }}>
          <Tooltip>
            <TooltipTrigger>
              <ChevronLeft />
            </TooltipTrigger>
            <TooltipContent>
              {tCommon('back')}
            </TooltipContent>
          </Tooltip>
          </Link>
          
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
            {t('title')}
          </CardTitle>
          <CardDescription className="text-center text-slate-600 dark:text-gray-400 animate-in slide-in-from-top-4 duration-600 delay-400">
            {t('description')}
          </CardDescription>
        </CardHeader>
        <CardContent>
          {
            !isCompletePassword?<PasswordSetting/>:<EmailConfirm/>
          }
        </CardContent>
      </Card>
    </div>
  )
}
