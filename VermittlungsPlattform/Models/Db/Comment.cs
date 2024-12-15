using System;
using System.Collections.Generic;

namespace VermittlungsPlattform.Models.Db;

public partial class Comment
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string CommentText { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public int UnternehmenId { get; set; }
}
