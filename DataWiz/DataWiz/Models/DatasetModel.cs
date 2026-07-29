using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataWiz.Models;

public class DatasetModel
{
    public string FileName { get; set; } = string.Empty;
    public List<ColumnInfo> Columns { get; set; } = new();
    public List<DataRow> Rows { get; set; } = new();

    public int RowCount => Rows.Count;
    public int ColumnCount => Columns.Count;

    public double OverallMissingPercentage()
    {
        if (RowCount == 0 || ColumnCount == 0) return 0;
        int totalCells = RowCount * ColumnCount;
        int totalMissing = Columns.Sum(c => c.MissingCount);
        return (double)totalMissing / totalCells * 100;
    }
}