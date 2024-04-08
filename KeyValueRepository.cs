using Npgsql;

namespace RESTful
{
    public class KeyValueRepository
    {
        private readonly NpgsqlDataSource _dataSource;

        public KeyValueRepository(NpgsqlDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public async Task<KeyValue> GetValueAsync(string key)
        {
            await using var command = _dataSource.CreateCommand("SELECT * FROM keyvalues WHERE key = @key");
            command.Parameters.AddWithValue("key", key);

            await using var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
            {
                return null;
            }
            else
            {
                return new KeyValue
                {
                    Key = reader.GetString(1),
                    Value = reader.GetString(2)
                };
            }
        }

        public async Task<bool> DeleteValueAsync(string key)
        {
            await using var command = _dataSource.CreateCommand("DELETE FROM keyvalues WHERE key = @key");
            command.Parameters.AddWithValue("key", key);

            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }


        public async Task SaveValueAsync(string key, string value)
        {
            await using var command = _dataSource.CreateCommand("INSERT INTO keyvalues (key, value) VALUES (@key, @value)");
            command.Parameters.AddWithValue("key", key);
            command.Parameters.AddWithValue("value", value);

            await command.ExecuteNonQueryAsync();
        }
    }
}
