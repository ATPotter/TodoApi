using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

using DataObjects;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public partial class TodoController : Controller
    {
        #region Declarations
        private ITodoRepository _todoStore;
        #endregion Declarations
        public TodoController(ITodoRepository todoStore)
        {
            _todoStore = todoStore;
        }

        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            return _todoStore.GetAll();
        }

        [HttpGetAttribute("{id}", Name = "GetTodo")]
        public IActionResult GetById(Guid id)
        {
            var item = _todoStore.Find(id);

            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item)
        {
            _todoStore.Add(item);
            //return CreatedAtRoute("GetTodo", new { Key = item.Key }, item);

            return new NoContentResult();

        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] TodoItem item)
        {
            if (item == null || item.Key != id)
            {
                return BadRequest();
            }

            var todo = _todoStore.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            _todoStore.Update(item);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var todo = _todoStore.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            _todoStore.Remove(id);
            return new NoContentResult();
        }


    }
}
