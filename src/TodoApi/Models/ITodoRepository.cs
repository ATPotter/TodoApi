using System;
using System.Collections.Generic;

using DataObjects;

namespace TodoApi.Models
{
    public interface ITodoRepository
    {
        void Add(TodoItem item);
        IEnumerable<TodoItem> GetAll();
        TodoItem Find(Guid key);
        TodoItem Remove(Guid key);
        void Update(TodoItem item);
    }
}
