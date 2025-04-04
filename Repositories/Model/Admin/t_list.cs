using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Models;

public class t_list
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? ParentId { get; set; }
}
