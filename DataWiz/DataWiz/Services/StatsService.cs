using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataWiz.Models;

namespace DataWiz.Services;

public class StatsService : IStatsService
{
    public void ComputeStats(DatasetModel dataset)
    {
        foreach (var column in dataset.Columns)
        {
            var rawValues = dataset.Rows.Select(r => r[column.Name]).ToList();
            var nonNullValues = rawValues.Where(v => v is not null).Select(v => v!).ToList();

            column.MissingCount = rawValues.Count(v => v is null);
            column.UniqueCount = nonNullValues.Distinct().Count();

            if (column.DataType == ColumnDataType.Numeric && nonNullValues.Count > 0)
            {
                var numbers = nonNullValues
                    .Select(v => double.TryParse(v, out var n) ? n : (double?)null)
                    .Where(n => n.HasValue)
                    .Select(n => n!.Value)
                    .OrderBy(n => n)
                    .ToList();

                if (numbers.Count > 0)
                {
                    column.Min = numbers.First();
                    column.Max = numbers.Last();
                    column.Mean = numbers.Average();
                    column.Median = ComputeMedian(numbers);
                }
            }
        }
    }

    private static double ComputeMedian(List<double> sortedNumbers)
    {
        int count = sortedNumbers.Count;
        int mid = count / 2;

        return count % 2 == 0
            ? (sortedNumbers[mid - 1] + sortedNumbers[mid]) / 2.0
            : sortedNumbers[mid];
    }
}