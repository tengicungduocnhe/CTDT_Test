﻿using System;
using System.Collections.Generic;

namespace CTDT.Models;

public partial class DmNguonKinhPhiCuaGvduocCuDiDaoTao
{
    public int IdNguonKinhPhiCuaGvduocCuDiDaoTao { get; set; }

    public string? NguonKinhPhiCuaGvduocCuDiDaoTao { get; set; }

    public virtual ICollection<TbGvduocCuDiDaoTao> TbGvduocCuDiDaoTaos { get; set; } = new List<TbGvduocCuDiDaoTao>();
}
