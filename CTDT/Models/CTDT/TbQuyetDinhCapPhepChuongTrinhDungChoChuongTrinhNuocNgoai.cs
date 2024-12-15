using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CTDT.Models;

public partial class TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai

{
    [Display(Name = "Id Quyết Định Cấp Phép Chương Trình Dùng Cho Chương Trình Nước Ngoài")]
    public int IdQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai { get; set; }


    [Display(Name = "ID Chương Trình Đào Tạo")]
    public int? IdChuongTrinhDaoTao { get; set; }


    [Display(Name = "ID loại Quyết Định")]
    public int? IdLoaiQuyetDinh { get; set; }

    [Display(Name = "Số Quyết Định")]
    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Chỉ được chứa ký tự chữ và số.")]
    public string? SoQuyetDinh { get; set; }

    [Display(Name = "Ngày Ban Hành Quyết Định")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? NgayBanHanhQuyetDinh { get; set; }

    [Display(Name = "ID Hình thức đào tạo")]
    public int? IdHinhThucDaoTao { get; set; }

    [Display(Name = "ID Chương Trình Đào Tạo")]
    public virtual TbChuongTrinhDaoTao? IdChuongTrinhDaoTaoNavigation { get; set; }

    [Display(Name = "ID Hình Thức Đào Tạo")]
    public virtual DmHinhThucDaoTao? IdHinhThucDaoTaoNavigation { get; set; }

    [Display(Name = "ID Loại Quyết Định")]
    public virtual DmLoaiQuyetDinh? IdLoaiQuyetDinhNavigation { get; set; }
}