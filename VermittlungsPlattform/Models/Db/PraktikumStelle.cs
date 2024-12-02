using System;
using System.Collections.Generic;

namespace VermittlungsPlattform.Models.Db;

public partial class PraktikumStelle
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int UnternehmenProfileId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string Branche { get; set; } = null!;

    public int Dauer { get; set; }

    public string Skills { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public string Tags { get; set; } = null!;

    public string Arbeitsyp { get; set; } = null!;

    public decimal Gehalt { get; set; }

   
}
