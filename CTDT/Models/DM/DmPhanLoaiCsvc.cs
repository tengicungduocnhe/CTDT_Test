using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class DmPhanLoaiCsvc
{
    public int IdPhanLoaiCsvc { get; set; }

    public string? PhanLoaiCsvc { get; set; }

    public virtual ICollection<TbPhongHocGiangDuongHoiTruong> TbPhongHocGiangDuongHoiTruongs { get; set; } = new List<TbPhongHocGiangDuongHoiTruong>();
}
