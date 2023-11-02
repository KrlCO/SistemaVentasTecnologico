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


        //public static string GenPassword()
        //{
        //    string pass = Guid.NewGuid().ToString("U").Substring(0, 12);
        //    return pass;
        //}

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

    }
}
