using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CTDT.Models;

public partial class TbThongTinKiemDinhCuaChuongTrinh
{
    [RegularExpression(@"^\d+$", ErrorMessage = "ID phải là số nguyên dương")]
    [Display(Name = "ID Thông Tin Kiểm Định Của Chương Trình")]
    public int IdThongTinKiemDinhCuaChuongTrinh { get; set; }

  [Display(Name = "Chương Trình Đào Tạo")]
    public int IdChuongTrinhDaoTao { get; set; }

    [Display(Name = "Tổ Chức Kiểm Định")]
    public int? IdToChucKiemDinh { get; set; }

    [Display(Name = "Kết Chức Kiểm Định")]
    public int? IdKetQuaKiemDinh { get; set; }

    [Display(Name = "Số Quyết Định")]
  //  [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "chỉ được chứa ký tự chữ và số.")]
    public string? SoQuyetDinh { get; set; }

    [Display(Name = "Ngày Cấp Chứng Nhận")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? NgayCapChungNhanKiemDinh { get; set; }

    [Display(Name = "Thời Hạn")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? ThoiHanKiemDinh { get; set; }

    [Display(Name = "ID Chương Trình Đào Tạo")]
    public virtual TbChuongTrinhDaoTao? IdChuongTrinhDaoTaoNavigation { get; set; }

    [Display(Name = "ID Kết Quả Kiểm Định")]
    public virtual DmKetQuaKiemDinh? IdKetQuaKiemDinhNavigation { get; set; }

    [Display(Name = "ID Tổ Chức Kiểm Định")]
    public virtual DmToChucKiemDinh? IdToChucKiemDinhNavigation { get; set; }
}

