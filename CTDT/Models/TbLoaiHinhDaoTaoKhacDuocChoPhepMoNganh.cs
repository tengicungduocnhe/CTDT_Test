﻿using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class TbLoaiHinhDaoTaoKhacDuocChoPhepMoNganh
{
    public int IdLoaiHinhDaoTaoKhacDuocChoPhepMoNganh { get; set; }

    public int? IdNganhDaoTao { get; set; }

    public int? IdLoaiHinhDaoTao { get; set; }

    public string? SoQuyetDinhChoPhep { get; set; }

    public DateOnly? NgayBanHanhQuyetDinhChoPhep { get; set; }

    public virtual DmLoaiHinhDaoTao? IdLoaiHinhDaoTaoNavigation { get; set; }

    public virtual DmNganhDaoTao? IdNganhDaoTaoNavigation { get; set; }
}
