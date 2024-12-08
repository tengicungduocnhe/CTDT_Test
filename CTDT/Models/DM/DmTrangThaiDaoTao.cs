﻿using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class DmTrangThaiDaoTao
{
    public int IdTrangThaiDaoTao { get; set; }

    public string? TrangThaiDaoTao { get; set; }

    public virtual ICollection<TbDanhSachNganhDaoTao> TbDanhSachNganhDaoTaos { get; set; } = new List<TbDanhSachNganhDaoTao>();
}
