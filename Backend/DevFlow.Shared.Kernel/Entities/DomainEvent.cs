using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFlow.Shared.Kernel.Entities
{
    public abstract class DomainEvent
    {
        public int Id { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    }
}
