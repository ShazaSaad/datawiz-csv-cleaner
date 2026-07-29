using CsvHelper;
using CsvHelper.Configuration;
using DataWiz.Helpers;
using DataWiz.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataWiz.Services;

public class CsvService : ICsvService
{
    public async Task<DatasetModel> LoadAsync(string filePath)
    {
        return await Task.Run(() =>
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null, // don't throw on ragged rows
                BadDataFound = null       // don't throw on malformed fields
            };

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);

            csv.Read();
            csv.ReadHeader();
            var headers = csv.HeaderRecord ?? Array.Empty<string>();

            var dataset = new DatasetModel
            {
                FileName = Path.GetFileName(filePath)
            };

            // Initialize columns (data type inferred after we've read all rows)
            var rawColumnValues = headers.ToDictionary(h => h, _ => new List<string?>());

            while (csv.Read())
            {
                var row = new DataRow();
                foreach (var header in headers)
                {
                    var value = csv.GetField(header);
                    // Treat empty string as missing
                    var normalized = string.IsNullOrWhiteSpace(value) ? null : value;
                    row[header] = normalized;
                    rawColumnValues[header].Add(normalized);
                }
                dataset.Rows.Add(row);
            }

            // Build ColumnInfo for each header using inferred type + missing count
            foreach (var header in headers)
            {
                var values = rawColumnValues[header];
                var inferredType = CsvTypeInference.InferType(values);

                dataset.Columns.Add(new ColumnInfo
                {
                    Name = header,
                    DataType = inferredType,
                    MissingCount = values.Count(v => v is null),
                    UniqueCount = values.Where(v => v is not null).Distinct().Count()
                });
            }

            return dataset;
        });
    }

    public async Task ExportAsync(DatasetModel dataset, string filePath)
    {
        await Task.Run(() =>
        {
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            foreach (var column in dataset.Columns)
                csv.WriteField(column.Name);
            csv.NextRecord();

            foreach (var row in dataset.Rows)
            {
                foreach (var column in dataset.Columns)
                    csv.WriteField(row[column.Name] ?? string.Empty);
                csv.NextRecord();
            }
        });
    }
}
