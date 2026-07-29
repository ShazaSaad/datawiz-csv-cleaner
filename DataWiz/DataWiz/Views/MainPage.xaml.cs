using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using CommunityToolkit.WinUI.UI.Controls;
using DataWiz.ViewModels;
using System.ComponentModel;

namespace DataWiz.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel { get; } = new();
    private void DataGridControl_CurrentCellChanged(object? sender, EventArgs e)
    {
        var columnHeader = DataGridControl.CurrentColumn?.Header?.ToString();
        if (columnHeader is null || ViewModel.CurrentDataset is null) return;

        var columnInfo = ViewModel.CurrentDataset.Columns.FirstOrDefault(c => c.Name == columnHeader);
        if (columnInfo is not null)
            ViewModel.Inspector.SetColumn(columnInfo, ViewModel.CurrentDataset.RowCount);
    }
    public MainPage()
    {
        InitializeComponent();
        DataContext = ViewModel;
        // HostWindow is assigned by MainWindow after navigation to ensure a valid instance
        ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        DataGridControl.CurrentCellChanged += DataGridControl_CurrentCellChanged;

    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // Rebuild grid columns whenever a new dataset finishes loading
        if (e.PropertyName == nameof(ViewModel.CurrentDataset) && ViewModel.CurrentDataset is not null)
        {
            BuildColumns();
        }
    }

    private void BuildColumns()
    {
        DataGridControl.Columns.Clear();
        if (ViewModel.CurrentDataset is null) return;

        foreach (var column in ViewModel.CurrentDataset.Columns)
        {
            var textColumn = new DataGridTextColumn
            {
                Header = column.Name,
                Binding = new Binding { Path = new PropertyPath($"Cells[{column.Name}]") }
            };
            DataGridControl.Columns.Add(textColumn);
        }
    }

    private async void LoadButton_Click(object sender, RoutedEventArgs e)
        => await ViewModel.LoadFileCommand.ExecuteAsync(null);

    private async void ExportButton_Click(object sender, RoutedEventArgs e)
        => await ViewModel.ExportFileCommand.ExecuteAsync(null);
}
