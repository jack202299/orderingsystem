using System.ComponentModel.DataAnnotations.Schema;

namespace chinacity70sever.Models
{
    [Table("tb_users")]
    public class tb_users
    {
        public int id { get; set; }
        public string useremail { get; set; }
        public string pwd { set; get; }

        public string Identitytype  { get; set; }
        public string name { set; get; }

        public string tel { set; get; }

        public string? company { set; get; }

        public string? pib  { set; get; }

        public string? address { set; get; }

        public string? mb { set; get; }

        public string? city { set; get; }
    }
}
