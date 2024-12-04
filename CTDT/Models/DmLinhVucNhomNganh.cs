﻿using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class DmLinhVucNhomNganh
{
    public int IdLinhVucNhomNganh { get; set; }

    public int? IdLinhVucDaoTao { get; set; }

    public int? IdNhomNganh { get; set; }

    public virtual DmLinhVucDaoTao? IdLinhVucDaoTaoNavigation { get; set; }

    public virtual DmNhomNganh? IdNhomNganhNavigation { get; set; }
}
