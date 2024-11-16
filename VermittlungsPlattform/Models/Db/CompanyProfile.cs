using System;
using System.Collections.Generic;

namespace VermittlungsPlattform.Models.Db;

public partial class CompanyProfile
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string? Branche { get; set; }

    public string Description { get; set; } = null!;

    public string? Webseite { get; set; }

    public string? Imagename { get; set; }

    public string? Link { get; set; }
}
