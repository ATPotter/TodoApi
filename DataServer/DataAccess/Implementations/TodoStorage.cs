using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataObjects;
using DataServer.DataAccess.Interfaces;

namespace DataServer.DataAccess.Implementations
{
    public class TodoStorage : ITodoStorage
    {
        public void AddTodoItem(Guid key, string description, bool isComplete, DateTime timeCreated)
        {
            return;
        }

        public List<TodoItem> GetTodoList()
        {
            return null;
        }

        
    }
}
