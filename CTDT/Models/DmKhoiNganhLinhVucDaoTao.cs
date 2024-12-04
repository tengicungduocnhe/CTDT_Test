﻿using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class DmKhoiNganhLinhVucDaoTao
{
    public int IdKhoiNganhLinhVucNghienCuu { get; set; }

    public int? IdKhoiNganh { get; set; }

    public int? IdLinhVucDaoTao { get; set; }

    public virtual TbKhoiNganhDaoTao? IdKhoiNganhNavigation { get; set; }

    public virtual DmLinhVucDaoTao? IdLinhVucDaoTaoNavigation { get; set; }
}
