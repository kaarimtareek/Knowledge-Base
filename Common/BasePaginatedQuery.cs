﻿namespace Common;

public abstract record BasePaginatedQuery : BaseQuery
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}