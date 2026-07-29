using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataWiz.Models;

namespace DataWiz.Services;

public interface ICsvService
{
    Task<DatasetModel> LoadAsync(string filePath);
    Task ExportAsync(DatasetModel dataset, string filePath);
}