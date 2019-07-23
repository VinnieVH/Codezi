using Microsoft.EntityFrameworkCore;

namespace Codezi.Data.EF
{
    public class CodeziContext : DbContext
    {
        public CodeziContext(DbContextOptions options) : base(options)
        {

        }
    }
}
