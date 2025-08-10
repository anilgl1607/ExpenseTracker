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
    public class CategoryConfig: BaseConfig<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);

            if (builder != null)
            {
                builder.ToTable("categories");
                builder.Property(c => c.Id).HasColumnName("id")
                        .UseIdentityColumn();
                builder.Property(c => c.Name).HasColumnName("name");

                builder.Property(c => c.Description).HasColumnName("description");

                builder.Property(c => c.CreatedAt).HasColumnName("createdat")
                    .HasColumnType("datetime");
                builder.Property(c => c.UserId).HasColumnName("userid");
            }
        }
    }
}
