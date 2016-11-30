using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServer.Handlers
{
    public interface IHandler
    {
        string HandleRequest(string message);
    }
}
