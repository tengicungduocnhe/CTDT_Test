using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CTDT.Models;

public partial class TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai

{
    [Display(Name = "Id Quyết định cấp phép chương trình dùng cho chương trình nước ngoài")]
    public int IdQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai { get; set; }


    [Display(Name = "ID chương trình đào tạo")]
    public int? IdChuongTrinhDaoTao { get; set; }


    [Display(Name = "ID loại quyết định")]
    public int? IdLoaiQuyetDinh { get; set; }

    [Display(Name = "số quyết định")]
    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Chỉ được chứa ký tự chữ và số.")]
    public string? SoQuyetDinh { get; set; }

    [Display(Name = "Ngày ban hành quyết định")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? NgayBanHanhQuyetDinh { get; set; }

    [Display(Name = "ID Hình thức đào tạo")]
    public int? IdHinhThucDaoTao { get; set; }

    public virtual TbChuongTrinhDaoTao? IdChuongTrinhDaoTaoNavigation { get; set; }

    public virtual DmHinhThucDaoTao? IdHinhThucDaoTaoNavigation { get; set; }

    public virtual DmLoaiQuyetDinh? IdLoaiQuyetDinhNavigation { get; set; }
}