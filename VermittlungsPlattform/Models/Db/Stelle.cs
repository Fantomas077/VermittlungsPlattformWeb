using System;
using System.Collections.Generic;

namespace VermittlungsPlattform.Models.Db;

public partial class Stelle
{
    public int Id { get; set; }

    public int UnternehmenProfileId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string Branche { get; set; } = null!;

    public int Duration { get; set; }

    public string Skills { get; set; } = null!;

    public string Tags { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public string ArbeitsTyp { get; set; } = null!;

    public decimal? Gehalt { get; set; }
}
