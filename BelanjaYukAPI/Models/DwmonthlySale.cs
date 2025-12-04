using System;
using System.Collections.Generic;

namespace BelanjaYukAPI.Models;

public partial class DwmonthlySale
{
    public Guid Id { get; set; }

    public string? Sellerid { get; set; }

    public string? Monthyear { get; set; }

    public decimal? Totalsales { get; set; }

    public int? Totalqty { get; set; }

    public decimal? Avgrating { get; set; }

    public DateTime? Loaddate { get; set; }
}
