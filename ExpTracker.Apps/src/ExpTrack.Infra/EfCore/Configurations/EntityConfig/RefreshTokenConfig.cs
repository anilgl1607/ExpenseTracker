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
    public class RefreshTokenConfig: BaseConfig<RefreshToken>
    {
        public override void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            base.Configure(builder);

            if (builder != null)
            {
                builder.Property(e => e.Id).HasColumnName("id")
                    .UseIdentityColumn();
                builder.Property(e => e.UserId).HasColumnName("userid");
                builder.Property(e=>e.Expires).HasColumnName("expires").HasColumnType("datetime");
                builder.Property(e => e.ReplacedByToken).HasColumnName("replacedbytoken");
                builder.Property(e => e.RevokedByIp).HasColumnName("revokedbyip");
                builder.Property(e=>e.Created).HasColumnName("created").HasColumnType("datetime");
                builder.Property(e => e.CreatedByIp).HasColumnName("createdbyip");
                builder.Property(e => e.Token).HasColumnName("token");
            }
        }
    }
}
