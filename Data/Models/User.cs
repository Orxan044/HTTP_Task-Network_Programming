﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models;

public class User : BaseEntity
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public int? Age { get; set; }
}
