import type { ApiResult } from '@/types/result';
import { ApiService } from './api.service';
import type { SendEmailType } from '@/enums/email';

export class EmailService {
  static async checkEmailBinding(email: string) {
    try {
      const response = await ApiService.get<ApiResult<boolean>>(`/email/is-bind`, {
        params: { email: email },
      });

      return response.data;
    } catch (error) {
      console.error('Error checking email binding:', error);
      return false;
    }
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
      email: email,
      code,
      type,
    });

    return response.data;
  }
}
