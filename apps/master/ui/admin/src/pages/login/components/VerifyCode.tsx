import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from '@tanstack/react-router';
import {
    Box,
    TextField,
    Button,
    FormControlLabel,
    Checkbox,
    alpha,
    useTheme,
    Typography
} from '@mui/material';
import { LogIn, RefreshCw } from 'lucide-react';
import { toast } from 'react-hot-toast';

export const VerifyCodeLogin = () => {
    const { t } = useTranslation('login');
    const navigate = useNavigate();
    const theme = useTheme();
    const [phone, setPhone] = useState('');
    const [verifyCode, setVerifyCode] = useState('');
    const [rememberMe, setRememberMe] = useState(false);
    const [countdown, setCountdown] = useState(0);
    // 发送验证码
    const handleSendCode = () => {
        if (!phone) {
            toast.error(t('phoneRequired', '请输入手机号'));
            return;
        }

        if (!/^1[3-9]\d{9}$/.test(phone)) {
            toast.error(t('invalidPhone', '手机号格式不正确'));
            return;
        }

        // 模拟发送验证码
        toast.success(t('codeSent', '验证码已发送'));
        setCountdown(60);

        // 倒计时
        const timer = setInterval(() => {
            setCountdown((prev) => {
                if (prev <= 1) {
                    clearInterval(timer);
                    return 0;
                }
                return prev - 1;
            });
        }, 1000);
    };

    // 登录
    const handleLogin = (e: React.FormEvent) => {
        e.preventDefault();

        if (!phone) {
            toast.error(t('usernameRequired', '请输入手机号'));
            return;
        }

        if (!verifyCode) {
            toast.error(t('codeRequired', '请输入验证码'));
            return;
        }

        // 模拟登录成功
        console.log('Login with:', { phone, verifyCode, rememberMe });
        toast.success(t('loginSuccess', '登录成功'));
        navigate({ to: '/' });
    };

    return (
        <form onSubmit={handleLogin}>
            <Box sx={{ mb: 3 }}>
                <TextField
                    label={t('username', '手机号')}
                    fullWidth
                    variant="outlined"
                    value={phone}
                    onChange={(e) => setPhone(e.target.value)}
                    InputProps={{
                        sx: {
                            borderRadius: 2,
                            backgroundColor: alpha(theme.palette.background.paper, 0.6),
                            '&:hover': {
                                backgroundColor: alpha(theme.palette.background.paper, 0.8),
                            },
                            transition: 'background-color 0.3s ease-in-out',
                        }
                    }}
                />
            </Box>

            <Box sx={{ mb: 3, display: 'flex', gap: 1 }}>
                <TextField
                    label={t('verifyCode', '验证码')}
                    fullWidth
                    variant="outlined"
                    value={verifyCode}
                    onChange={(e) => setVerifyCode(e.target.value)}
                    InputProps={{
                        sx: {
                            borderRadius: 2,
                            backgroundColor: alpha(theme.palette.background.paper, 0.6),
                            '&:hover': {
                                backgroundColor: alpha(theme.palette.background.paper, 0.8),
                            },
                            transition: 'background-color 0.3s ease-in-out',
                        }
                    }}
                />
                <Button
                    variant="outlined"
                    disabled={countdown > 0}
                    onClick={handleSendCode}
                    sx={{
                        minWidth: '140px', // 增加最小宽度
                        height: '56px', // 匹配输入框高度
                        borderRadius: 2,
                        borderColor: theme.palette.mode === 'dark' ? '#f5f5f5' : '#333333',
                        color: theme.palette.mode === 'dark' ? '#f5f5f5' : '#333333',
                        fontSize: '0.9rem', // 增加字体大小
                        fontWeight: 500, // 增加字体粗细
                        '&:hover': {
                            borderColor: theme.palette.mode === 'dark' ? '#ffffff' : '#222222',
                            backgroundColor: alpha(theme.palette.mode === 'dark' ? '#ffffff' : '#222222', 0.04),
                        },
                    }}
                    startIcon={countdown === 0 && <RefreshCw size={16} />}
                >
                    {countdown > 0 ? `${countdown}s` : t('sendCode', '发送验证码')}
                </Button>
            </Box>

            {/* 记住我 */}
            <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3, alignItems: 'center' }}>
                <FormControlLabel
                    control={
                        <Checkbox
                            checked={rememberMe}
                            onChange={(e) => setRememberMe(e.target.checked)}
                            sx={{
                                color: '#555',
                                '&.Mui-checked': {
                                    color: '#555',
                                }
                            }}
                        />
                    }
                    label={t('rememberMe', '记住我')}
                />
                <Typography variant="body2" sx={{ color: 'transparent' }}>
                    {/* 占位，保持对齐 */}
                    占位
                </Typography>
            </Box>

            {/* 登录按钮 */}
            <Button
                type="submit"
                variant="contained"
                fullWidth
                size="large"
                startIcon={<LogIn />}
                sx={{
                    mb: 3,
                    py: 1.2,
                    borderRadius: 2,
                    background: theme.palette.mode === 'dark'
                        ? `linear-gradient(90deg, #f5f5f5, #e0e0e0)`
                        : `linear-gradient(90deg, #222222, #333333)`,
                    boxShadow: theme.palette.mode === 'dark'
                        ? `0 4px 14px ${alpha('#fff', 0.2)}`
                        : `0 4px 14px ${alpha('#000', 0.3)}`,
                    color: theme.palette.mode === 'dark' ? '#222222' : '#ffffff',
                    '&:hover': {
                        background: theme.palette.mode === 'dark'
                            ? `linear-gradient(90deg, #ffffff, #f0f0f0)`
                            : `linear-gradient(90deg, #333333, #444444)`,
                        boxShadow: theme.palette.mode === 'dark'
                            ? `0 6px 20px ${alpha('#fff', 0.3)}`
                            : `0 6px 20px ${alpha('#000', 0.4)}`,
                    },
                    transition: 'all 0.3s ease',
                }}
            >
                {t('login', '登录')}
            </Button>
        </form>
    );
};
