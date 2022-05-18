using chinacity70sever.Models;

namespace chinacity70sever.BLL
{
    public class Syslog
    {

        private readonly china70Context context1;
        public Syslog(china70Context context)
        {
          this.context1=context;
        }

        public void writelog(string fun,string exmsg)
        {
            var log = new tb_syslog();
            log.errmsg = exmsg;
            log.fun = fun;
            log.createdate = DateTime.Now.ToString();
            log.id = 0;
            context1.tb_Syslogs.Add(log);
            context1.SaveChanges();
        }
    }
}
