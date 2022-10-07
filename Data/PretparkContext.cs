using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

    public class PretparkContext : IdentityDbContext
    {
        public PretparkContext (DbContextOptions<PretparkContext> options)
            : base(options)
        {
        }
    }
