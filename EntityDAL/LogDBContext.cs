using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EntityModel;

namespace EntityDAL
{
    public class LogDBContext : DbContext
    {
         public LogDBContext()
        {

        }

         public DbSet<LogItem> LogItems { get; set; }
    }
}
