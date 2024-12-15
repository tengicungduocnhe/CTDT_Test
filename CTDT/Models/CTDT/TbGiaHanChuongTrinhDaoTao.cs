using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CTDT.Models;

public partial class TbGiaHanChuongTrinhDaoTao
{
    [DisplayName(displayName: "Id Chương Trình Đào Tạo")]
    public int IdGiaHanChuongTrinhDaoTao { get; set; }
    [DisplayName(displayName: " Tên Chương Trình Đào Tạo")]
    public int? IdChuongTrinhDaoTao { get; set; }
    public string? TenChuongTrinh { get; set; }

    [DisplayName(displayName: "Số Quyết Định Gia Hạn")]
    [RegularExpression(@"^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪỬỮỰỲỴÝỶỸỳỵýỷỹ\s]+$", ErrorMessage = "Chỉ được chứa ký tự chữ cái tiếng Việt và dấu cách.")]
    public string? SoQuyetDinhGiaHan { get; set; }

    [DisplayName(displayName: "Ngày Ban Hành Văn Bản Gia Hạn")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? NgayBanHanhVanBanGiaHan { get; set; }


    [DisplayName(displayName: "Số Lần Gia Hạn")]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = " Chỉ được chứa ký tự số.")]
    public int? GiaHanLanThu { get; set; }
    [DisplayName(displayName: "ID Chương Trình Đào Tạo")]
    public virtual TbChuongTrinhDaoTao? IdChuongTrinhDaoTaoNavigation { get; set; }

}