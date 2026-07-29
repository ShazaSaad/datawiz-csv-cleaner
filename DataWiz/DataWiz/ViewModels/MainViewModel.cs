using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataWiz.Models;
using DataWiz.Services;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace DataWiz.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ICsvService _csvService = new CsvService();

    private DatasetModel? currentDataset;
    public DatasetModel? CurrentDataset
    {
        get => currentDataset;
        set => SetProperty(ref currentDataset, value);
    }

    private string statusText = "No file loaded.";
    public string StatusText
    {
        get => statusText;
        set => SetProperty(ref statusText, value);
    }

    public ObservableCollection<DataRow> Rows { get; } = new();

    public IAsyncRelayCommand LoadFileCommand { get; }
    public IAsyncRelayCommand ExportFileCommand { get; }

    public MainViewModel()
    {
        LoadFileCommand = new AsyncRelayCommand(LoadFileAsync);
        ExportFileCommand = new AsyncRelayCommand(ExportFileAsync);
    }

    // MainPage passes its Window in so we can attach the file picker correctly
    public Window? HostWindow { get; set; }

    private async Task LoadFileAsync()
    {
        var picker = new FileOpenPicker();
        InitializeWithWindow.Initialize(picker, WindowNative.GetWindowHandle(HostWindow));

        picker.FileTypeFilter.Add(".csv");
        picker.SuggestedStartLocation = PickerLocationId.Downloads;

        StorageFile? file = await picker.PickSingleFileAsync();
        if (file is null) return;

        StatusText = "Loading...";
        CurrentDataset = await _csvService.LoadAsync(file.Path);

        Rows.Clear();
        foreach (var row in CurrentDataset.Rows)
            Rows.Add(row);

        StatusText = $"{CurrentDataset.FileName} — {CurrentDataset.RowCount} rows, {CurrentDataset.ColumnCount} columns";
    }

    private async Task ExportFileAsync()
    {
        if (CurrentDataset is null) return;

        var picker = new FileSavePicker();
        InitializeWithWindow.Initialize(picker, WindowNative.GetWindowHandle(HostWindow));

        picker.FileTypeChoices.Add("CSV File", new List<string> { ".csv" });
        picker.SuggestedFileName = $"cleaned_{CurrentDataset.FileName}";

        StorageFile? file = await picker.PickSaveFileAsync();
        if (file is null) return;

        await _csvService.ExportAsync(CurrentDataset, file.Path);
        StatusText = $"Exported to {file.Name}";
    }
}