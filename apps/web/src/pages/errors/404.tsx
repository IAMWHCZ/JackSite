import { Link } from "@tanstack/react-router";
import { Button } from "@/components/ui/button";
import { motion } from "framer-motion";

export function NotFoundPage() {
    return (
        <div className="flex flex-col items-center justify-center min-h-[80vh] text-center px-4">
            {/* 动画数字 */}
            <motion.div
                className="flex gap-4 text-8xl font-bold text-primary mb-8"
                initial={{ opacity: 0, y: -20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.5 }}
            >
                <motion.span
                    animate={{
                        rotate: [0, -10, 10, -10, 0],
                    }}
                    transition={{
                        duration: 1,
                        repeat: Infinity,
                        repeatDelay: 3,
                    }}
                >
                    4
                </motion.span>
                <motion.span
                    animate={{
                        rotate: [0, 10, -10, 10, 0],
                    }}
                    transition={{
                        duration: 1,
                        repeat: Infinity,
                        repeatDelay: 3,
                    }}
                >
                    0
                </motion.span>
                <motion.span
                    animate={{
                        rotate: [0, -10, 10, -10, 0],
                    }}
                    transition={{
                        duration: 1,
                        repeat: Infinity,
                        repeatDelay: 3,
                    }}
                >
                    4
                </motion.span>
            </motion.div>

            {/* 错误信息 */}
            <motion.div
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.5, delay: 0.2 }}
            >
                <h1 className="text-2xl font-bold mb-4">
                    糟糕！页面不见了
                </h1>
                <p className="text-muted-foreground mb-8">
                    看起来你访问的页面已经不存在了，或者可能被移动到了其他地方
                </p>

                {/* 操作按钮 */}
                <div className="flex flex-col sm:flex-row gap-4 justify-center">
                    <Button
                        variant="default"
                        asChild
                    >
                        <Link to="/">
                            返回首页
                        </Link>
                    </Button>
                    <Button
                        variant="outline"
                        onClick={() => window.history.back()}
                    >
                        返回上一页
                    </Button>
                </div>
            </motion.div>

            {/* 背景装饰 */}
            <div className="absolute inset-0 -z-10 h-full w-full bg-background">
                <div className="absolute h-full w-full bg-grid-white/10 [mask-image:radial-gradient(ellipse_at_center,transparent_20%,black)]" />
            </div>
        </div>
    );
}