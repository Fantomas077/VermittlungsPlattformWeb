using System;
using System.Collections.Generic;

namespace VermittlungsPlattform.Models.Db;

public partial class CompanyInteresse
{
    public int Id { get; set; }

    public int? UnternehmenprofilId { get; set; }

    public string? UnternehmenInteresse { get; set; }
}
