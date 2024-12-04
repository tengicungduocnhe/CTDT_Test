﻿using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class TbThanhPhanThamGiaDoanCongTac
{
    public int IdThanhPhanThamGiaDoanCongTac { get; set; }

    public int? IdDoanCongTac { get; set; }

    public int? IdCanBo { get; set; }

    public int? IdVaiTroThamGia { get; set; }

    public virtual TbCanBo? IdCanBoNavigation { get; set; }

    public virtual TbDoanCongTac? IdDoanCongTacNavigation { get; set; }

    public virtual DmVaiTroThamGium? IdVaiTroThamGiaNavigation { get; set; }
}
