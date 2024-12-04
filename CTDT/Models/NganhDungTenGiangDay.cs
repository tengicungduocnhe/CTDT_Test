using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class NganhDungTenGiangDay
{
    public int IdNganhGiangDay { get; set; }

    public int? IdCanBo { get; set; }

    public int? IdNganhDaoTao { get; set; }

    public double? TrongSo { get; set; }
}
