﻿using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class DmQuyetDinhTuChu
{
    public int IdQuyetDinhTuChu { get; set; }

    public string? QuyetDinhTuChu { get; set; }

    public virtual ICollection<TbDanhSachNganhDaoTao> TbDanhSachNganhDaoTaos { get; set; } = new List<TbDanhSachNganhDaoTao>();
}
