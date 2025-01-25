using System;
using System.Collections.Generic;

namespace VermittlungsPlattform.Models.Db;

public partial class StelleBewerbung
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int StelleId { get; set; }

    public int UnternhemenId { get; set; }

    public int StudentProfilId { get; set; }

    public string Anschreiben { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime ApplyDate { get; set; }

    public string Cv { get; set; } = null!;
}
