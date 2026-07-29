using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataWiz.Models;

namespace DataWiz.Helpers;

public static class CsvTypeInference
{
    public static ColumnDataType InferType(List<string?> values)
    {
        var nonNull = values.Where(v => v is not null).ToList();
        if (nonNull.Count == 0) return ColumnDataType.String;

        if (nonNull.All(v => bool.TryParse(v, out _)))
            return ColumnDataType.Boolean;

        if (nonNull.All(v => double.TryParse(v, out _)))
            return ColumnDataType.Numeric;

        if (nonNull.All(v => DateTime.TryParse(v, out _)))
            return ColumnDataType.DateTime;

        return ColumnDataType.String;
    }
}