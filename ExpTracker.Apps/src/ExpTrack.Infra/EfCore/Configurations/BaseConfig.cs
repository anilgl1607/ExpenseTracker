using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpTrack.EfCore.Configurations
{
    public class BaseConfig<T> : IEntityTypeConfiguration<T> where T : class
    {
        protected BaseConfig()
        {
            // This constructor can be used for any common initialization if needed
        }
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            
        }
    }
}
