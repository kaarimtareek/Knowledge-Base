using System.ComponentModel.DataAnnotations;
using Common.Constants;

namespace Common;

public abstract record BaseQuery
{
    [MaxLength(ModelConstants.MaxLength.Name)]
    public string Search { get; init; } = string.Empty;
    public string SortBy { get; init; } = string.Empty;
    public bool SortDescending { get; init; } = false;
}