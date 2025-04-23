/** @type {import('tailwindcss').Config} */
export default {
  // 防止 Tailwind 重置影响 MUI 组件
  corePlugins: {
    preflight: false,
  },
  important: '#root', // 确保 Tailwind 样式优先级
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {},
  },
  plugins: [],
}