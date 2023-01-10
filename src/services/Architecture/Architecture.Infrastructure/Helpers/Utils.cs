using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Architecture.Infrastructure.Helpers
{
    public static class Utils
    {
        public static string GetFormatedPaginatedQuery(string query, string filter, string sort)
        {
            var op = filter == "" ? "" : " where ";
            return $@"WITH Data_CTE AS ({query}), " +
                        $"Count_CTE AS (SELECT COUNT(*) AS TotalRows FROM Data_CTE {op} { filter} ) SELECT* FROM Data_CTE CROSS JOIN Count_CTE " +
                        $" {op} { filter} " +
                        $" ORDER BY {sort}" +
                        $" OFFSET @Offset ROWS " +
                        $"FETCH NEXT @PageSize ROWS ONLY ";
        }
        public static string GetFormatedFilterQuery(string query, string filter, string sort)
        {
            var op = string.IsNullOrEmpty(filter) ? "" : " where ";
            return $@"WITH Data_CTE AS ({query}), " +
                        $"Count_CTE AS (SELECT COUNT(*) AS TotalRows FROM Data_CTE {op} { filter} ) SELECT* FROM Data_CTE CROSS JOIN Count_CTE " +
                        $" {op} { filter} " +
                        $" ORDER BY {sort}";
        }
        public static string GetFormatedPaginatedInvoiceQuery(string query, string filter, string sort)
        {
            var op = filter == "" ? "" : " where ";
            return $@"WITH Data_CTE AS ({query}), " +
                        $"Count_CTE AS (SELECT COUNT(*) AS TotalRows FROM Data_CTE  ) SELECT * FROM Data_CTE a CROSS JOIN Count_CTE " +
                        $" {op} { filter} " +
                        $" ORDER BY {sort}" +
                        $" OFFSET @Offset ROWS " +
                        $"FETCH NEXT @PageSize ROWS ONLY ";
        }
        public static string GetinvoiceFormatedFilterQuery(string query, string filter, string sort)
        {
            var op = string.IsNullOrEmpty(filter) ? "" : " where ";
            return $@"WITH Data_CTE AS ({query}), " +
                        $"Count_CTE AS (SELECT COUNT(*) AS TotalRows FROM Data_CTE  ) SELECT* FROM Data_CTE a CROSS JOIN Count_CTE " +
                        $" {op} { filter} " +
                        $" ORDER BY {sort}";
        }

    }
}