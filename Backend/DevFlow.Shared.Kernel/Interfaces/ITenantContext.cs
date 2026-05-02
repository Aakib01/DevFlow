using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFlow.Shared.Kernel.Interfaces
{
    public interface ITenantContext
    {
        public int TenantId { get; set; }
    }
}
