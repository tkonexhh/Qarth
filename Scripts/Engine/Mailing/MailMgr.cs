using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Qarth
{
    public class MailMgr : TSingleton<MailMgr>
    {
        public System.Action sentCallback;

        private bool IsInputInvalid(string input)
        {
            return string.IsNullOrEmpty(input.Trim());
        }

        private bool IsMailConfValid()
        {
            if (IsInputInvalid(MailConfig.mailSender) || IsInputInvalid(MailConfig.mailPWD) || IsInputInvalid(MailConfig.mailReceiver))
            {
                Log.e("MailConfig info missing.");
                return false;
            }
            return true;
        }

        public void SendFeedbackMail(string name, string addr, string content, string attachmentPath = null)
        {
            if (!IsMailConfValid())
                return;

            // build mail
            MailMessage mail = new MailMessage();
            var display = "Feedback from " + Application.productName;
            mail.From = new MailAddress(MailConfig.mailSender, display);
            mail.To.Add(MailConfig.mailReceiver);
            mail.Subject = display;
            mail.Body = "Name: " + name + "\nEmail: " + addr + "\n\nFeedback\n\n" + content;
            if (!string.IsNullOrEmpty(attachmentPath))
            {
                Attachment attachment = new Attachment(attachmentPath);
                mail.Attachments.Add(attachment);
            }

            // smtp
            SmtpClient smtpServer = GetSmtpClient();
            smtpServer.Port = MailConfig.smtpPort;
            smtpServer.UseDefaultCredentials = false;
            smtpServer.Credentials = new NetworkCredential(MailConfig.mailSender, MailConfig.mailPWD) as ICredentialsByHost;
            smtpServer.EnableSsl = MailConfig.enableSSL;

            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

            smtpServer.SendAsync(mail, null);
            smtpServer.SendCompleted += OnMailSendComplete;
        }


        private SmtpClient GetSmtpClient()
        {
            switch (MailConfig.mailClient)
            {
                case MailConfig.MailClientType.GMAIL:
                    return new SmtpClient("smtp.gmail.com");
                case MailConfig.MailClientType.HOTMAIL:
                    return new SmtpClient("smtp.live.com");
                case MailConfig.MailClientType.CUSTOM:
                    return new SmtpClient(MailConfig.customSmtp);
                default:
                    return null;
            }
        }

        private void OnMailSendComplete(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Log.i("Mail sending was cancelled");
            }

            if (e.Error != null)
            {
                Log.e("Mail Error: " + e.Error.ToString());
            }
            else
            {
                Log.i("Mail sent!");
                if (sentCallback != null)
                    sentCallback.Invoke();
            }
        }
    }
}
