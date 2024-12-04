using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class DmNgoaiNgu
{
    public int IdNgoaiNgu { get; set; }

    public string? NgoaiNgu { get; set; }

    public virtual ICollection<TbNgonNguGiangDay> TbNgonNguGiangDays { get; set; } = new List<TbNgonNguGiangDay>();

    public virtual ICollection<TbNguoi> TbNguois { get; set; } = new List<TbNguoi>();
}
