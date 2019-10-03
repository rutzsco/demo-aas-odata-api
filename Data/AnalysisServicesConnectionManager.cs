using Microsoft.AnalysisServices.AdomdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Demo.Api.Data
{
    public class ConnectionManager
    {
        private static readonly Queue<AdomdConnection> ConnectionPool;
        private static string ConnectionString;
        private static int QueueCapacity;
        private static bool IsPoolingEnabled = true;

        static ConnectionManager()
        {
            ConnectionPool = new Queue<AdomdConnection>(QueueCapacity);
        }
        public static void Initialize(string connectionString, int queueCapacity = 50, bool isPoolingEnabled = true)
        {
            ConnectionString = connectionString;
            QueueCapacity = queueCapacity;
            IsPoolingEnabled = isPoolingEnabled;
        }

        public static AdomdConnection GetConnection()
        {
            if (!IsPoolingEnabled)
            {
                return CreateConnection();
            }

            lock (ConnectionPool)
            {
                if (ConnectionPool.Count <= 0)
                {
                    return CreateConnection();
                }

                var connection = ConnectionPool.Dequeue();

                if (connection.State == ConnectionState.Open)
                {
                    return connection;
                }
                connection.Dispose();
                connection = CreateConnection();
                return connection;
            }
        }

        private static AdomdConnection CreateConnection()
        {
            var connection = new AdomdConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        public static void ReturnConnection(AdomdConnection connection)
        {
            if (!IsPoolingEnabled)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close(true);
                }

                connection.Dispose();
                return;
            }

            if (connection.State != ConnectionState.Open)
            {
                connection.Dispose();
                return;
            }

            lock (ConnectionPool)
            {
                if (ConnectionPool.Count == QueueCapacity)
                {
                    connection.Close(true);
                    connection.Dispose();
                    return;
                }

                ConnectionPool.Enqueue(connection);
            }
        }

        public static void CloseConnections()
        {
            lock (ConnectionPool)
            {
                foreach (var c in ConnectionPool)
                {
                    if (c.State == ConnectionState.Open)
                    {
                        try
                        {
                            c.Close(true);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }
    }
}
