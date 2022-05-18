using chinacity70sever.Models;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace chinacity70sever.BLL
{
    public class dbManager
    {
          
        public static dynamic SendMail(string sub,string body,string toaddress)
        {
            
            var mssage=new MailMessage(Gethosturl("fromemail"), toaddress);
            mssage.Subject = sub;
            mssage.Body = body;
            mssage.IsBodyHtml = true;
            mssage.BodyEncoding = Encoding.UTF8;
            var smtp = new SmtpClient
            {
                Host = Gethosturl("stmpservice"),
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true
            };
            NetworkCredential networkCredential = new(Gethosturl("fromemail"), Gethosturl("fromialpwd"));
            smtp.Credentials = networkCredential;
            try
            {
                smtp.Send(mssage);
                return new { code = 0, msg = "发送成功" };
            } 
            catch (Exception ex)
            {
                return new { code = 1, msg = ex.Message };
            }

        }

    public static string Gethosturl(string par)
    {
        ConfigurationBuilder configurationBuilder = new();
        configurationBuilder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
        var configuration = configurationBuilder.Build();
        var hosturl = configuration[par];
        return hosturl;
    }

      
        public static string Encrypt(string str)
        {
           return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));

        }
        public  static string Decrypt(string str)
        {
            var stp = Convert.FromBase64String(str);
            return Encoding.UTF8.GetString(stp);
        }

           

    }


}
