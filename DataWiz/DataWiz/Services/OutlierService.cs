using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataWiz.Models;

namespace DataWiz.Services;

public class OutlierService : IOutlierService
{
    public void FlagMissing(DatasetModel dataset)
    {
        foreach (var row in dataset.Rows)
        {
            row.IsFlaggedMissing = dataset.Columns.Any(col => row[col.Name] is null);
        }
    }

    public void FlagOutliers(DatasetModel dataset)
    {
        // Reset flags first
        foreach (var row in dataset.Rows)
            row.IsFlaggedOutlier = false;

        foreach (var column in dataset.Columns.Where(c => c.DataType == ColumnDataType.Numeric))
        {
            var numericRows = dataset.Rows
                .Select(r => new { Row = r, Value = ParseOrNull(r[column.Name]) })
                .Where(x => x.Value.HasValue)
                .ToList();

            if (numericRows.Count < 4) continue; // not enough data for meaningful quartiles

            var sortedValues = numericRows.Select(x => x.Value!.Value).OrderBy(v => v).ToList();
            var q1 = Percentile(sortedValues, 0.25);
            var q3 = Percentile(sortedValues, 0.75);
            var iqr = q3 - q1;

            var lowerBound = q1 - 1.5 * iqr;
            var upperBound = q3 + 1.5 * iqr;

            foreach (var entry in numericRows)
            {
                if (entry.Value < lowerBound || entry.Value > upperBound)
                    entry.Row.IsFlaggedOutlier = true;
            }
        }
    }

    private static double? ParseOrNull(string? value) =>
        double.TryParse(value, out var n) ? n : null;

    private static double Percentile(List<double> sortedValues, double percentile)
    {
        double index = percentile * (sortedValues.Count - 1);
        int lower = (int)Math.Floor(index);
        int upper = (int)Math.Ceiling(index);

        if (lower == upper) return sortedValues[lower];

        double weight = index - lower;
        return sortedValues[lower] * (1 - weight) + sortedValues[upper] * weight;
    }
}