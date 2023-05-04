using Cadastro__.Models;
using Microsoft.EntityFrameworkCore;

namespace Cadastro__.Data
{
    public class aplicationDatabase :DbContext
    {
        public aplicationDatabase(DbContextOptions<aplicationDatabase>options) : base(options) 
        { }

        public DbSet<clienteModel> Clientes { get; set; }
    }
}
