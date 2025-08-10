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
    public class ExpenseConfig : BaseConfig<Expense>
    {
        public override void Configure(EntityTypeBuilder<Expense> builder)
        {
            base.Configure(builder);
            if (builder != null)
            {
                builder.ToTable("Expenses");

                builder.Property(e => e.Id).HasColumnName("Id")
                        .UseIdentityColumn();
                builder.Property(e => e.UserId).HasColumnName("UserId");
                builder.Property(e => e.Amount).HasColumnName("Amount")
                        .HasColumnType("decimal(18,2)");
                builder.Property(e => e.Description).HasColumnName("Description");
                builder.Property(e => e.ExpenseDate).HasColumnName("ExpenseDate")
                        .HasColumnType("datetime");
                builder.Property(e => e.CreatedAt).HasColumnName("CreatedAt")
                    .HasColumnType("datetime");
                builder.Property(e => e.ModifiedAt).HasColumnName("ModifiedAt")
                    .HasColumnType("datetime");
                builder.Property(e => e.CategoryId).HasColumnName("CategoryId");

            }
        }
    }
}
