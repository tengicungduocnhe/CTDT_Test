﻿using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class DmTrangThaiChuongTrinhDaoTao
{
    public int IdTrangThaiChuongTrinhDaoTao { get; set; }

    public string? TrangThaiChuongTrinhDaoTao { get; set; }

    public virtual ICollection<TbChuongTrinhDaoTao> TbChuongTrinhDaoTaos { get; set; } = new List<TbChuongTrinhDaoTao>();
}
