import { useTheme } from '@/contexts/ThemeContext'
import { useRouter } from '@tanstack/react-router'
import { useEffect, useState } from 'react'

interface ProgressBarProps {
    height?: number
    color?: string
    backgroundColor?: string
    duration?: number
    zIndex?: number
}

export function RouterProgress({
    height = 3,
    color = '#3b82f6',
    backgroundColor = '#e5e7eb',
    duration = 300,
    zIndex = 9999
}: ProgressBarProps = {}) {
    const router = useRouter()
    const [isLoading, setIsLoading] = useState(false)
    const [progress, setProgress] = useState(0)
    const { theme } = useTheme()
    color = theme === 'dark' ? '#3b82f6' : '#ffffff'
    // 监听路由状态
    const isNavigating = router?.state?.isLoading

    useEffect(() => {
        let progressTimer: ReturnType<typeof setTimeout> | undefined
        let startTime: number

        if (isNavigating && !isLoading) {
            // 开始加载
            setIsLoading(true)
            setProgress(0)
            startTime = Date.now()

            const updateProgress = () => {
                const elapsed = Date.now() - startTime
                const baseProgress = Math.min(elapsed / 2000, 0.9)
                const randomFactor = Math.random() * 0.1
                const newProgress = Math.min(baseProgress + randomFactor, 0.95)

                setProgress(newProgress * 100)

                if (newProgress < 0.95 && isNavigating) {
                    progressTimer = setTimeout(updateProgress, 100)
                }
            }

            updateProgress()
        } else if (!isNavigating && isLoading) {
            // 加载完成
            clearTimeout(progressTimer)
            setProgress(100)

            setTimeout(() => {
                setIsLoading(false)
                setProgress(0)
            }, duration)
        }

        return () => {
            clearTimeout(progressTimer)
        }
    }, [isNavigating, isLoading, duration])

    if (!isLoading) return null

    return (
        <div
            style={{
                position: 'fixed',
                top: 0,
                left: 0,
                width: '100%',
                height: `${height}px`,
                backgroundColor: backgroundColor,
                zIndex,
                pointerEvents: 'none',
            }}
        >
            <div
                style={{
                    height: '100%',
                    width: `${progress}%`,
                    backgroundColor: color,
                    transition: 'width 0.2s ease-out',
                    boxShadow: `0 0 10px ${color}40, 0 0 5px ${color}60`,
                }}
            />
        </div>
    )
}