using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class DmHinhThucTuyenDung
{
    public int IdHinhThucTuyenDung { get; set; }

    public string? HinhThucTuyenDung { get; set; }

    public virtual ICollection<TbThongTinViecLamSauTotNghiep> TbThongTinViecLamSauTotNghieps { get; set; } = new List<TbThongTinViecLamSauTotNghiep>();
}
