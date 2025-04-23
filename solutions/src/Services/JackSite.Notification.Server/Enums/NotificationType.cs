using System.ComponentModel;

namespace JackSite.Notification.Server.Enums;

public enum NotificationType
{
    
    [Description("邮件")]
    Email = 1,
    [Description("短信")]    
    Sms ,
    [Description("系统")]
    System,
    [Description("微信")]
    Wechat,
    [Description("QQ")]
    QQ
}