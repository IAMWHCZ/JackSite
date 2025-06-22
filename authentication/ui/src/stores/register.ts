import type { RegisterForm } from '@/pages/register/models/register';
import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';

interface RegisterState {
  isCompletePassword: boolean;
  setIsCompletePassword: (value: boolean) => void;
  register: RegisterForm;
  setRegister: (value: RegisterForm) => void;
  sendCodeCountDown: number;
  setSendCodeCountDown: (value: number | ((prev: number) => number)) => void;
  isSendCode: boolean;
  setIsSendCode: (value: boolean) => void;
}

const useRegisterStore = create<RegisterState>()(
  persist(
    set => ({
      isCompletePassword: true,
      setIsCompletePassword: (value: boolean) => set({ isCompletePassword: value }),
      register: {
        account: '',
        password: '',
        confirmPassword: '',
        email: '',
        validationCode: '',
      },
      setRegister: (value: RegisterForm) => set({ register: value }),
      sendCodeCountDown: 60,
      setSendCodeCountDown: (value: number | ((prev: number) => number)) =>
        set(state => ({
          sendCodeCountDown: typeof value === 'function' ? value(state.sendCodeCountDown) : value,
        })),
      isSendCode: false,
      setIsSendCode: (value: boolean) => set({ isSendCode: value }),
    }),
    {
      name: 'register-store',
      storage: createJSONStorage(() => localStorage), // 使用 localStorage
      partialize: state => ({
        register: {
          account: state.register.account,
          email: state.register.email,
          // 不缓存密码等敏感信息
        },
        sendCodeCountDown: state.sendCodeCountDown,
        isSendCode: state.isSendCode,
      }),
      // 可选：设置过期时间
      version: 1,
    }
  )
);

export default useRegisterStore;
