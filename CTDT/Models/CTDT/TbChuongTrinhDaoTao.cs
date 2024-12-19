﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CTDT.Models;

public partial class TbChuongTrinhDaoTao
{
    [DisplayName(" ID chương trình đào tạo")]
    public int IdChuongTrinhDaoTao { get; set; }


    [DisplayName("Mã chương trình đào tạo ")]
    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Chỉ được chứa ký tự chữ và số.")]
    public string? MaChuongTrinhDaoTao { get; set; }


    [DisplayName("Ngành đào tạo")]
    public int? IdNganhDaoTao { get; set; }


    [DisplayName("Tên chương trình")]
    [RegularExpression(@"^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪỬỮỰỲỴÝỶỸỳỵýỷỹ\s]+$", ErrorMessage = "Chỉ được chứa ký tự chữ cái tiếng Việt và dấu cách.")]
    public string? TenChuongTrinh { get; set; }


    [DisplayName("Tên chương trình bằng tiếng Anh")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Chỉ được chứa ký tự chữ.")]
    public string? TenChuongTrinhBangTiengAnh { get; set; }


    [DisplayName("Năm bắt đầu tuyển sinh")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? NamBatDauTuyenSinh { get; set; }


    [DisplayName("Tên cơ sở đào tạo nước ngoài")]
    [RegularExpression(@"^[a-zA-aăâbcdđeêghiklmnoôơpqrstuưvxyAĂÂBCDĐEÊGHIKLMNOÔƠPQRSTUƯVXYàáảãạằắẳẵặầấẩẫậèéẻẽẹềếểễệìíỉĩịòóỏõọồốổỗộờớởỡợùúủũụừứửữựỳýỷỹỵÀÁẢÃẠẰẮẲẴẶẦẤẨẪẬÈÉẺẼẸỀẾỂỄỆÌÍỈĨỊÒÓỎÕỌỒỐỔỖỘỜỚỞỠỢÙÚỦŨỤỪỨỬỮỰỲÝỶỸỴ\s]+$", ErrorMessage = "Chỉ được chứa ký tự chữ cái tiếng Việt và dấu cách.")]

    public string? TenCoSoDaoTaoNuocNgoai { get; set; }


    [DisplayName("Loại chương trình đào tạo ")]

    public int? IdLoaiChuongTrinhDaoTao { get; set; }


    [DisplayName("Loại chương trình liên kết đào tạo")]
    public int? IdLoaiChuongTrinhLienKetDaoTao { get; set; }


    [DisplayName("Địa điểm đào tạo")]
    [RegularExpression(@"^[a-zA-aăâbcdđeêghiklmnoôơpqrstuưvxyAĂÂBCDĐEÊGHIKLMNOÔƠPQRSTUƯVXYàáảãạằắẳẵặầấẩẫậèéẻẽẹềếểễệìíỉĩịòóỏõọồốổỗộờớởỡợùúủũụừứửữựỳýỷỹỵÀÁẢÃẠẰẮẲẴẶẦẤẨẪẬÈÉẺẼẸỀẾỂỄỆÌÍỈĨỊÒÓỎÕỌỒỐỔỖỘỜỚỞỠỢÙÚỦŨỤỪỨỬỮỰỲÝỶỸỴ\s]+$", ErrorMessage = "Chỉ được chứa ký tự chữ cái tiếng Việt và dấu cách.")]

    public string? DiaDiemDaoTao { get; set; }


    [DisplayName("Học chế đào tạo")]
    public int? IdHocCheDaoTao { get; set; }


    [DisplayName("Quốc gia trụ sở chính")]
    public int? IdQuocGiaCuaTruSoChinh { get; set; }


    [DisplayName("Ngày ban hành chuẩn đầu ra")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly? NgayBanHanhChuanDauRa { get; set; }


    [DisplayName("Trình độ đào tạo")]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = " Chỉ được chứa ký tự số.")]
    public int? IdTrinhDoDaoTao { get; set; }


    [DisplayName("Thời gian đào tạo chuẩn")]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = " Chỉ được chứa ký tự số.")]

    public int? ThoiGianDaoTaoChuan { get; set; }


    [DisplayName("Chuẩn đầu ra")]
    [RegularExpression(@"^[a-zA-aăâbcdđeêghiklmnoôơpqrstuưvxyAĂÂBCDĐEÊGHIKLMNOÔƠPQRSTUƯVXYàáảãạằắẳẵặầấẩẫậèéẻẽẹềếểễệìíỉĩịòóỏõọồốổỗộờớởỡợùúủũụừứửữựỳýỷỹỵÀÁẢÃẠẰẮẲẴẶẦẤẨẪẬÈÉẺẼẸỀẾỂỄỆÌÍỈĨỊÒÓỎÕỌỒỐỔỖỘỜỚỞỠỢÙÚỦŨỤỪỨỬỮỰỲÝỶỸỴ\s]+$", ErrorMessage = "Chỉ được chứa ký tự chữ cái tiếng Việt và dấu cách.")]

    public string? ChuanDauRa { get; set; }


    [DisplayName("Đơn vị cấp bằng")]
    public int? IdDonViCapBang { get; set; }


    [DisplayName("Loại chứng chỉ được chấp thuận")]
    [RegularExpression(@"^[a-zA-aăâbcdđeêghiklmnoôơpqrstuưvxyAĂÂBCDĐEÊGHIKLMNOÔƠPQRSTUƯVXYàáảãạằắẳẵặầấẩẫậèéẻẽẹềếểễệìíỉĩịòóỏõọồốổỗộờớởỡợùúủũụừứửữựỳýỷỹỵÀÁẢÃẠẰẮẲẴẶẦẤẨẪẬÈÉẺẼẸỀẾỂỄỆÌÍỈĨỊÒÓỎÕỌỒỐỔỖỘỜỚỞỠỢÙÚỦŨỤỪỨỬỮỰỲÝỶỸỴ\s]+$", ErrorMessage = "Chỉ được chứa ký tự chữ cái tiếng Việt và dấu cách.")]

    public string? LoaiChungChiDuocChapThuan { get; set; }


    [DisplayName("Đơn vị thực hiện chương trình")]
    [RegularExpression(@"^[a-zA-aăâbcdđeêghiklmnoôơpqrstuưvxyAĂÂBCDĐEÊGHIKLMNOÔƠPQRSTUƯVXYàáảãạằắẳẵặầấẩẫậèéẻẽẹềếểễệìíỉĩịòóỏõọồốổỗộờớởỡợùúủũụừứửữựỳýỷỹỵÀÁẢÃẠẰẮẲẴẶẦẤẨẪẬÈÉẺẼẸỀẾỂỄỆÌÍỈĨỊÒÓỎÕỌỒỐỔỖỘỜỚỞỠỢÙÚỦŨỤỪỨỬỮỰỲÝỶỸỴ\s]+$", ErrorMessage = "Chỉ được chứa ký tự chữ cái tiếng Việt và dấu cách.")]

    public string? DonViThucHienChuongTrinh { get; set; }


    [DisplayName("Trạng thái của chương trình")]
    public int? IdTrangThaiCuaChuongTrinh { get; set; }


    [DisplayName("Chuẩn đầu ra về ngoại ngữ")]
    [RegularExpression(@"^[a-zA-aăâbcdđeêghiklmnoôơpqrstuưvxyAĂÂBCDĐEÊGHIKLMNOÔƠPQRSTUƯVXYàáảãạằắẳẵặầấẩẫậèéẻẽẹềếểễệìíỉĩịòóỏõọồốổỗộờớởỡợùúủũụừứửữựỳýỷỹỵÀÁẢÃẠẰẮẲẴẶẦẤẨẪẬÈÉẺẼẸỀẾỂỄỆÌÍỈĨỊÒÓỎÕỌỒỐỔỖỘỜỚỞỠỢÙÚỦŨỤỪỨỬỮỰỲÝỶỸỴ\s]+$", ErrorMessage = "Chỉ được chứa ký tự chữ cái tiếng Việt và dấu cách.")]

    public string? ChuanDauRaVeNgoaiNgu { get; set; }


    [DisplayName("Chuẩn đầu ra về tin học")]
    [RegularExpression(@"^[a-zA-aăâbcdđeêghiklmnoôơpqrstuưvxyAĂÂBCDĐEÊGHIKLMNOÔƠPQRSTUƯVXYàáảãạằắẳẵặầấẩẫậèéẻẽẹềếểễệìíỉĩịòóỏõọồốổỗộờớởỡợùúủũụừứửữựỳýỷỹỵÀÁẢÃẠẰẮẲẴẶẦẤẨẪẬÈÉẺẼẸỀẾỂỄỆÌÍỈĨỊÒÓỎÕỌỒỐỔỖỘỜỚỞỠỢÙÚỦŨỤỪỨỬỮỰỲÝỶỸỴ\s]+$", ErrorMessage = "Chỉ được chứa ký tự chữ cái tiếng Việt và dấu cách.")]

    public string? ChuanDauRaVeTinHoc { get; set; }


    [DisplayName("Học phí tại Việt Nam")]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = " Chỉ được chứa ký tự số.")]

    public int? HocPhiTaiVietNam { get; set; }


    [DisplayName("Học phí tại nước ngoài")]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = " Chỉ được chứa ký tự số.")]

    public int? HocPhiTaiNuocNgoai { get; set; }


    [DisplayName("Đơn Vị Cấp Bằng Navigation ")]
    public virtual DmDonViCapBang? IdDonViCapBangNavigation { get; set; }


    [DisplayName("Học Chế Đào Tạo Navigation ")]
    public virtual DmHocCheDaoTao? IdHocCheDaoTaoNavigation { get; set; }


    [DisplayName("Loại Chương Trình Đào Tạo Navigation ")]
    public virtual DmLoaiChuongTrinhDaoTao? IdLoaiChuongTrinhDaoTaoNavigation { get; set; }


    [DisplayName("Loại Chương Trình Liên Kết Đào Tạo Navigation ")]
    public virtual DmLoaiChuongTrinhLienKetDaoTao? IdLoaiChuongTrinhLienKetDaoTaoNavigation { get; set; }


    [DisplayName("Ngành Đào Tạo Navigation ")]
    public virtual DmNganhDaoTao? IdNganhDaoTaoNavigation { get; set; }


    [DisplayName("Quốc Gia Của Trụ Sở Chính Navigation ")]
    public virtual DmQuocTich? IdQuocGiaCuaTruSoChinhNavigation { get; set; }


    [DisplayName("Trạng Thái Của Chương Trình Navigation ")]
    public virtual DmTrangThaiChuongTrinhDaoTao? IdTrangThaiCuaChuongTrinhNavigation { get; set; }


    [DisplayName(" Trình Độ Đào Tạo Navigation ")]
    public virtual DmTrinhDoDaoTao? IdTrinhDoDaoTaoNavigation { get; set; }


    [DisplayName("Gia Hạn Chương Trình Đào Tạo ")]
    public virtual ICollection<TbGiaHanChuongTrinhDaoTao> TbGiaHanChuongTrinhDaoTaos { get; set; } = new List<TbGiaHanChuongTrinhDaoTao>();


    [DisplayName("Năm Áp Dụng Chương Trình ")]
    public virtual ICollection<TbNamApDungChuongTrinh> TbNamApDungChuongTrinhs { get; set; } = new List<TbNamApDungChuongTrinh>();


    [DisplayName("Ngôn Ngữ Giảng Dạy ")]
    public virtual ICollection<TbNgonNguGiangDay> TbNgonNguGiangDays { get; set; } = new List<TbNgonNguGiangDay>();


    [DisplayName("Quyết Định Cấp Phép Chương Trình Dùng Cho Chương Trình Nước Ngoài ")]
    public virtual ICollection<TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai> TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoais { get; set; } = new List<TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai>();


    [DisplayName("Thông Tin Kiểm Định Của Chương Trình ")]
    public virtual ICollection<TbThongTinKiemDinhCuaChuongTrinh> TbThongTinKiemDinhCuaChuongTrinhs { get; set; } = new List<TbThongTinKiemDinhCuaChuongTrinh>();
}

