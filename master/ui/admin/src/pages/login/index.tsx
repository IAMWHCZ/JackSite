import { useState, useEffect } from 'react';
import { useNavigate } from '@tanstack/react-router';
import { useTranslation } from 'react-i18next';
import {
    Box,
    Button,
    Typography,
    Paper,
    Container,
    IconButton,
    Divider,
    Link,
    useTheme,
    useMediaQuery,
    alpha,
    Menu,
    MenuItem,
    Tabs,
    Tab
} from '@mui/material';
import { motion } from 'framer-motion';
import { Languages } from 'lucide-react';
import { PageTransition } from '@/components/animations/PageTransition';
import { useThemeStore } from '@/hooks/useThemeMode';
import { PasswordLoginForm } from './components/Password';
import { VerifyCodeLogin } from './components/VerifyCode';

export const LoginPage = () => {
    const { t, i18n } = useTranslation('login');
    const navigate = useNavigate();
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('sm'));
    const isTablet = useMediaQuery(theme.breakpoints.down('md'));
    const { setTheme } = useThemeStore();
    const [languageMenu, setLanguageMenu] = useState<null | HTMLElement>(null);
    const [loginMethod, setLoginMethod] = useState(0); // 0: 密码登录, 1: 验证码登录
    const [contactDisabled, setContactDisabled] = useState(false);

    // 根据时间自动切换主题
    useEffect(() => {
        const setThemeBasedOnTime = () => {
            const currentHour = new Date().getHours();
            // 晚上8点到早上6点使用暗色主题
            const isDarkTime = currentHour >= 20 || currentHour < 6;
            setTheme(isDarkTime ? 'dark' : 'light');
        };

        // 初始设置
        setThemeBasedOnTime();

        // 每小时检查一次
        const intervalId = setInterval(setThemeBasedOnTime, 60 * 60 * 1000);

        // 清理函数
        return () => clearInterval(intervalId);
    }, [setTheme]);

    // 语言菜单处理
    const handleLanguageMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
        setLanguageMenu(event.currentTarget);
    };

    const handleLanguageMenuClose = () => {
        setLanguageMenu(null);
    };

    const changeLanguage = (lang: string) => {
        i18n.changeLanguage(lang);
        localStorage.setItem('preferred-language', lang);
        handleLanguageMenuClose();
    };

    // 切换登录方式
    const handleLoginMethodChange = (_: React.SyntheticEvent, newValue: number) => {
        setLoginMethod(newValue);
    };

    // 处理联系管理员点击
    const handleContactAdmin = () => {
        if (contactDisabled) return;

        // 禁用按钮
        setContactDisabled(true);

        // 尝试打开邮件客户端
        const emailAddress = '2545481217@outlook.com';
        const subject = encodeURIComponent('请求创建账户');
        const body = encodeURIComponent('您好，\n\n我需要一个新账户，请帮助创建。\n\n谢谢！');

        // 创建mailto链接
        const mailtoLink = `mailto:${emailAddress}?subject=${subject}&body=${body}`;

        // 打开邮件客户端
        window.location.href = mailtoLink;

        // 5秒后重新启用按钮
        setTimeout(() => {
            setContactDisabled(false);
        }, 5000);
    };

    return (
        <PageTransition>
            <Box
                sx={{
                    height: '100vh',
                    width: '100vw',
                    overflow: 'hidden',
                    position: 'relative',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    // 渐变背景
                    background: theme.palette.mode === 'dark'
                        ? `linear-gradient(135deg, ${alpha('#333', 0.2)} 0%, ${alpha(theme.palette.background.default, 0.95)} 100%)`
                        : `linear-gradient(135deg, ${alpha('#e0e0e0', 0.2)} 0%, ${alpha(theme.palette.background.default, 0.9)} 100%)`,
                    transition: 'background 0.5s ease-in-out',
                }}
            >
                {/* 装饰性背景元素 */}
                <Box
                    sx={{
                        position: 'absolute',
                        top: 0,
                        left: 0,
                        right: 0,
                        bottom: 0,
                        overflow: 'hidden',
                        zIndex: 0,
                        opacity: 0.6,
                        pointerEvents: 'none',
                    }}
                >
                    {/* 圆形装饰 */}
                    <Box
                        component={motion.div}
                        initial={{ scale: 0.8, opacity: 0 }}
                        animate={{ scale: 1, opacity: 1 }}
                        transition={{ duration: 1.5, ease: "easeOut" }}
                        sx={{
                            position: 'absolute',
                            width: '50vw',
                            height: '50vw',
                            borderRadius: '50%',
                            background: `radial-gradient(circle, ${alpha('#555', 0.1)} 0%, ${alpha('#555', 0)} 70%)`,
                            top: '-15vw',
                            right: '-15vw',
                            transition: 'background 0.5s ease-in-out',
                        }}
                    />
                    <Box
                        component={motion.div}
                        initial={{ scale: 0.8, opacity: 0 }}
                        animate={{ scale: 1, opacity: 1 }}
                        transition={{ duration: 1.5, delay: 0.2, ease: "easeOut" }}
                        sx={{
                            position: 'absolute',
                            width: '60vw',
                            height: '60vw',
                            borderRadius: '50%',
                            background: `radial-gradient(circle, ${alpha('#777', 0.1)} 0%, ${alpha('#777', 0)} 70%)`,
                            bottom: '-20vw',
                            left: '-20vw',
                            transition: 'background 0.5s ease-in-out',
                        }}
                    />

                    {/* 波浪装饰 */}
                    <Box
                        component={motion.div}
                        initial={{ y: 20, opacity: 0 }}
                        animate={{ y: 0, opacity: 0.3 }}
                        transition={{ duration: 1, ease: "easeOut" }}
                        sx={{
                            position: 'absolute',
                            bottom: '10%',
                            left: 0,
                            right: 0,
                            height: '10vh',
                            background: `url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 1440 320'%3E%3Cpath fill='%23${theme.palette.mode === 'dark' ? '444444' : '999999'}' fill-opacity='0.2' d='M0,192L48,176C96,160,192,128,288,122.7C384,117,480,139,576,165.3C672,192,768,224,864,213.3C960,203,1056,149,1152,138.7C1248,128,1344,160,1392,176L1440,192L1440,320L1392,320C1344,320,1248,320,1152,320C1056,320,960,320,864,320C768,320,672,320,576,320C480,320,384,320,288,320C192,320,96,320,48,320L0,320Z'%3E%3C/path%3E%3C/svg%3E")`,
                            backgroundSize: 'cover',
                            backgroundPosition: 'center',
                            transform: 'scale(1.5)',
                            opacity: 0.3,
                            transition: 'background 0.5s ease-in-out',
                        }}
                    />
                </Box>

                {/* 顶部工具栏 */}
                <Box
                    sx={{
                        position: 'absolute',
                        top: 20,
                        right: 20,
                        display: 'flex',
                        alignItems: 'center',
                        gap: 2,
                        zIndex: 10,
                    }}
                >
                    {/* 语言切换按钮 */}
                    <IconButton
                        onClick={handleLanguageMenuOpen}
                        sx={{
                            backgroundColor: alpha(theme.palette.background.paper, 0.7),
                            backdropFilter: 'blur(4px)',
                            boxShadow: theme.shadows[2],
                            '&:hover': {
                                backgroundColor: alpha(theme.palette.background.paper, 0.9),
                            }
                        }}
                    >
                        <Languages />
                    </IconButton>

                    {/* 语言菜单 */}
                    <Menu
                        anchorEl={languageMenu}
                        open={Boolean(languageMenu)}
                        onClose={handleLanguageMenuClose}
                        PaperProps={{
                            sx: {
                                mt: 1.5,
                                backdropFilter: 'blur(10px)',
                                backgroundColor: alpha(theme.palette.background.paper, 0.9),
                                boxShadow: theme.shadows[4],
                                borderRadius: 2,
                                minWidth: 120,
                            }
                        }}
                        transformOrigin={{ horizontal: 'right', vertical: 'top' }}
                        anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
                    >
                        <MenuItem
                            onClick={() => changeLanguage('zh')}
                            selected={i18n.language === 'zh'}
                            sx={{
                                borderRadius: 1,
                                mx: 0.5,
                                my: 0.3,
                                '&.Mui-selected': {
                                    backgroundColor: alpha('#555', 0.1),
                                    '&:hover': {
                                        backgroundColor: alpha('#555', 0.2),
                                    }
                                }
                            }}
                        >
                            <Typography variant="body2">中文</Typography>
                        </MenuItem>
                        <MenuItem
                            onClick={() => changeLanguage('en')}
                            selected={i18n.language === 'en'}
                            sx={{
                                borderRadius: 1,
                                mx: 0.5,
                                my: 0.3,
                                '&.Mui-selected': {
                                    backgroundColor: alpha('#555', 0.1),
                                    '&:hover': {
                                        backgroundColor: alpha('#555', 0.2),
                                    }
                                }
                            }}
                        >
                            <Typography variant="body2">English</Typography>
                        </MenuItem>
                    </Menu>
                </Box>

                <Container
                    maxWidth="sm"
                    sx={{
                        position: 'relative',
                        zIndex: 1,
                        px: isMobile ? 2 : 3
                    }}
                >
                    <Paper
                        elevation={6}
                        component={motion.div}
                        initial={{ opacity: 0, y: 20 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ duration: 0.5 }}
                        sx={{
                            width: '100%',
                            p: isMobile ? 3 : 4,
                            borderRadius: 3,
                            boxShadow: `0 10px 40px ${alpha('#000', 0.15)}`,
                            backdropFilter: 'blur(10px)',
                            background: alpha(theme.palette.background.paper, theme.palette.mode === 'dark' ? 0.8 : 0.9),
                            border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
                            overflow: 'hidden',
                            position: 'relative',
                            transition: 'all 0.5s ease-in-out',
                        }}
                    >
                        {/* 装饰性卡片元素 */}
                        <Box
                            sx={{
                                position: 'absolute',
                                top: 0,
                                right: 0,
                                width: '150px',
                                height: '150px',
                                background: `linear-gradient(135deg, ${alpha('#555', 0.2)} 0%, ${alpha('#555', 0)} 60%)`,
                                borderRadius: '0 0 0 100%',
                                zIndex: 0,
                                transition: 'background 0.5s ease-in-out',
                            }}
                        />

                        {/* 标题 */}
                        <Box
                            sx={{
                                mb: 4,
                                textAlign: 'center',
                                position: 'relative',
                                zIndex: 1,
                            }}
                        >
                            <motion.div
                                initial={{ opacity: 0, y: -10 }}
                                animate={{ opacity: 1, y: 0 }}
                                transition={{ delay: 0.2, duration: 0.5 }}
                            >
                                <Typography
                                    variant="h4"
                                    component="h1"
                                    fontWeight="bold"
                                    gutterBottom
                                    sx={{
                                        background: `linear-gradient(90deg, #555, #888)`,
                                        WebkitBackgroundClip: 'text',
                                        WebkitTextFillColor: 'transparent',
                                        mb: 1,
                                        transition: 'background 0.5s ease-in-out',
                                    }}
                                >
                                    {theme.palette.mode === 'dark'
                                        ? t('welcomeNight')
                                        : t('welcomeDay')}
                                </Typography>
                            </motion.div>
                            <motion.div
                                initial={{ opacity: 0 }}
                                animate={{ opacity: 1 }}
                                transition={{ delay: 0.3, duration: 0.5 }}
                            >
                                <Typography variant="body2" color="text.secondary">
                                    {t('subtitle')}
                                </Typography>
                            </motion.div>
                        </Box>

                        {/* 登录方式选项卡 */}
                        <Box sx={{ mb: 3 }}>
                            <Tabs
                                value={loginMethod}
                                onChange={handleLoginMethodChange}
                                variant="fullWidth"
                                sx={{
                                    '& .MuiTabs-indicator': {
                                        backgroundColor: theme.palette.mode === 'dark' ? '#f5f5f5' : '#333333',
                                    },
                                    '& .MuiTab-root': {
                                        color: alpha(theme.palette.text.primary, 0.7),
                                        '&.Mui-selected': {
                                            color: theme.palette.mode === 'dark' ? '#f5f5f5' : '#333333',
                                        },
                                    },
                                }}
                            >
                                <Tab label={t('passwordLogin', '密码登录')} />
                                <Tab label={t('verifyCodeLogin', '验证码登录')} />
                            </Tabs>
                        </Box>

                        {/* 登录表单 */}
                        <Box sx={{ mt: 3 }}>
                            {loginMethod === 0 ? (
                                <PasswordLoginForm />
                            ) : (
                                <VerifyCodeLogin />
                            )}
                        </Box>

                        {/* 分割线 */}
                        <Divider
                            sx={{
                                my: 3,
                                '&::before, &::after': {
                                    borderColor: alpha(theme.palette.divider, 0.2),
                                },
                                transition: 'border-color 0.5s ease-in-out',
                            }}
                        >
                            <Typography
                                variant="body2"
                                color="text.secondary"
                                sx={{ px: 1, opacity: 0.7 }}
                            >
                                {t('or', '或')}
                            </Typography>
                        </Divider>

                        {/* 其他登录选项 */}
                        <Box sx={{ textAlign: 'center' }}>
                            <Typography variant="body2" color="text.secondary" gutterBottom>
                                {t('noAccount', '没有账户?')}
                            </Typography>
                            <Link
                                component="button"
                                type="button"
                                variant="body2"
                                onClick={handleContactAdmin}
                                disabled={contactDisabled}
                                sx={{
                                    textDecoration: 'none',
                                    color: contactDisabled ? '#999' : '#555',
                                    fontWeight: 500,
                                    '&:hover': {
                                        color: contactDisabled ? '#999' : '#333',
                                    },
                                    transition: 'color 0.3s ease-in-out',
                                    cursor: contactDisabled ? 'not-allowed' : 'pointer',
                                    opacity: contactDisabled ? 0.7 : 1,
                                }}
                            >
                                {contactDisabled ? t('contactAdminSending', '正在发送...') : t('contactAdmin')}
                            </Link>
                        </Box>

                        {/* 手动切换主题按钮 */}
                        <Box sx={{ mt: 4, textAlign: 'center' }}>
                            <Button
                                variant="text"
                                size="small"
                                onClick={() => setTheme(theme.palette.mode === 'dark' ? 'light' : 'dark')}
                                sx={{
                                    color: alpha(theme.palette.text.secondary, 0.7),
                                    fontSize: '0.75rem',
                                    '&:hover': {
                                        backgroundColor: alpha('#555', 0.1),
                                    }
                                }}
                            >
                                {theme.palette.mode === 'dark'
                                    ? t('switchToLight', '切换到日间模式')
                                    : t('switchToDark', '切换到夜间模式')}
                            </Button>
                        </Box>
                    </Paper>
                </Container>
            </Box>
        </PageTransition>
    );
};
