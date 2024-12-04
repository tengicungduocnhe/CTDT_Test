﻿using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class VThoaThuanHopTacQuocTe
{
    public string? MaThoaThuan { get; set; }

    public string? TenThoaThuan { get; set; }

    public string? NoiDungTomTat { get; set; }

    public string? TenToChuc { get; set; }

    public DateOnly? NgayKyKet { get; set; }

    public string? SoVanBanKyKet { get; set; }

    public string? TenNuoc { get; set; }

    public DateOnly? NgayHetHan { get; set; }
}
