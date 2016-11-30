using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Npgsql;

using DataServer.DataAccess.Implementations.Postgres.PgEnums;

namespace DataServer.DataAccess.Implementations.Postgres.SQL
{
    internal class TodoStorageCommands
    {
        public static NpgsqlCommand GetAllTodoItems(NpgsqlConnection conn)
        {
            var command = string.Format(CultureInfo.InvariantCulture,
                "SELECT * FROM {0}", PgTables.todo_item);

            return new NpgsqlCommand(command, conn);
        }


        public static NpgsqlCommand AddNewItem(
            NpgsqlConnection conn, 
            Guid key, 
            string description, 
            bool isComplete, 
            DateTime dateTime)
        {
            var command = string.Format(CultureInfo.InvariantCulture,
                "INSERT INTO {0} ({1}, {2}, {3}, {4}) VALUES (@key, @description, @isComplete, @timeCreated)",
                PgTables.todo_item,
                PgEnums.Tables.TodoItems.key_uid,
                PgEnums.Tables.TodoItems.description,
                PgEnums.Tables.TodoItems.is_complete,
                PgEnums.Tables.TodoItems.time_created
                );

            var cmd = new NpgsqlCommand(command, conn);
            cmd.Parameters.AddWithValue("@key", key);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@isComplete", isComplete);
            cmd.Parameters.AddWithValue("@timeCreated", dateTime);

            return cmd;

        }

    }


}
