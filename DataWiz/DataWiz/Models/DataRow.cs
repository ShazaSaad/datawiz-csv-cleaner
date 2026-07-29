using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataWiz.Models;

public class DataRow
{
    // Column name -> raw cell value (keep as string; services interpret it)
    public Dictionary<string, string?> Cells { get; set; } = new();

    public bool IsFlaggedMissing { get; set; }
    public bool IsFlaggedOutlier { get; set; }

    public string? this[string columnName]
    {
        get => Cells.TryGetValue(columnName, out var value) ? value : null;
        set => Cells[columnName] = value;
    }
}
