﻿using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class DmDauMoiLienHe
{
    public int IdDauMoiLienHe { get; set; }

    public string? DauMoiLienHe { get; set; }

    public virtual ICollection<TbDauMoiLienHe> TbDauMoiLienHes { get; set; } = new List<TbDauMoiLienHe>();
}
