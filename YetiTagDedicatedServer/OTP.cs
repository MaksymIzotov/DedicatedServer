using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace YetiTagDedicatedServer
{
    class OTP
    {

        private static OTP instance = null;
        private static readonly object padlock = new object();

        OTP()
        {
            
        }

        public static OTP Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new OTP();
                    }
                    return instance;
                }
            }
        }

        public Dictionary<string, int> otpData = new Dictionary<string, int>();

        public bool CheckLogin(string username, int otp)
        {
            foreach (KeyValuePair<string, int> value in otpData)
            {
                if (username == value.Key)
                    if (otp == value.Value)
                        return true;
            }

            return false;
        }

        public void SendOTP(string email, string username)
        {
            try
            {
                int otp = GetOTP();

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("serverloginotp@gmail.com");
                mail.To.Add(email);
                mail.Subject = "OTP";
                mail.Body = "Your One-Time-Password is: " + otp;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("serverloginotp@gmail.com", "test12345otp");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                otpData.Add(username, otp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldn't send an OTP");
            }
        }

        private int GetOTP()
        {
            Random rand = new Random();
            return rand.Next(10000, 99999);
        }
    }
}
