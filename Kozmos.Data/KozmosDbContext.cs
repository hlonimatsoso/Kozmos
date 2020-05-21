using Kozmos.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kozmos.Data
{
    public class KozmosDbContext : IdentityDbContext<KozmosUser>
    {
        public KozmosDbContext(DbContextOptions<KozmosDbContext> options)
            : base(options)
        {

        }
    }
}
