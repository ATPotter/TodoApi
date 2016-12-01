using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Controllers
{
    public partial class TodoController
    {
        [HttpGet("measurements", Name = "GetMeasurementList")]
        public IActionResult GetMeasurementList()
        {

            return new NoContentResult();
        }
    }
}
