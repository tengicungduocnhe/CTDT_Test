using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CTDT.Models;

public partial class TbNamApDungChuongTrinh
{
    public int IdNamApDungChuongTrinh { get; set; }

    public int? IdChuongTrinhDaoTao { get; set; }

    [Display(Name = "Số tín chỉ tối thiểu để tốt nghiệp")]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = " Chỉ được chứa ký tự số.")]
    public int? SoTinChiToiThieuDeTotNghiep { get; set; }
    [Display(Name = "Tổng học phí toàn khoá")]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = " Chỉ được chứa ký tự số.")]
    public int? TongHocPhiToanKhoa { get; set; }

    [Display(Name = "Năm áp dụng")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? NamApDung { get; set; }


    [Display(Name = "Chỉ tiêu tuyển sinh hằng năm")]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = " Chỉ được chứa ký tự số.")]
    public int? ChiTieuTuyenSinhHangNam { get; set; }

    public virtual TbChuongTrinhDaoTao? IdChuongTrinhDaoTaoNavigation { get; set; }
}

