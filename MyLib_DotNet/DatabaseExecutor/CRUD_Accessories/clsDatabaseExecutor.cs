using System.Data;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace MyLib_DotNet.DatabaseExecutor.CRUD_Accessories
{
    public partial class clsDatabaseExecutor : absDatabaseHelper
    {
        protected static int? _ExecuteNonQuery(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => _ExecuteWithRetry(() => __ExecuteCommand(query, type, parameters, cmd => cmd.ExecuteNonQuery()), retryAttempts, retryDelayMilliseconds, nameof(_ExecuteNonQuery));

        protected static object? _ExecuteScalar(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => _ExecuteWithRetry(() => __ExecuteCommand(query, type, parameters, cmd => cmd.ExecuteScalar()), retryAttempts, retryDelayMilliseconds, nameof(_ExecuteScalar));

        protected static Dictionary<string, object>? _ExecuteSingleRecord(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        {
            return _ExecuteWithRetry(() =>
            {
                return __ExecuteCommand(query, type, parameters, cmd =>
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                object value = reader.IsDBNull(i) ? DBNull.Value : reader.GetValue(i);
                                row[columnName] = value;
                            }
                            return row;
                        }
                    }
                    return null;
                });
            }, retryAttempts, retryDelayMilliseconds, nameof(_ExecuteSingleRecord));
        }

        protected static T? _ExecuteSingleRecord<T>(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500) where T : class
        {
            return _ExecuteWithRetry(() =>
            {
                return __ExecuteCommand(query, type, parameters, cmd =>
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            object[] row = new object[reader.FieldCount];
                            reader.GetValues(row);
                            var json = JsonConvert.SerializeObject(row);
                            return JsonConvert.DeserializeObject<T>(json);
                        }
                        return null;
                    }
                });
            }, retryAttempts, retryDelayMilliseconds, nameof(_ExecuteSingleRecord));
        }

        protected static List<Dictionary<string, object>>? _ExecuteReader(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        {
            return _ExecuteWithRetry(() =>
            {
                return __ExecuteCommand(query, type, parameters, cmd =>
                {
                    List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                object value = reader.IsDBNull(i) ? DBNull.Value : reader.GetValue(i);
                                row[columnName] = value;
                            }
                            result.Add(row);
                        }
                    }
                    return result;
                });
            }, retryAttempts, retryDelayMilliseconds, nameof(_ExecuteReader));
        }

        protected static List<T>? _ExecuteReader<T>(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500) where T : class
        {
            return _ExecuteWithRetry(() =>
            {
                return __ExecuteCommand(query, type, parameters, cmd =>
                {
                    List<T> results = new List<T>();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new object[reader.FieldCount];
                            reader.GetValues(row);
                            var json = JsonConvert.SerializeObject(row);
                            var item = JsonConvert.DeserializeObject<T>(json);

                            if (item != null)
                            {
                                results.Add(item);
                            }
                        }
                    }
                    return results;
                });
            }, retryAttempts, retryDelayMilliseconds, nameof(_ExecuteReader));
        }

        protected static DataTable? _ExecuteDataAdapter(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        {
            return _ExecuteWithRetry(() =>
            {
                return __ExecuteCommand(query, type, parameters, cmd =>
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                });
            }, retryAttempts, retryDelayMilliseconds, nameof(_ExecuteDataAdapter));
        }

        protected static Dictionary<string, DataTable>? _ExecuteWithRetryTablesByName(string query, IEnumerable<string> tableNames, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        {
            return _ExecuteWithRetry(() =>
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = __PrepareCommand(connection, query, type, parameters, null))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            var result = new Dictionary<string, DataTable>();
                            var nameEnumerator = tableNames.GetEnumerator();
                            bool hasMoreNames = nameEnumerator.MoveNext();

                            do
                            {
                                string currentTableName = hasMoreNames ? nameEnumerator.Current : $"Table{result.Count}";
                                DataTable dataTable = new DataTable(currentTableName);
                                dataTable.Load(reader);
                                result.Add(currentTableName, dataTable);

                                hasMoreNames = nameEnumerator.MoveNext();
                            }
                            while (!reader.IsClosed && reader.NextResult());

                            return result;
                        }
                    }
                }
            }, retryAttempts, retryDelayMilliseconds, nameof(_ExecuteWithRetryTablesByName));
        }

        protected static bool _ExecuteTransaction(List<(string query, SqlParameter[] parameters, CommandType type)> commands, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        {
            return _ExecuteWithRetry(() =>
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            foreach (var (query, parameters, type) in commands)
                            {
                                using (SqlCommand command = __PrepareCommand(connection, query, type, parameters, transaction))
                                {
                                    command.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            _LogEvent($"Transaction failed: {ex.Message}");
                            throw;
                        }
                    }
                }
            }, retryAttempts, retryDelayMilliseconds, nameof(_ExecuteTransaction));
        }

        private static T? __ExecuteCommand<T>(string query, CommandType type, SqlParameter[]? parameters, Func<SqlCommand, T> commandExecutor, SqlConnection? externalConnection = null, SqlTransaction? externalTransaction = null)
        {
            _ValidateQuery(query);
            SqlConnection connection = externalConnection ?? new SqlConnection(_connectionString);
            try
            {
                if (externalConnection == null)
                    connection.Open();
                using (SqlCommand command = __PrepareCommand(connection, query, type, parameters, externalTransaction))
                {
                    return commandExecutor(command);
                }
            }
            finally
            {
                if (externalConnection == null)
                    connection.Dispose();
            }
        }

        private static SqlCommand __PrepareCommand(SqlConnection connection, string query, CommandType type, SqlParameter[]? parameters, SqlTransaction? transaction)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.CommandType = type;
            if (transaction != null)
                command.Transaction = transaction;
            if (parameters != null)
                command.Parameters.AddRange(parameters);
            return command;
        }
    }

    public partial class clsDatabaseExecutor
    {
        protected static async Task<int?> _ExecuteNonQueryAsync(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => await _ExecuteWithRetryAsync(() => __ExecuteCommandAsync(query, type, parameters, async cmd => await cmd.ExecuteNonQueryAsync().ConfigureAwait(false)), retryAttempts, retryDelayMilliseconds, nameof(_ExecuteNonQueryAsync)).ConfigureAwait(false);

        protected static async Task<object?> _ExecuteScalarAsync(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => await _ExecuteWithRetryAsync(() => __ExecuteCommandAsync(query, type, parameters, async cmd => await cmd.ExecuteScalarAsync().ConfigureAwait(false)), retryAttempts, retryDelayMilliseconds, nameof(_ExecuteScalarAsync)).ConfigureAwait(false);

        protected static async Task<Dictionary<string, object>?> _ExecuteSingleRecordAsync(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => await _ExecuteWithRetryAsync(async () => await __ExecuteCommandAsync(query, type, parameters, async cmd => await ReadSingleRecordAsync(cmd)), retryAttempts, retryDelayMilliseconds, nameof(_ExecuteSingleRecordAsync)).ConfigureAwait(false);

        protected static async Task<T?> _ExecuteSingleRecordAsync<T>(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500) where T : class
            => await _ExecuteWithRetryAsync(async () => await __ExecuteCommandAsync(query, type, parameters, async cmd => await ReadSingleRecordAsync<T>(cmd)), retryAttempts, retryDelayMilliseconds, nameof(_ExecuteSingleRecordAsync)).ConfigureAwait(false);

        protected static async Task<List<Dictionary<string, object>>?> _ExecuteReaderAsync(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => await _ExecuteWithRetryAsync(async () => await __ExecuteCommandAsync(query, type, parameters, async cmd => await ReadMultipleRecordsAsync(cmd)), retryAttempts, retryDelayMilliseconds, nameof(_ExecuteReaderAsync)).ConfigureAwait(false);

        protected static async Task<List<T>?> _ExecuteReaderAsync<T>(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500) where T : class
            => await _ExecuteWithRetryAsync(async () => await __ExecuteCommandAsync(query, type, parameters, async cmd => await ReadMultipleRecordsAsync<T>(cmd)), retryAttempts, retryDelayMilliseconds, nameof(_ExecuteReaderAsync)).ConfigureAwait(false);

        protected static async Task<DataTable?> _ExecuteDataAdapterAsync(string query, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => await _ExecuteWithRetryAsync(async () => await __ExecuteCommandAsync(query, type, parameters, async cmd => await FillDataTableAsync(cmd)), retryAttempts, retryDelayMilliseconds, nameof(_ExecuteDataAdapterAsync)).ConfigureAwait(false);

        protected static async Task<Dictionary<string, DataTable>?> _ExecuteWithRetryTablesByNameAsync(string query, IEnumerable<string> tableNames, SqlParameter[]? parameters = null, CommandType type = CommandType.Text, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
        {
            return await _ExecuteWithRetryAsync(async () =>
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);
                    using (SqlCommand command = __PrepareCommand(connection, query, type, parameters, null))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                        {
                            var result = new Dictionary<string, DataTable>();
                            var nameEnumerator = tableNames.GetEnumerator();
                            bool hasMoreNames = nameEnumerator.MoveNext();

                            do
                            {
                                string currentTableName = hasMoreNames ? nameEnumerator.Current : $"Table{result.Count}";
                                DataTable dataTable = new DataTable(currentTableName);
                                dataTable.Load(reader);
                                result.Add(currentTableName, dataTable);

                                hasMoreNames = nameEnumerator.MoveNext();
                            }
                            while (!reader.IsClosed && await reader.NextResultAsync().ConfigureAwait(false));

                            return result;
                        }
                    }
                }
            }, retryAttempts, retryDelayMilliseconds, nameof(_ExecuteWithRetryTablesByNameAsync)).ConfigureAwait(false);
        }

        protected static async Task<bool> _ExecuteTransactionAsync(List<(string query, SqlParameter[] parameters, CommandType type)> commands, byte retryAttempts = 5, ushort retryDelayMilliseconds = 500)
            => await _ExecuteWithRetryAsync(async () => await ExecuteTransactionCommandsAsync(commands), retryAttempts, retryDelayMilliseconds, nameof(_ExecuteTransactionAsync)).ConfigureAwait(false);

        #region Helper Methods
        private static async Task<Dictionary<string, object>?> ReadSingleRecordAsync(SqlCommand cmd)
        {
            using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
            {
                if (await reader.ReadAsync().ConfigureAwait(false))
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        object value = await reader.IsDBNullAsync(i) ? DBNull.Value : reader.GetValue(i);
                        row[columnName] = value;
                    }
                    return row;
                }
            }
            return null;
        }

        private static async Task<T?> ReadSingleRecordAsync<T>(SqlCommand cmd) where T : class
        {
            using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
            {
                if (await reader.ReadAsync().ConfigureAwait(false))
                {
                    var row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    var json = JsonConvert.SerializeObject(row);
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            return null;
        }

        private static async Task<List<Dictionary<string, object>>?> ReadMultipleRecordsAsync(SqlCommand cmd)
        {
            var results = new List<Dictionary<string, object>>();
            using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
            {
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        object value = await reader.IsDBNullAsync(i) ? DBNull.Value : reader.GetValue(i);
                        row[columnName] = value;
                    }
                    results.Add(row);
                }
            }
            return results.Count > 0 ? results : null;
        }

        private static async Task<List<T>?> ReadMultipleRecordsAsync<T>(SqlCommand cmd) where T : class
        {
            var results = new List<T>();
            using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
            {
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    var row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    var json = JsonConvert.SerializeObject(row);
                    var item = JsonConvert.DeserializeObject<T>(json);
                    if (item != null) results.Add(item);
                }
            }
            return results.Count > 0 ? results : null;
        }

        private static async Task<DataTable> FillDataTableAsync(SqlCommand cmd)
        {
            using (var adapter = new SqlDataAdapter(cmd))
            {
                var dataTable = new DataTable();
                await Task.Run(() => adapter.Fill(dataTable)).ConfigureAwait(false);
                return dataTable;
            }
        }

        private static async Task<bool> ExecuteTransactionCommandsAsync(List<(string query, SqlParameter[] parameters, CommandType type)> commands)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var (query, parameters, type) in commands)
                        {
                            using (var command = __PrepareCommand(connection, query, type, parameters, transaction))
                            {
                                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                            }
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private static async Task<T> __ExecuteCommandAsync<T>(string query, CommandType type, SqlParameter[]? parameters, Func<SqlCommand, Task<T>> commandExecutor, SqlConnection? externalConnection = null, SqlTransaction? externalTransaction = null)
        {
            _ValidateQuery(query);

            SqlConnection connection = externalConnection ?? new SqlConnection(_connectionString);

            try
            {
                if (externalConnection == null)
                    await connection.OpenAsync().ConfigureAwait(false);

                using (SqlCommand command = __PrepareCommand(connection, query, type, parameters, externalTransaction))
                {
                    try
                    {
                        return await commandExecutor(command).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        _LogEvent($"Command execution failed for query: {query}. Error: {ex.Message}");
                        throw;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                _LogEvent($"SQL Error in command execution: {sqlEx.Message}");
                throw;
            }
            finally
            {
                // التخلص من الاتصال إذا كان جديداً
                if (externalConnection == null)
                {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                    await connection.DisposeAsync().ConfigureAwait(false);
#else
                        connection.Dispose();
#endif
                }
            }
        }
        #endregion
    }
}
