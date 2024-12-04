using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class DmLoaiPhongHoc
{
    public int IdLoaiPhongHoc { get; set; }

    public string? LoaiPhongHoc { get; set; }

    public virtual ICollection<TbPhongHocGiangDuongHoiTruong> TbPhongHocGiangDuongHoiTruongs { get; set; } = new List<TbPhongHocGiangDuongHoiTruong>();
}
