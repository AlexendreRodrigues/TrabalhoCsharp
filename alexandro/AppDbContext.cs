namespace alexandro
{
    using alexandro.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Funcionario> funcionario { get; set; }
        public DbSet<Pagamento> pagamentos { get; set; }
    }
}
