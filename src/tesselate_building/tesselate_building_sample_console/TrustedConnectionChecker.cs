using Npgsql;

namespace tesselate_building_sample_console
{
    public static class TrustedConnectionChecker
    {
        public static bool HasTrustedConnection(string connectionString)
        {
            var conn = new NpgsqlConnection(connectionString);
            try
            {
                conn.Open();
            }
            catch (PostgresException ex)
            {
                if (ex.SqlState == "28P01")
                {
                    return false;
                }
            }
            catch (NpgsqlException)
            {
                return false;
            }

            conn.Close();
            return true;
        }
    }
}
