using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Models;

public class t_notification
{
    public int c_notification_id { get; set; }

    public string c_title_name { get; set; }

    public string c_title_description { get; set; }

    public string c_receiver { get; set; }

    public DateTime c_notification_date { get; set; } = DateTime.UtcNow;
}
