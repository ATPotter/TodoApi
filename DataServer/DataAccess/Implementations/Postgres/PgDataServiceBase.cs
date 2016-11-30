using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Npgsql;

namespace DataServer.DataAccess.Implementations.Postgres
{
    public class PgDataServiceBase
    {
        #region Declarations
        private const int DefaultNumDbRetries = 5;

        /// <summary>
        /// Lifetime of this service.
        /// Default to expired to stop people being lazy in future.
        /// </summary>
        private uint _lifetime;

        #endregion Declarations

        #region Lifetime Management
        /// <summary>
        /// Increment the service lifetime counter.
        /// It is strongly suggested that this precedes one extra transaction at a time.
        /// Do not increment in multiples.
        /// </summary>
        protected void IncreaseLifetime()
        {
            if (_lifetime != uint.MaxValue)
                _lifetime++;
        }


        /// <summary>
        /// Checks that the service lifetime allows a transaction to be run, and 
        /// count the transaction as running
        /// </summary>
        public void DecrementLifetime()
        {
            if (_lifetime == 0) throw new ApplicationException("Service lifetime has expired");
            if (_lifetime != uint.MaxValue) _lifetime--;
        }
        #endregion Lifetime Management

        #region Constructor
        protected PgDataServiceBase(uint lifetime = uint.MaxValue)
        {
            _lifetime = lifetime;
        }
        #endregion Constructor


        #region Transaction Runners

        #region RunVoidQuery
        /// <summary>
        /// Attempts to run the provided void method within a single database transaction
        /// </summary>
        /// <typeparam name="TArg1">The type of the first method argument</typeparam>
        /// <typeparam name="TArg2">The type of the second method argument</typeparam>
        /// <typeparam name="TArg3">The type of the third method argument</typeparam>
        /// <param name="method">The method which handles the transaction. This should be static!</param>
        /// <param name="saveSet"></param>
        /// <param name="arg1">The first method argument</param>
        /// <param name="arg2">The second method argument</param>
        /// <param name="arg3">The third method argument</param>
        /// <param name="numRetries">Number of times to attempt a retry if a serialisation error occurs</param>
        /// <returns>Returns whatever a successful "method" call returns</returns>
        protected void RunVoidQuery<TArg1, TArg2, TArg3>(Action<NpgsqlConnection, TArg1, TArg2, TArg3> method, TArg1 arg1,
            TArg2 arg2, TArg3 arg3, int numRetries = DefaultNumDbRetries)
        {
            DecrementLifetime();
            var attempts = 0;
            while (true)
            {
                try
                {
                    using (var conn = PgDalConnection.GetConn())
                    {
                        using (var transaction = conn.BeginTransaction(IsolationLevel.Serializable /* changed */))
                        {
                            method(transaction.Connection, arg1, arg2, arg3);
                            transaction.Commit();
                            break;
                        }
                    }
                }
                catch (NpgsqlException e)
                {
                    if (!HandleNpgsqlException(e, ref attempts, numRetries)) throw;
                }
            }
        }



        /// <summary>
        /// Attempts to run the provided void method within a single database transaction
        /// </summary>
        /// <typeparam name="TArg1">The type of the first method argument</typeparam>
        /// <typeparam name="TArg2">The type of the second method argument</typeparam>
        /// <typeparam name="TArg3">The type of the third method argument</typeparam>
        /// <typeparam name="TArg4">The type of the fourth method argument</typeparam>
        /// <param name="method">The method which handles the transaction. This should be static!</param>
        /// <param name="saveSet"></param>
        /// <param name="arg1">The first method argument</param>
        /// <param name="arg2">The second method argument</param>
        /// <param name="arg3">The third method argument</param>
        /// <param name="arg4">The fourth method argument</param>
        /// <param name="numRetries">Number of times to attempt a retry if a serialisation error occurs</param>
        /// <returns>Returns whatever a successful "method" call returns</returns>
        protected void RunVoidQuery<TArg1, TArg2, TArg3, TArg4>(
            Action<NpgsqlConnection, TArg1, TArg2, TArg3, TArg4> method, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4,
            int numRetries = DefaultNumDbRetries)
        {
            DecrementLifetime();
            var attempts = 0;
            while (true)
            {
                try
                {
                    using (var conn = PgDalConnection.GetConn())
                    {
                        using (var transaction = conn.BeginTransaction(IsolationLevel.Serializable /* changed */))
                        {
                            method(transaction.Connection, arg1, arg2, arg3, arg4);
                            transaction.Commit();
                            break;
                        }
                    }
                }
                catch (NpgsqlException e)
                {
                    if (!HandleNpgsqlException(e, ref attempts, numRetries)) throw;
                }
            }
        }
        #endregion RunVoidQuery

        #region RunReturningQuery
        /// <summary>
        /// Attempts to run the provided method within a single database transaction
        /// </summary>
        /// <typeparam name="T">The return type of the provided method</typeparam>
        /// <param name="method">The method which handles the transaction. This should be static!</param>
        /// <param name="saveSet"></param>
        /// <param name="numRetries">Number of times to attempt a retry if a serialisation error occurs</param>
        /// <returns>Returns whatever a successful "method" call returns</returns>
        protected T RunReturningQuery<T>(Func<NpgsqlConnection, T> method, int numRetries = DefaultNumDbRetries)
        {
            DecrementLifetime();

            var attempts = 0;
            while (true)
            {
                try
                {
                    T ret;
                    using (var conn = PgDalConnection.GetConn())
                    {
                        using (var transaction = conn.BeginTransaction(IsolationLevel.Serializable /* changed */))
                        {
                            ret = method(transaction.Connection);
                            transaction.Commit();
                        }
                    }
                    return ret;
                }
                catch (NpgsqlException e)
                {
                    if (!HandleNpgsqlException(e, ref attempts, numRetries)) throw;
                }
            }
        }
        #endregion RunReturningQuery
        #endregion Transaction Runners


        #region Exception Handlers
        protected bool HandleNpgsqlException(NpgsqlException e, ref int attemptNumber, int maxAttempts)
        {
            // Don't try to handle anything other than interference errors

            //if (!e.ErrorCode.Contains("40001") && !e.Code.Contains("40P01")) return false;

            //var message = string.Format(CultureInfo.InvariantCulture,
            //    "Database Condition: {0}\r\nAttempt No: {1} of {2}\r\nStack Trace: {3}",
            //    e.Code,
            //    attemptNumber + 1, maxAttempts,
            //    new StackTrace());

            //if (attemptNumber == maxAttempts)
            //{
            //    Logger.Error(message);
            //    throw new ApplicationException(
            //        string.Format("Failed to commit transaction after {0} attempts\r\nStack Trace: {1}",
            //                      maxAttempts,
            //                      new StackTrace()), e);
            //}

            //// These are happening all the time - trace only
            //Logger.Trace(message);

            //// Random backoff here
            //var baseDelay = 50 * attemptNumber;
            //var randomDelay = Rnd.Next(20, 100);
            //Thread.Sleep(baseDelay + randomDelay);

            //// And try again
            //++attemptNumber;

            // Mark the error as handled
            return true;
        }
        #endregion Exception Handlers

    }
}
