import { Button, Container, Typography, Box } from "@mui/material";
import { useNavigate } from "@tanstack/react-router";
import { useTranslation } from "react-i18next";
import { motion } from "framer-motion";
import { Home, RefreshCw } from "lucide-react";

export const ErrorPage = () => {
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
        // 后备翻译函数，添加明确的类型定义
        t = (key: string, defaultValue?: string): string => defaultValue || key;
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
                <motion.div
                    initial={{ opacity: 0, y: -20 }}
                    animate={{ opacity: 1, y: 0 }}
                    transition={{ duration: 0.5 }}
                >
                    <Typography variant="h2" gutterBottom color="error">
                        {t('error.title', '出错了')}
                    </Typography>
                </motion.div>

                <motion.div
                    initial={{ opacity: 0, y: 20 }}
                    animate={{ opacity: 1, y: 0 }}
                    transition={{ duration: 0.5, delay: 0.2 }}
                >
                    <Typography
                        variant="body1"
                        color="text.secondary"
                        sx={{ mb: 4 }}
                    >
                        {t('error.description', '发生了意外错误，请稍后再试。')}
                    </Typography>

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
                            {t('error.backHome', '返回首页')}
                        </Button>
                        <Button
                            variant="outlined"
                            startIcon={<RefreshCw />}
                            onClick={() => window.location.reload()}
                        >
                            {t('error.refresh', '刷新页面')}
                        </Button>
                    </Box>
                </motion.div>
            </Box>
        </Container>
    );
}
