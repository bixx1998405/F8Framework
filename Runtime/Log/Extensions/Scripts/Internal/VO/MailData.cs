using System;

namespace F8Framework.Core
{
    [Serializable]
    public class MailData
    {
        // 不清楚参数含义可上网学习：Unity如何发送Email邮件
        public string to = "";
        public string userName = "";
        public string userPassword = "";
        public string smtpHost = "smtp.gmail.com";
        public int smtpPort = 587;
        public string[] cc = null;
    }
}