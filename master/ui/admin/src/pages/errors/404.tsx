import { Button, Container, Typography, Box } from "@mui/material";
import { useNavigate } from "@tanstack/react-router";
import { useTranslation } from "react-i18next";
import { motion } from "framer-motion";
import { Home, ArrowLeft } from "lucide-react";

export function NotFoundPage() {
    // 使用try/catch来处理可能的导航错误
    let navigate;
    try {
        navigate = useNavigate();
    } catch (e) {
        // 如果导航失败，提供一个后备函数
        navigate = () => window.location.href = '/';
    }

    // 使用try/catch处理翻译，防止错误
    let t;
    try {
        const result = useTranslation();
        t = result.t;
    } catch (e) {
        // 后备翻译函数
        t = (key: string) => key.split('.').pop() || key;
    }

    return (
        <Container>
            <Box
                sx={{
                    minHeight: '80vh',
                    display: 'flex',
                    flexDirection: 'column',
                    justifyContent: 'center',
                    alignItems: 'center',
                    textAlign: 'center',
                }}
            >
                {/* 动画数字 */}
                <motion.div
                    style={{
                        display: 'flex',
                        gap: '1rem',
                        fontSize: '6rem',
                        fontWeight: 'bold',
                        marginBottom: '2rem',
                    }}
                    initial={{ opacity: 0, y: -20 }}
                    animate={{ opacity: 1, y: 0 }}
                    transition={{ duration: 0.5 }}
                >
                    <span>4</span>
                    <span>0</span>
                    <span>4</span>
                </motion.div>

                {/* 错误信息 */}
                <Typography variant="h4" gutterBottom>
                    {t('error.404.title', '页面未找到')}
                </Typography>
                <Typography
                    variant="body1"
                    color="text.secondary"
                    sx={{ mb: 4 }}
                >
                    {t('error.404.description', '您请求的页面不存在或已被移除')}
                </Typography>

                {/* 操作按钮 */}
                <Box
                    sx={{
                        display: 'flex',
                        gap: 2,
                        justifyContent: 'center',
                        flexWrap: 'wrap',
                    }}
                >
                    <Button
                        variant="contained"
                        startIcon={<Home />}
                        onClick={() => {
                            try {
                                navigate({ to: '/' });
                            } catch (e) {
                                window.location.href = '/';
                            }
                        }}
                    >
                        {t('error.404.backHome', '返回首页')}
                    </Button>
                    <Button
                        variant="outlined"
                        startIcon={<ArrowLeft />}
                        onClick={() => window.history.back()}
                    >
                        {t('error.404.backPrev', '返回上一页')}
                    </Button>
                </Box>
            </Box>
        </Container>
    );
}
