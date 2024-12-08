using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class DmLoaiPhongBan
{
    public int IdLoaiPhongBan { get; set; }

    public string? LoaiPhongBan { get; set; }

    public virtual ICollection<TbCoCauToChuc> TbCoCauToChucs { get; set; } = new List<TbCoCauToChuc>();
}
