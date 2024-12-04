﻿using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class TbThoaThuanHopTacQuocTe
{
    public int IdThoaThuanHopTacQuocTe { get; set; }

    public string? MaThoaThuan { get; set; }

    public string? TenThoaThuan { get; set; }

    public string? NoiDungTomTat { get; set; }

    public string? TenToChuc { get; set; }

    public DateOnly? NgayKyKet { get; set; }

    public string? SoVanBanKyKet { get; set; }

    public int? IdQuocGia { get; set; }

    public DateOnly? NgayHetHan { get; set; }

    public virtual DmQuocTich? IdQuocGiaNavigation { get; set; }
}
