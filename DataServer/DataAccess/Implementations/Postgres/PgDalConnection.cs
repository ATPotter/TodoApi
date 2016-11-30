using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Npgsql;

namespace DataServer.DataAccess.Implementations.Postgres
{
    internal static class PgDalConnection
    {
        #region Declarations
        private static readonly string _connStr;

        public const string DbAddr = "127.0.0.1";
        public const string DbPort = "5432";
        public const string DbName = "rpcdemo";
        public const bool DbConnectionPooling = true;
        public const int DbMinPoolSize = 1;
        public const int DbMaxPoolSize = 10;
        public const string DbUser = "rpcadmin";
        public const string DbPassword = "rpcadmin";
        #endregion Declarations


        #region Constructor
        static PgDalConnection()
        {
            _connStr = string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};Pooling={5};MinPoolSize={6};MaxPoolSize={7};",
                         DbAddr, DbPort, DbUser, DbPassword,
                         DbName, DbConnectionPooling,
                         DbMinPoolSize, DbMaxPoolSize);
        }
        #endregion Constructor

        #region Public Methods
        public static NpgsqlConnection GetConn()
        {
            var conn = new NpgsqlConnection(_connStr);
            conn.Open();

            // Set the connection timezone to local time.
            // This makes sure all times returned from the server are correct.
            using (
                var query =
                    new NpgsqlCommand(
                        string.Format("SET TIMEZONE = 'UTC-{0}';",
                            TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours), conn))
                query.ExecuteNonQuery();

            return conn;
        }
        #endregion Public Methods
    }
}
