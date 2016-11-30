using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServer.DataAccess.Implementations.Postgres.PgEnums.Tables
{
    [Flags]
    public enum TodoItems
    {
        key_uid = 1 << 0,
        description = 1 << 1,
        is_complete = 1 << 2,
        time_created = 1 << 3
    }
}
