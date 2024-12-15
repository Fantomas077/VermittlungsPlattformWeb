using System;
using System.Collections.Generic;

namespace VermittlungsPlattform.Models.Db;

public partial class CompanyGallery
{
    public int Id { get; set; }

    public int? CompanyProfileId { get; set; }

    public string? ImageName { get; set; }
}
