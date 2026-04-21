using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFlow.Shared.Kernel
{
    public class TenantedEntity
    {
        public int TenantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
