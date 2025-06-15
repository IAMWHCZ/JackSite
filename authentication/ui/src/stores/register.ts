import type { RegisterForm } from '@/pages/register/models/register';
import { create } from 'zustand';
interface RegisterState {
  isCompletePassword: boolean;
  setIsCompletePassword: (value: boolean) => void;
  register:RegisterForm;
  setRegister: (value: RegisterForm) => void;
}
const useRegisterStore = create<RegisterState>((set) => ({
  isCompletePassword: false,
  setIsCompletePassword: (value: boolean) => set({ isCompletePassword: value }),
  register: {
    account: '',
    password: '',
    confirmPassword: '',
    email: '',
    validationCode: ''
  },
  setRegister: (value: RegisterForm) => set({ register: value })
}))

export default useRegisterStore;
