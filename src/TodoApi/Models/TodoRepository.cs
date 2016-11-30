using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using DataObjects;

namespace TodoApi.Models
{
    public class TodoRepository : ITodoRepository
    {
        private static ConcurrentDictionary<Guid, TodoItem> _todos =
              new ConcurrentDictionary<Guid, TodoItem>();

        public TodoRepository()
        {
        }

        public IEnumerable<TodoItem> GetAll()
        {
            return _todos.Values;
        }

        public void Add(TodoItem item)
        {
            item.Key = Guid.NewGuid();
            _todos[item.Key] = item;
        }

        public TodoItem Find(Guid key)
        {
            TodoItem item;
            _todos.TryGetValue(key, out item);
            return item;
        }

        public TodoItem Remove(Guid key)
        {
            TodoItem item;
            _todos.TryRemove(key, out item);
            return item;
        }

        public void Update(TodoItem item)
        {
            _todos[item.Key] = item;
        }
    }
}
