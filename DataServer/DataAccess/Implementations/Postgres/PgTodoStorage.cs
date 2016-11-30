using System;
using System.Collections.Generic;

using Npgsql;

using DataObjects;
using DataServer.DataAccess.Interfaces;
using DataServer.DataAccess.Implementations.Postgres.SQL;



namespace DataServer.DataAccess.Implementations.Postgres
{
    public class PgToDoStorage : PgDataServiceBase, ITodoStorage
    {
        #region Declarations
        #endregion Declarations

        public PgToDoStorage()
        {

        }

        #region ITodoStorage implementation
        public List<TodoItem> GetTodoList()
        {
            return RunReturningQuery(GetTodoList);
        }

        public void AddTodoItem(
            Guid key,
            string description,
            bool isComplete,
            DateTime timeCreated)
        {
            RunVoidQuery(AddTodoItem, key, description, isComplete, timeCreated);
        }
        #endregion ITodoStorage implementation


        #region Query Runners
        protected static List<TodoItem> GetTodoList(NpgsqlConnection conn)
        {
            var retVal = new List<TodoItem>();

            using (var reader = TodoStorageCommands.GetAllTodoItems(conn).ExecuteReader())
            {
                while(reader.Read())
                {
                    var item = new TodoItem
                    {
                        Key = reader.GetGuid(reader.GetOrdinal(PgEnums.Tables.TodoItems.key_uid.ToString())),
                        Description = reader.GetString(reader.GetOrdinal(PgEnums.Tables.TodoItems.description.ToString())),
                        IsComplete = reader.GetBoolean(reader.GetOrdinal(PgEnums.Tables.TodoItems.is_complete.ToString())),
                        TimeCreated = reader.GetDateTime(reader.GetOrdinal(PgEnums.Tables.TodoItems.time_created.ToString()))
                    };

                    retVal.Add(item);
                }
            }

            return retVal;
        }

        protected static void AddTodoItem(
            NpgsqlConnection conn, 
            Guid key,
            string description,
            bool isComplete,
            DateTime timeCreated)
        {
            TodoStorageCommands.AddNewItem(conn, key, description, isComplete, timeCreated).ExecuteNonQuery();
        }
        #endregion Query Runners
    }
}
