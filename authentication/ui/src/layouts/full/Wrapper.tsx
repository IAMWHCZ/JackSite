import { LanguageSelector } from '@/components/LanguageSelector'
import { ThemeToggle } from '@/components/ThemeToggle'
import { useTheme } from '@/contexts/ThemeContext'
import { Outlet } from '@tanstack/react-router'

export const Wrapper = () => {
  const { theme } = useTheme()

  return (
    <div className={`${theme} w-full min-h-screen flex flex-col`}>
      {/* 主内容区域 */}
      <div className='flex-1 w-full h-[95vh]'>
        <Outlet />
      </div>
      {/* 底部固定区域 */}
      <div className='h-16 w-full flex items-center justify-center gap-4 px-4 bg-slate-100 dark:bg-gray-800 border-t border-slate-200 dark:border-slate-700 z-10'>
        <span className='text-sm text-slate-600 dark:text-slate-400'>
          Powered by JackSite
          <span className='ml-1'>
            @{new Date().getFullYear()}
          </span>
        </span>
        <LanguageSelector/>
        <ThemeToggle/>
      </div>
    </div>
  )
}