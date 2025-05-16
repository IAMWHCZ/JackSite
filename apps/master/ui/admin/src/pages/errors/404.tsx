import { Button, Container, Typography, Box } from "@mui/material";
import { useNavigate } from "@tanstack/react-router";
import { useTranslation } from "react-i18next";
import { motion } from "framer-motion";
import { Home, ArrowLeft } from "lucide-react";

export function NotFoundPage() {
    const navigate = useNavigate();
    const { t } = useTranslation();

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
                    <Typography variant="h4" gutterBottom>
                        {t('error.404.title')}
                    </Typography>
                    <Typography 
                        variant="body1" 
                        color="text.secondary"
                        sx={{ mb: 4 }}
                    >
                        {t('error.404.description')}
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
                            onClick={() => navigate({ to: '/' })}
                        >
                            {t('error.404.backHome')}
                        </Button>
                        <Button
                            variant="outlined"
                            startIcon={<ArrowLeft />}
                            onClick={() => window.history.back()}
                        >
                            {t('error.404.backPrev')}
                        </Button>
                    </Box>
                </motion.div>
            </Box>
        </Container>
    );
}