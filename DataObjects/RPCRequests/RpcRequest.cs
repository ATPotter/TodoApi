using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.RPCRequests
{
    [Serializable]
    public class RpcRequest<T>
    {
        public RpcRequest(T payload)
        {
            Payload = payload;
            ObjectType = typeof(T);
        }

        public Type ObjectType { get; set; }
        public T Payload { get; set; }
    }
}
