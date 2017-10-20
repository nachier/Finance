using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finance.Models.Identity
{
    // 可以通过向 ApplicationUser 类添加更多属性来为用户添加配置文件数据。若要了解详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=317594。
    public class ApplicationUser : IdentityUser<int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserCliam>, IUser<int>
    {
        public virtual string ImageUrl { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser,int> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            return userIdentity;
        }
    }

    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class FinanceDB : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserCliam>
    {
        public FinanceDB()
            : base("FinanceDB")
        {

        }

        public static FinanceDB Create()
        {
            return new FinanceDB();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder");
            }

            var user = modelBuilder.Entity<ApplicationUser>()
            .ToTable("Users");
            user.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
            user.HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            user.HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            user.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserNameIndex") { IsUnique = true }));

            user.Property(u => u.Email).HasMaxLength(256);

            modelBuilder.Entity<ApplicationUserRole>()
            .HasKey(r => new { r.UserId, r.RoleId })
            .ToTable("UserRoles");

            modelBuilder.Entity<ApplicationUserLogin>()
            .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
            .ToTable("UserLogins");

            modelBuilder.Entity<ApplicationUserCliam>()
            .ToTable("UserClaims");

            var role = modelBuilder.Entity<ApplicationRole>()
            .ToTable("Roles");
            role.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(256)
            .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("RoleNameIndex") { IsUnique = true }));
            role.HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);
        }
    }

    public class ApplicationUserLogin : IdentityUserLogin<int> { }
    public class ApplicationUserCliam : IdentityUserClaim<int> { }
    public class ApplicationUserRole : IdentityUserRole<int> { }
    public class ApplicationRole : IdentityRole<int, ApplicationUserRole>, IRole<int>
    {      
        public string Description { get; set; }
        public ApplicationRole() { }
        public ApplicationRole(string name): this()
        {
            this.Name = name;
        }
        public ApplicationRole(string name, string description) : this(name) { this.Description = description; }
    }

    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserCliam>, IUserStore<ApplicationUser, int>, IDisposable
    {
        public ApplicationUserStore(DbContext context)
            : base(context)
        { }

        public ApplicationUserStore()

            : this(new IdentityDbContext())
        {

            base.DisposeContext = true;

        }
    }

    public class ApplicationRoleStore : RoleStore<ApplicationRole, int, ApplicationUserRole>, IQueryableRoleStore<ApplicationRole, int>, IRoleStore<ApplicationRole,int>, IDisposable
    {
        public ApplicationRoleStore(DbContext context)
            : base(context)
        {
           
        }

        public ApplicationRoleStore()

            : this(new IdentityDbContext())
        {

            base.DisposeContext = true;

        }
    }
}