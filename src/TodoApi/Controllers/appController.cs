using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;


namespace TodoApi.Controllers
{
    /// <summary>
    /// Root of the "app" node of the application
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]

    public class appController
    {
        #region Declarations
        private ITodoRepository _todoRepository;
        #endregion Declarations

        public appController(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }
    }
}
