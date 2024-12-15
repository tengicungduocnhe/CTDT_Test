using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CTDT.Models;

public partial class TbThongTinKiemDinhCuaChuongTrinh
{
    [RegularExpression(@"^\d+$", ErrorMessage = "ID phải là số nguyên dương")]
    [Display(Name = "ID")]
    public int IdThongTinKiemDinhCuaChuongTrinh { get; set; }

 //   [Display(Name = "CHƯƠNG TRÌNH ĐÀO TẠO")]
    public int IdChuongTrinhDaoTao { get; set; }

    [Display(Name = "Tổ chức kiểm định")]
    public int? IdToChucKiemDinh { get; set; }

    [Display(Name = "Kết quả kiểm định")]
    public int? IdKetQuaKiemDinh { get; set; }

    [Display(Name = "Số quết định")]
    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "chỉ được chứa ký tự chữ và số.")]
    public string? SoQuyetDinh { get; set; }

    [Display(Name = "Ngày cấp chứng nhận")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? NgayCapChungNhanKiemDinh { get; set; }

    [Display(Name = "Thời hạn")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? ThoiHanKiemDinh { get; set; }

    public virtual TbChuongTrinhDaoTao? IdChuongTrinhDaoTaoNavigation { get; set; }

    public virtual DmKetQuaKiemDinh? IdKetQuaKiemDinhNavigation { get; set; }

    public virtual DmToChucKiemDinh? IdToChucKiemDinhNavigation { get; set; }
}

