using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFlow.Shared.Kernel
{
    public interface ITenantContext
    {
        public int TenantId { get; }
    }
}
