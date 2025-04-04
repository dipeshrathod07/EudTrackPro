using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Models;

public class t_class
{
    [Key]

    public int c_class_id { get; set; }

    public string c_class_name { get; set; }
}
