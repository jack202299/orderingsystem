
using chinacity70sever.BLL;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore;
namespace chinacity70sever.Models
{
    public class china70Context: DbContext
    {
        public china70Context() : base() { }

        public china70Context(DbContextOptions<china70Context> options):base(options)
        {

        }

        public DbSet<tb_loginlog> tb_loginlog { get; set; } 
        public DbSet<tb_discount> tb_Discounts { get; set; }
        public DbSet<tb_position> tb_position { get; set; }
        public DbSet<tb_shopuser> tb_Shopusers { set; get; }  
        public DbSet<tb_producttype> tb_Producttypes { get; set; }
        public DbSet<tb_shoptype> tb_shoptype { get; set; }
        public DbSet<tb_shoppingcart> tb_shoppingcarts { get; set; }
        public DbSet<tb_kinds> kinds { get; set; }
        public DbSet<tb_orderdetail> orderDeTails { get; set;}
        public DbSet<tb_orders> tb_Orders { get; set; }
        public DbSet<tb_product> tb_Products { get; set; }
        public DbSet<tb_ships> tb_ships { get; set; }
        public DbSet<tb_shop> tb_shop { get; set; }

        public DbSet<tb_unit> tb_Units { get; set; }
        public DbSet<tb_users> tb_users { get; set; }

        public DbSet<tb_sendtask> tb_Sendtasks { get; set; }

        public DbSet<tb_clientage>  tb_Clientages { get; set; }  
        public DbSet<tb_remark> tb_Remarks { get; set; }
        public DbSet<tb_syslog> tb_Syslogs { get; set; }

        public DbSet<tb_menu> tb_Menus { get; set; }   
        public DbSet<tb_permission> tb_Permissions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connstring = dbManager.Gethosturl("myconnstring");
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseMySql(connstring, ServerVersion.AutoDetect(connstring));
          //  optionsBuilder.UseLazyLoadingProxies(false);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<tb_clientage>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.shopid);
                entity.Property(e=>e.userid);
            });
            modelBuilder.Entity<tb_loginlog>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.userid);
                entity.Property(e => e.guid);
                entity.Property(e => e.createdate);
                entity.Property(e => e.express);
            });
            modelBuilder.Entity<tb_discount>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.Discount);
                entity.Property(e => e.DiscountName);
            });

            modelBuilder.Entity<tb_position>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.shopid);
                entity.Property(e => e.position);
            });
            modelBuilder.Entity<tb_permission>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.is_active);
                entity.Property(e => e.menuid);
                entity.Property(e => e.userid);

            });

            modelBuilder.Entity<tb_menu>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.partid);
                entity.Property(e => e.menuguid);
                entity.Property(e=>e.menuname);
            });
            modelBuilder.Entity<tb_shopuser>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.shopid);
                entity.Property(e => e.userid);
            });
            modelBuilder.Entity<tb_remark>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.userid);
                entity.Property(e => e.images);
            });
            modelBuilder.Entity<tb_producttype>(entity =>
            {
                entity.HasKey(e=>e.id);
                entity.Property(e => e.protype);
                entity.Property(e => e.kindsid);
            });

            modelBuilder.Entity<tb_shoptype>(entity =>
            {
                entity.HasKey(e=>e.id);
                entity.Property(e => e.typename);
                entity.Property(e=>e.price);
                entity.Property(e=>e.icount);
            });

            modelBuilder.Entity<tb_sendtask>(e =>
            {
                e.HasKey(r=>r.id);
                e.Property(r => r.title);
                e.Property(r=>r.createdate);
                e.Property(r=>r.blfage);
                e.Property(r=>r.body);
                e.Property(r=>r.tomail);
            });
            modelBuilder.Entity<tb_shoppingcart>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.proid);
                entity.Property(e => e.userid);
                entity.Property(e => e.shopid);
                entity.Property(e=>e.num);
               
            });

            modelBuilder.Entity<tb_unit>(e =>
            {
                e.HasKey(e => e.id);
                e.Property(e => e.unitname);
            });
            modelBuilder.Entity<tb_kinds>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.kindsname);
            });

            modelBuilder.Entity<tb_orderdetail>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.orderid);
                entity.Property(d => d.productID);
                entity.Property(e => e.quantity);
                entity.Property(e => e.discount);
                entity.Property(e => e.discountname);
                entity.Property(e=>e.lastprice);

            });
            modelBuilder.Entity<tb_orders>(entity => { 
               entity
                .HasKey(e => e.id);
                entity.Property(e => e.userid);
                entity.Property(e => e.ordername);
                entity.Property(e => e.blfalge);
                entity.Property(e => e.createdate);
                entity.Property(e => e.shipsid);
                entity.Property(e => e.shopid);
                entity.Property(e => e.meney);
                entity.Property(e => e.paystatus);
                entity.Property(e => e.ordertype);
                entity.Property(e=>e.paymethod);
                entity.Property(e=>e.productcount);
                entity.Property(e => e.actuallyreceived);
                               
            });

            modelBuilder.Entity<tb_product>(entity => {
                entity
                 .HasKey(e => e.id);
                entity.Property(e => e.hotsort);
                entity.Property(e => e.productName);
                entity.Property(e => e.createdate);
                entity.Property(e => e.images);
                entity.Property(e => e.isdown);
                entity.Property(e => e.price);
                entity.Property(e => e.shopid);
                entity.Property(e => e.sellcount);
                entity.Property(e => e.unit);
                entity.Property(e => e.createby);
                entity.Property(e => e.productType);
                entity.Property(e => e.alias);
               

            });

            modelBuilder.Entity<tb_ships>(entity => {
                entity
                 .HasKey(e => e.id);
                entity.Property(e => e.shipaddress);
                entity.Property(e => e.shipperson);
                entity.Property(e => e.shipTel);
                entity.Property(e => e.userid);
                entity.Property(e=>e.company);
                entity.Property(e=>e.TaxID);
                entity.Property(e => e.isdefault);                
            });

            modelBuilder.Entity<tb_users>(entity => {
                entity
                 .HasKey(e => e.id);
                entity.Property(e => e.useremail);
                entity.Property(e => e.pwd);
                entity.Property(e => e.name);
                entity.Property(e => e.address);
                entity.Property(e => e.pib);
                entity.Property(e => e.tel);
                entity.Property(e => e.company);
                entity.Property(e => e.mb);
                entity.Property(e => e.Identitytype);
                entity.Property(e => e.city);

            });

            modelBuilder.Entity<tb_shop>(entity => {
                 entity.HasKey(e => e.id);
                entity.Property(e => e.person);
                entity.Property(e => e.shopaddress);
                entity.Property(e => e.kindid);
                entity.Property(e => e.shopname);
                entity.Property(e => e.tel);
                entity.Property(e => e.usdays);
                entity.Property(e=>e.userid);
                entity.Property(e=>e.lastDate);
                entity.Property(e => e.shopimg);
                entity.Property(e => e.shoptypid);
                entity.Property(e => e.bankaccount);
                entity.Property(e => e.blfage);

            });

            modelBuilder.Entity<tb_syslog>(entity => {
                entity.HasKey(e => e.id);
                entity.Property(e => e.createdate);
                entity.Property(e => e.errmsg);
                entity.Property(e => e.fun);
            
            });
        }
    }
}
