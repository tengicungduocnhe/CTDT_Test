﻿using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class DmLoaiGiangVienQuocPhong
{
    public int IdLoaiGiangVienQuocPhong { get; set; }

    public string? LoaiGiangVienQuocPhong { get; set; }

    public virtual ICollection<TbGiaoVienQpan> TbGiaoVienQpans { get; set; } = new List<TbGiaoVienQpan>();
}
