using ExpTrack.EfCore.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpTrack.EfCore.Repositories.Contracts
{
    public interface IAppContextFactory<T> where T : DbContext
    {
        
    }
}
