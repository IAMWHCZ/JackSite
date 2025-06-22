import { LanguageSelector } from '@/components/LanguageSelector';
import { RouterProgress } from '@/components/RouterProgress';
import { ThemeToggle } from '@/components/ThemeToggle';
import { useTheme } from '@/contexts/ThemeContext';
import { Outlet } from '@tanstack/react-router';

export const Wrapper = () => {
    const { theme } = useTheme();

    return (
        <div className={`${theme} flex min-h-screen w-full flex-col bg-gradient-to-br from-slate-50 to-slate-100 dark:from-gray-950 dark:to-gray-900`}>
            <RouterProgress />
            {/* 顶部导航栏 */}
            <header className="z-20 flex h-16 w-full shrink-0 items-center justify-between border-b border-slate-200/50 bg-white/80 px-4 backdrop-blur-sm sm:px-6 dark:border-slate-700/50 dark:bg-black/80">
                <div className="flex items-center gap-2">
                    <img src="/logo.png" alt="JackSite" className="h-8 w-8" />
                    <span className="text-lg font-semibold text-slate-900 dark:text-white">
                        JackSite
                    </span>
                </div>
                <div className="flex items-center gap-2 sm:gap-3">
                    <LanguageSelector />
                    <ThemeToggle />
                </div>
            </header>

            {/* 主内容区域 */}
            <main className="flex min-h-0 flex-1 w-full">
                <div className="flex flex-1 flex-col overflow-hidden">
                    <Outlet />
                </div>
            </main>

            {/* 底部区域 */}
            <footer className="z-10 flex h-auto min-h-[3.5rem] w-full shrink-0 items-center justify-center border-t border-slate-200/50 bg-white/60 px-4 py-3 backdrop-blur-sm sm:px-6 dark:border-slate-700/50 dark:bg-black/60">
                <div className="flex w-full max-w-6xl flex-col items-center justify-between gap-2 sm:flex-row sm:gap-4">
                    <span className="text-sm text-slate-600 dark:text-slate-400">
                        © {new Date().getFullYear()} JackSite. All rights reserved.
                    </span>
                    <div className="flex flex-wrap items-center justify-center gap-3 text-xs text-slate-500 sm:gap-4 dark:text-slate-500">
                        <a href="#" className="transition-colors hover:text-slate-700 dark:hover:text-slate-300">
                            Privacy Policy
                        </a>
                        <a href="#" className="transition-colors hover:text-slate-700 dark:hover:text-slate-300">
                            Terms of Service
                        </a>
                        <a href="#" className="transition-colors hover:text-slate-700 dark:hover:text-slate-300">
                            Support
                        </a>
                    </div>
                </div>
            </footer>
        </div>
    );
};
