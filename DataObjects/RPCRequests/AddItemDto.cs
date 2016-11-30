using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.RPCRequests
{
    [Serializable]
    public class AddItemDto
    {
        public Guid Key { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }
        public DateTime TimeCreated { get; set; }
    }
}
