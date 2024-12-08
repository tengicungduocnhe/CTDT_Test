﻿using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class DmPhuongThucKhenThuong
{
    public int IdPhuongThucKhenThuong { get; set; }

    public string? PhuongThucKhenThuong { get; set; }

    public virtual ICollection<TbDanhHieuThiDuaGiaiThuongKhenThuongCanBo> TbDanhHieuThiDuaGiaiThuongKhenThuongCanBos { get; set; } = new List<TbDanhHieuThiDuaGiaiThuongKhenThuongCanBo>();

    public virtual ICollection<TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd> TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGds { get; set; } = new List<TbDanhHieuThiDuaGiaiThuongKhenThuongCuaCoSoGd>();

    public virtual ICollection<TbDanhHieuThiDuaGiaiThuongKhenThuongNguoiHoc> TbDanhHieuThiDuaGiaiThuongKhenThuongNguoiHocs { get; set; } = new List<TbDanhHieuThiDuaGiaiThuongKhenThuongNguoiHoc>();
}
