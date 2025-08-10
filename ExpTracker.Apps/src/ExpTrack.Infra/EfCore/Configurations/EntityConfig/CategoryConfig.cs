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
                builder.ToTable("Categories");
                builder.Property(c => c.Id).HasColumnName("Id")
                        .UseIdentityColumn();
                builder.Property(c => c.Name).HasColumnName("Name");

                builder.Property(c => c.Description).HasColumnName("Description");

                builder.Property(c => c.CreatedAt).HasColumnName("CreatedAt")
                    .HasColumnType("datetime");
            }
        }
    }
}
