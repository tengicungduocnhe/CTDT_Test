using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class DmLoaiTaiSanTriTue
{
    public int IdLoaiTaiSanTriTue { get; set; }

    public string? LoaiTaiSanTriTue { get; set; }

    public virtual ICollection<TbTaiSanTriTue> TbTaiSanTriTues { get; set; } = new List<TbTaiSanTriTue>();
}
