import React from 'react'
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from '@tanstack/react-router';
import {
    Box,
    TextField,
    Button,
    FormControlLabel,
    Checkbox,
    Link,
    InputAdornment,
    IconButton,
    alpha,
    useTheme
} from '@mui/material';
import { Eye, EyeOff, LogIn } from 'lucide-react';
import { toast } from 'react-hot-toast';

export const PasswordLoginForm = () => {
    const { t } = useTranslation('login');
    const navigate = useNavigate();
    const theme = useTheme();
    const [showPassword, setShowPassword] = useState(false);
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [rememberMe, setRememberMe] = useState(false);

    const handleLogin = (e: React.FormEvent) => {
        e.preventDefault();
        // 这里添加登录逻辑
        console.log('Login with:', { username, password, rememberMe });
        // 示例: 模拟登录成功
        toast.success(t('loginSuccess', '登录成功'));
        navigate({ to: '/' });
    };

    return (
        <form onSubmit={handleLogin}>
            <Box sx={{ mb: 3 }}>
                <TextField
                    label={t('username')}
                    fullWidth
                    variant="outlined"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
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

            <Box sx={{ mb: 3 }}>
                <TextField
                    label={t('password')}
                    type={showPassword ? 'text' : 'password'}
                    fullWidth
                    variant="outlined"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    InputProps={{
                        endAdornment: (
                            <InputAdornment position="end">
                                <IconButton
                                    onClick={() => setShowPassword(!showPassword)}
                                    edge="end"
                                >
                                    {showPassword ? <EyeOff size={20} /> : <Eye size={20} />}
                                </IconButton>
                            </InputAdornment>
                        ),
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

            {/* 记住我和忘记密码 */}
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
                <Link
                    component="button"
                    type="button"
                    variant="body2"
                    onClick={() => {
                        toast.custom(t('resetPasswordInfo', '请联系管理员重置密码'));
                    }}
                    sx={{
                        textDecoration: 'none',
                        color: '#555',
                        fontWeight: 500,
                        '&:hover': {
                            color: '#333',
                        },
                        transition: 'color 0.3s ease-in-out',
                    }}
                >
                    {t('forgotPassword', '忘记密码?')}
                </Link>
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
