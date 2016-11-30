using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using DataServer.DataAccess.Interfaces;
using DataObjects.RPCRequests;

namespace DataServer.Handlers
{
    internal class AddItemHandler : IHandler
    {
        #region Declarations
        private ITodoStorage _dataService;
        #endregion Declarations

        public AddItemHandler(ITodoStorage dataService)
        {
            _dataService = dataService;
        }

        public string HandleRequest(string message)
        {
            var rpc = JsonConvert.DeserializeObject<RpcRequest<AddItemDto>>(message);

            // At this stage, we return a query from the database
            if(null != rpc)
            {
                var dto = rpc.Payload;
                _dataService.AddTodoItem(dto.Key, dto.Description, dto.IsComplete, dto.TimeCreated);
            }
            else
            {
                throw new Exception("Cannot convert message to DTO");
            }

            return "";
        }
    }
}
