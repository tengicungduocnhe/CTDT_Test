﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CTDT.Models;

public partial class TbNgonNguGiangDay
{
    [DisplayName(displayName: "ID Ngôn Ngữ Giảng Dạy")]
    public int IdNgonNguGiangDay { get; set; }
    [DisplayName(displayName: "Chương Trình Đào Tạo")]
    public int? IdChuongTrinhDaoTao { get; set; }
    [DisplayName(displayName: "Ngôn Ngữ")]
    public int? IdNgonNgu { get; set; }
    [DisplayName(displayName: "Trình Độ Ngôn Ngữ Đầu Vào")]
    public int? IdTrinhDoNgonNguDauVao { get; set; }
    [DisplayName(displayName: "Chương Trình Đào Tạo")]
    public virtual TbChuongTrinhDaoTao? IdChuongTrinhDaoTaoNavigation { get; set; }
    [DisplayName(displayName: "Ngôn Ngữ")]
    public virtual DmNgoaiNgu? IdNgonNguNavigation { get; set; }
    [DisplayName(displayName: "Trình Độ Ngôn Ngữ Đầu Vào")]
    public virtual DmKhungNangLucNgoaiNgu? IdTrinhDoNgonNguDauVaoNavigation { get; set; }
}
