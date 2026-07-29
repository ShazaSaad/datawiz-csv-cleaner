using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataWiz.Models;

public enum CleaningOperationType
{
    DropMissingRows,
    FillMissingMean,
    FillMissingMedian,
    FillMissingMode,
    FillMissingCustom,
    RemoveOutliers,
    TrimWhitespace,
    DropDuplicateRows
}

public class CleaningOperation
{
    public CleaningOperationType Type { get; set; }
    public string? TargetColumn { get; set; }   // null = applies to whole dataset
    public string? CustomFillValue { get; set; } // only used for FillMissingCustom
    public DateTime AppliedAt { get; set; } = DateTime.Now;
}