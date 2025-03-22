using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class ApplicationTenant
    {
        public Guid Id { get; set; }
        public Guid Tenant { get; set; }
    }
}
