﻿using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class DmSinhVienNam
{
    public int IdSinhVienNam { get; set; }

    public string? SinhVienNam { get; set; }

    public virtual ICollection<TbThongTinHocTapNghienCuuSinh> TbThongTinHocTapNghienCuuSinhs { get; set; } = new List<TbThongTinHocTapNghienCuuSinh>();
}
