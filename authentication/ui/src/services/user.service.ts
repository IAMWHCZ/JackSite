import { ApiService } from './api.service';
import type { ApiResult } from '@/types/result';

export class UserService {
  static async UsernameExists(username: string) {
    const response = await ApiService.get<ApiResult<boolean>>(`/user-basic/exists/${username}`);
    return response.data;
  }
}
