import type { ApiResult } from '@/types/result';
import { ApiService } from './api.service';
import type { SendEmailType } from '@/enums/email';

export class EmailService {
  static async checkEmailBinding(email: string) {
    const response = await ApiService.get<ApiResult<boolean>>(`/email/is-bind`, {
      params: { email: encodeURIComponent(email) },
    });

    return response.data;
  }

  static async sendVerificationCode(email: string, type: SendEmailType) {
    const response = await ApiService.post<ApiResult<void>>(`/email/send-validation-code`, {
      email: email,
      type,
    });
    return response.data;
  }
  static async verifyCode(email: string, code: string, type: SendEmailType) {
    const response = await ApiService.post<ApiResult<void>>(`/email/verify-code`, {
      email: encodeURIComponent(email),
      code,
      type,
    });

    return response.data;
  }
}
