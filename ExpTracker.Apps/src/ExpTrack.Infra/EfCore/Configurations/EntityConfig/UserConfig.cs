using ExpTrack.DbAccess.Entities;
using ExpTrack.EfCore.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpTrack.EfCore.Configurations.EntityConfig
{
    public class UserConfig: BaseConfig<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            if (builder!=null)
            {
                builder.ToTable("users");

                builder.Property(e => e.Id).HasColumnName("id").UseIdentityColumn();
                builder.Property(e => e.Username).HasColumnName("username");
                builder.Property(e => e.Email).HasColumnName("email");
                builder.Property(e=>e.CreatedAt).HasColumnName("createdat")
                    .HasColumnType("datetime");
                builder.Property(e => e.PasswordHash).HasColumnName("passwordhash");
                builder.Property(e => e.PasswordSalt).HasColumnName("passwordsalt");

            }
        }
    }
}
