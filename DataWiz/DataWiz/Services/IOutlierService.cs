using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataWiz.Models;

namespace DataWiz.Services;

public interface IOutlierService
{
    void FlagOutliers(DatasetModel dataset);
    void FlagMissing(DatasetModel dataset);
}