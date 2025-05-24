import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react-swc';
import { TanStackRouterVite } from '@tanstack/router-plugin/vite';
import { fileURLToPath } from 'node:url';
import tailwindcss from '@tailwindcss/vite';
import svgr from 'vite-plugin-svgr';

// https://vite.dev/config/
export default defineConfig({
    plugins: [
        TanStackRouterVite({ target: 'react', autoCodeSplitting: true }),
        tailwindcss(),
        react(),
        svgr()
    ],
    resolve: {
        alias: {
            "@": fileURLToPath(new URL("./src", import.meta.url))
        }
    },
    server: {
        host: '0.0.0.0',
        port: 6601
    },
    build: {
        // 启用源码映射以便于调试
        sourcemap: true,
        // 启用 CSS 代码分割
        cssCodeSplit: true,
        // 启用 chunk 大小警告
        chunkSizeWarningLimit: 500,
        // 启用 rollup 选项
        rollupOptions: {
            output: {
                // 将大型依赖项拆分为单独的 chunk
                manualChunks: {
                    'react-vendor': ['react', 'react-dom'],
                    'mui-vendor': ['@mui/material', '@mui/system'],
                    'router-vendor': ['@tanstack/react-router'],
                    'utils-vendor': ['date-fns', 'i18next', 'react-i18next'],
                    // 优化 MUI 分块
                    'mui-core': ['@mui/material/styles', '@mui/material/colors'],
                    'mui-components': ['@mui/material/Button', '@mui/material/TextField']
                }
            }
        },
        // 启用压缩
        minify: 'terser',
        terserOptions: {
            compress: {
                drop_console: true,
                drop_debugger: true
            }
        }
    },
    // 启用依赖项优化
    optimizeDeps: {
        // 预构建这些依赖项
        include: [
            'react',
            'react-dom',
            '@mui/material',
            '@tanstack/react-router',
            'i18next',
            'react-i18next'
        ],
        // 强制预构建这些依赖项
        force: true
    },
    // 启用缓存
    cacheDir: 'node_modules/.vite'
})
