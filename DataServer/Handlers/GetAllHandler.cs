using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using DataObjects;

using DataServer.DataAccess.Interfaces;

namespace DataServer.Handlers
{
    internal class GetAllHandler : IHandler
    {
        #region Declarations
        private ITodoStorage _dataService;
        #endregion Declarations

        public GetAllHandler(ITodoStorage dataService)
        {
            _dataService = dataService;
        }

        public string HandleRequest(string message)
        {
            // At this stage, we return a query from the database
            var list = _dataService.GetTodoList();

            return JsonConvert.SerializeObject(list);

        }
    }
}
