using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CTDT.Models;

public partial class TbThongTinKiemDinhCuaChuongTrinh
{
    [RegularExpression(@"^\d+$", ErrorMessage = "ID phải là số nguyên dương")]
    [Display(Name = "ID")]
    public int IdThongTinKiemDinhCuaChuongTrinh { get; set; }

    [Display(Name = "CHƯƠNG TRÌNH ĐÀO TẠO")]
    public int IdChuongTrinhDaoTao { get; set; }

    [Display(Name = "TỔ CHỨC TÌM KIẾM")]
    public int? IdToChucKiemDinh { get; set; }

    [Display(Name = "KẾT QUẢ KIỂM ĐỊNH")]
    public int? IdKetQuaKiemDinh { get; set; }

    [Display(Name = "SỐ QUYẾT ĐỊNH")]
    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "chỉ được chứa ký tự chữ và số.")]
    public string? SoQuyetDinh { get; set; }

    [Display(Name = "NGÀY CẤP CHỨNG NHẬN")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? NgayCapChungNhanKiemDinh { get; set; }

    [Display(Name = "THỜI HẠN")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? ThoiHanKiemDinh { get; set; }

    public virtual TbChuongTrinhDaoTao? IdChuongTrinhDaoTaoNavigation { get; set; } ;

    public virtual DmKetQuaKiemDinh? IdKetQuaKiemDinhNavigation { get; set; }

    public virtual DmToChucKiemDinh? IdToChucKiemDinhNavigation { get; set; }
}

