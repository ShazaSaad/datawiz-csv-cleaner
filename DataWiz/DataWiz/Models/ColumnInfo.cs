using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataWiz.Models;

public enum ColumnDataType
{
    String,
    Numeric,
    Boolean,
    DateTime
}

public class ColumnInfo
{
    public string Name { get; set; } = string.Empty;
    public ColumnDataType DataType { get; set; }

    public int MissingCount { get; set; }
    public int UniqueCount { get; set; }

    // Only meaningful when DataType == Numeric
    public double? Min { get; set; }
    public double? Max { get; set; }
    public double? Mean { get; set; }
    public double? Median { get; set; }

    public double MissingPercentage(int totalRows) =>
        totalRows == 0 ? 0 : (double)MissingCount / totalRows * 100;
}
