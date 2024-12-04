﻿using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class TbDoiTuongChinhSachCanBo
{
    public int IdDoiTuongChinhSachCanBo { get; set; }

    public int? IdCanBo { get; set; }

    public int? IdDoiTuongChinhSach { get; set; }

    public DateOnly? TuNgay { get; set; }

    public DateOnly? DenNgay { get; set; }

    public virtual TbCanBo? IdCanBoNavigation { get; set; }

    public virtual DmDoiTuongChinhSach? IdDoiTuongChinhSachNavigation { get; set; }
}
