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
                builder.ToTable("expenses");

                builder.Property(e => e.Id).HasColumnName("id")
                        .UseIdentityColumn();
                builder.Property(e => e.UserId).HasColumnName("userid");
                builder.Property(e => e.Amount).HasColumnName("amount")
                        .HasColumnType("decimal(18,2)");
                builder.Property(e => e.Description).HasColumnName("description");
                builder.Property(e => e.ExpenseDate).HasColumnName("expensedate")
                        .HasColumnType("datetime");
                builder.Property(e => e.CreatedAt).HasColumnName("createdat")
                    .HasColumnType("datetime");
                builder.Property(e => e.ModifiedAt).HasColumnName("modifiedat")
                    .HasColumnType("datetime");
                builder.Property(e => e.CategoryId).HasColumnName("categoryid");

            }
        }
    }
}
