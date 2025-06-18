export interface RegisterForm {
  account: string;
  password: string;
  confirmPassword: string;
  email?: string;
  validationCode?: string;
}
