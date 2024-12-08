﻿using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class DmLoaiHocBong
{
    public int IdLoaiHocBong { get; set; }

    public string? LoaiHocBong { get; set; }

    public virtual ICollection<TbThongTinHocBong> TbThongTinHocBongs { get; set; } = new List<TbThongTinHocBong>();
}
