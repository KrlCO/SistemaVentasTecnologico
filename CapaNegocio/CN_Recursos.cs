using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using System.Net.Mail;
using System.Net;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace CapaNegocio
{
    public class CN_Recursos
    {


        public static string GenPassword()
        {
            string pass = Guid.NewGuid().ToString("N").Substring(0, 12);
            return pass;
        }

        //Encriptar Password en SHA256

        public static string ConvertSha256(string text)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 has = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = has.ComputeHash(enc.GetBytes(text));

                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        public static bool SendEmail(string mail, string cc, string msj)
        {
            bool resultado = false;

            try
            {
                MailMessage email = new MailMessage();
                email.To.Add(mail);
                email.From = new MailAddress("julioguidotti.antamina@gmail.com");
                email.Subject = cc;
                email.Body = msj;
                email.IsBodyHtml = true;

                //Configuration Server that Send Email
                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("julioguidotti.antamina@gmail.com", "fzjjvfskvrlspucb"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true
                };
                smtp.Send(email);
                resultado = true;
            }
            catch(Exception)
            {
                resultado = false;
            }
            return resultado;
        }

    }
}
