using System;
using System.Collections.Generic;

namespace CI_platform.Entities.DataModels;

public partial class EnableUserStatus
{
    public long EnableuserId { get; set; }

    public long? NotificationId { get; set; }

    public long? UserId { get; set; }

    public int? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual NotificationTitle? Notification { get; set; }

    public virtual User? User { get; set; }
}
