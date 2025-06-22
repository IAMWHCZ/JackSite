/**
 * 邮件发送类型枚举
 * 定义了系统中各种需要发送邮件通知的场景类型
 */
export enum SendEmailType {
  /** 注册用户 */
  registerUser = 1,
  /** 重置密码 */
  resetPassword,

  /** 更改邮箱地址 */
  changeEmail,

  /** 更改手机号码 */
  changePhoneNumber,

  /** 更改密码 */
  changePassword,

  /** 更改个人资料 */
  changeProfile,

  /** 更改系统设置 */
  changeSettings,

  /** 更改用户角色 */
  changeRole,

  /** 更改账户状态 */
  changeStatus,

  /** 更改时区设置 */
  changeTimeZone,

  /** 更改语言设置 */
  changeLanguage,

  /** 更改主题设置 */
  changeTheme,

  /** 更改双因子认证设置 */
  changeTwoFactorAuth,

  /** 更改通知设置 */
  changeNotifications,

  /** 更改短信通知设置 */
  changeSmsNotifications,

  /** 更改邮件通知设置 */
  changeEmailNotifications,
}
