using System;
using System.Collections.Generic;

namespace VermittlungsPlattform.Models.Db;

public partial class StudentenInteresse
{
    public int Id { get; set; }

    public int? StudentprofileId { get; set; }

    public string? StudentInteresse { get; set; }
}
