import { ApiService } from "./api.service"

export class UserService{
  /**
   * 检查用户是否存在
   */
  static async checkUserExists(username: string) {
    const response = await ApiService.get<boolean>(`/user-basic/exists/${username}`,{preventCache:false})
    return response
  }
}
