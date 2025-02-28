using AutoMapper.Internal;
using Dapper;
using Microsoft.Data.SqlClient;
using Offices;
using System.Reflection;
using System.Text;

namespace Persistence;

public static class DbInitializer
{
    public static void EnsureCreated(string dbName, string tableName)
    {
        EnsureDatabaseCreated(dbName);
        EnsureTableCreated(tableName);
    }

    private static void EnsureDatabaseCreated(string dbName)
    {
        var connString = Environment.GetEnvironmentVariable("MASTER_DB_CONNECTION");
        using var masterConnection = new SqlConnection(connString);
        var sql = 
            $"""
            IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{dbName}')
                CREATE DATABASE {dbName};
            """;
        masterConnection.Execute(sql);
    }

    private static void EnsureTableCreated(string tableName)
    {
        var sb = new StringBuilder();
        sb.Append($"""
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{tableName}' AND xtype='U')
                CREATE TABLE {tableName} (
        """);

        var properties = typeof(Office).GetProperties(BindingFlags.Instance | BindingFlags.Public);
        foreach (var property in properties)
        {
            var columnName = property.Name;
            var columnType = GetSqlType(property.PropertyType);
            sb.Append($"{columnName} {columnType}, ");
        }

        sb.Length -= 2;
        sb.Append(")");

        var connString = Environment.GetEnvironmentVariable("DB_CONNECTION");
        using var connection = new SqlConnection(connString);
        connection.Execute(sb.ToString());
    }

    private static string GetSqlType(Type propertyType)
    {
        var underlyingType = Nullable.GetUnderlyingType(propertyType);
        var typeToCheck = underlyingType ?? propertyType;

        var sb = new StringBuilder();

        sb.Append(typeToCheck switch
        {
            Type t when t == typeof(Guid) => "UNIQUEIDENTIFIER ",
            Type t when t == typeof(string) => "NVARCHAR(255) ",
            Type t when t == typeof(bool) => "BIT ",
            _ => throw new NotSupportedException($"Type {propertyType.Name} is not supported.")
        });

        if (underlyingType != null)
        {
            sb.Append(" NULL");
        }

        return sb.ToString();
    }
}
