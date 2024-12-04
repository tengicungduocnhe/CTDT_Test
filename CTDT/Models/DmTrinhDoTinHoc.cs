using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class DmTrinhDoTinHoc
{
    public int IdTrinhDoTinHoc { get; set; }

    public string? TrinhDoTinHoc { get; set; }

    public virtual ICollection<TbNguoi> TbNguois { get; set; } = new List<TbNguoi>();
}
