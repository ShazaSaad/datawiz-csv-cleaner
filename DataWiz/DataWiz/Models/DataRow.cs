using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;

namespace DataWiz.Models;

public class DataRow
{
    public Dictionary<string, string?> Cells { get; set; } = new();

    public bool IsFlaggedMissing { get; set; }
    public bool IsFlaggedOutlier { get; set; }

    public Brush RowBackground =>
        IsFlaggedOutlier
            ? new SolidColorBrush(Colors.IndianRed) { Opacity = 0.3 }
            : IsFlaggedMissing
                ? new SolidColorBrush(Colors.Orange) { Opacity = 0.3 }
                : new SolidColorBrush(Colors.Transparent);

    public string? this[string columnName]
    {
        get => Cells.TryGetValue(columnName, out var value) ? value : null;
        set => Cells[columnName] = value;
    }
}
