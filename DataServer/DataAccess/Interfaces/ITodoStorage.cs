using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataObjects;

namespace DataServer.DataAccess.Interfaces
{
    public interface ITodoStorage
    {
        List<TodoItem> GetTodoList();

        void AddTodoItem(Guid key, string description, bool isComplete, DateTime timeCreated);
    }
}
