using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using DataWiz.Models;

namespace DataWiz.ViewModels;

public partial class ColumnInspectorViewModel : ObservableObject
{
    private ColumnInfo? selectedColumn;
    private int totalRowCount;

    public ColumnInfo? SelectedColumn
    {
        get => selectedColumn;
        set => SetProperty(ref selectedColumn, value);
    }

    public int TotalRowCount
    {
        get => totalRowCount;
        set => SetProperty(ref totalRowCount, value);
    }

    public double MissingPercentage =>
        SelectedColumn is null ? 0 : SelectedColumn.MissingPercentage(TotalRowCount);
    public string MissingPercentageDisplay => $"{MissingPercentage:F1}%";
    public void SetColumn(ColumnInfo column, int totalRows)
    {
        SelectedColumn = column;
        TotalRowCount = totalRows;
        OnPropertyChanged(nameof(MissingPercentage));
        OnPropertyChanged(nameof(MissingPercentageDisplay));
    }
}