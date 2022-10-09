using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class PretparkContext : IdentityDbContext<Gebruiker>
{
    public PretparkContext(DbContextOptions<PretparkContext> options)
        : base(options)
    {
    }

    public DbSet<Attractie> Attracties { get; set; }
    public DbSet<Like> Likes { get; set; }
}
