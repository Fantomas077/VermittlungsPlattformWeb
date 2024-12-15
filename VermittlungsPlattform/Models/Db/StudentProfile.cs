using System;
using System.Collections.Generic;

namespace VermittlungsPlattform.Models.Db;

public partial class StudentProfile
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Apropos { get; set; } = null!;

    public string Fachrichtung { get; set; } = null!;

    public string Studiengang { get; set; } = null!;

    public string Schwerpunkte { get; set; } = null!;

    public string Skills { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string Geschlecht { get; set; } = null!;

    public string Abschluss { get; set; } = null!;

    public string? Cvname { get; set; }

    public string? Instagram { get; set; }

    public string? Github { get; set; }

    public string? Linkedin { get; set; }

    public string? Facebook { get; set; }

    public string? Twitter { get; set; }
}
