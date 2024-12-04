﻿using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class DmTrangThaiHopDong
{
    public int IdTrangThaiHopDong { get; set; }

    public string? TrangThaiHopDong { get; set; }

    public virtual ICollection<TbChuyenGiaoCongNgheVaDaoTao> TbChuyenGiaoCongNgheVaDaoTaos { get; set; } = new List<TbChuyenGiaoCongNgheVaDaoTao>();

    public virtual ICollection<TbHopDongThinhGiang> TbHopDongThinhGiangs { get; set; } = new List<TbHopDongThinhGiang>();
}
