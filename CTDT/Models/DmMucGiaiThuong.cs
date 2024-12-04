﻿using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class DmMucGiaiThuong
{
    public int IdMucGiaiThuong { get; set; }

    public string? MucGiaiThuong { get; set; }

    public virtual ICollection<TbGiaiThuongKhoaHoc> TbGiaiThuongKhoaHocs { get; set; } = new List<TbGiaiThuongKhoaHoc>();
}
