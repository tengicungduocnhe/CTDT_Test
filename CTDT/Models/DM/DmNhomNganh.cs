﻿using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class DmNhomNganh
{
    public int IdNhomNganh { get; set; }

    public string? NhomNganh { get; set; }

    public virtual ICollection<DmLinhVucNhomNganh> DmLinhVucNhomNganhs { get; set; } = new List<DmLinhVucNhomNganh>();

    public virtual ICollection<TbNhomNganhDaoTao> TbNhomNganhDaoTaos { get; set; } = new List<TbNhomNganhDaoTao>();
}
