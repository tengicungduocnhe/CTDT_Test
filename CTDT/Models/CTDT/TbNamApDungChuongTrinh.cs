using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CTDT.Models;

public partial class TbNamApDungChuongTrinh
{
    [Display(Name = "ID Năm Áp Dụng Chương Trình")]
    public int IdNamApDungChuongTrinh { get; set; }

    [Display(Name = "Chương Trình Đào Tạo")]
    public int? IdChuongTrinhDaoTao { get; set; }
    public string? TenChuongTrinh { get; set; }

    [Display(Name = "Số Tín Chỉ Tối Thiểu Để Tốt Nghiệp")]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = " Chỉ được chứa ký tự số.")]
    public int? SoTinChiToiThieuDeTotNghiep { get; set; }
    [Display(Name = "Tổng Học Phí Toàn Khóa")]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = " Chỉ được chứa ký tự số.")]
    public int? TongHocPhiToanKhoa { get; set; }

    [Display(Name = "Năm Áp Dụng")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? NamApDung { get; set; }


    [Display(Name = "Chỉ Tiêu Tuyển Sinh Hàng Năm")]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = " Chỉ được chứa ký tự số.")]
    public int? ChiTieuTuyenSinhHangNam { get; set; }


    [Display(Name = "ID Chương Trình Đào Tạo")]
    public virtual TbChuongTrinhDaoTao? IdChuongTrinhDaoTaoNavigation { get; set; }
}

