using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Сhemical_Complex
{
    public static class DatabaseManage
    {
        private static string _connectionString = @"Data Source=" + Application.StartupPath + @"\ComplexDB.db";

        // Универсальный метод добавления записи
        public static bool InsertRecord(string tableName, Dictionary<string, object> fields)
        {
            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();

                    var columns = string.Join(", ", fields.Keys);
                    var parameters = string.Join(", ", fields.Keys.Select(k => $"@{k}"));

                    string query = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";

                    using (var cmd = new SqliteCommand(query, connection))
                    {
                        foreach (var field in fields)
                        {
                            cmd.Parameters.AddWithValue($"@{field.Key}", field.Value);
                        }

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления: {ex.Message}");
                return false;
            }
        }

        // Универсальный метод удаления записи
        public static bool DeleteRecord(string tableName, int id)
        {
            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();
                    string query = $"DELETE FROM {tableName} WHERE Id = @id";

                    using (var cmd = new SqliteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}");
                return false;
            }
        }

        // Универсальный метод обновления записи
        public static bool UpdateRecord(string tableName, int id, Dictionary<string, object> fields)
        {
            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();

                    var setClause = string.Join(", ",
                        fields.Keys.Select(k => $"{k} = @{k}"));

                    string query = $"UPDATE {tableName} SET {setClause} WHERE Id = @id";

                    using (var cmd = new SqliteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        foreach (var field in fields)
                        {
                            cmd.Parameters.AddWithValue($"@{field.Key}", field.Value);
                        }

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex.Message}");
                return false;
            }
        }
    }
}
