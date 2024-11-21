using System;
using System.Collections.Generic;

namespace VermittlungsPlattform.Models.Db;

public partial class UnternehmenProfile
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Branche { get; set; }

    public string Location { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Webseite { get; set; }

    public string? Link { get; set; }

    public string? ImageName { get; set; }
}
