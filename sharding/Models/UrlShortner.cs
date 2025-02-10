using System;
using System.Collections.Generic;

namespace sharding.Models;

public partial class UrlShortner
{
    public int Id { get; set; }

    public string? Url { get; set; }

    public string? UrlId { get; set; }
}
