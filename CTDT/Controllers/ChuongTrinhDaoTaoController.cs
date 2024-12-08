﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CTDT.Models;
using CTDT.API;
using Microsoft.AspNetCore.Authorization;
//using CTDT.Models.DM;

namespace CTDT.Controllers
{
    [Authorize]
    public class ChuongTrinhDaoTaoController : Controller
    {
        private readonly ApiServices ApiServices_;
        // Lấy từ HemisContext 
        public ChuongTrinhDaoTaoController(ApiServices services)
        {
            ApiServices_ = services;
        }

        // GET: ChuongTrinhDaoTao
        // Lấy danh sách CTĐT từ database, trả về view Index.
        private async Task<List<TbChuongTrinhDaoTao>> TbChuongTrinhDaoTaos()
        {
            List<TbChuongTrinhDaoTao> getall = await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao");
            List<DmDonViCapBang> dmDonViCapBangs = await ApiServices_.GetAll<DmDonViCapBang>("/api/dm/DonViCapBang");
            List<DmHocCheDaoTao> dmHocCheDaoTaos = await ApiServices_.GetAll<DmHocCheDaoTao>("/api/dm/HocCheDaoTao");
            List<DmLoaiChuongTrinhDaoTao> dmLoaiChuongTrinhDaoTaos = await ApiServices_.GetAll<DmLoaiChuongTrinhDaoTao>("/api/dm/LoaiChuongTrinhDaoTao");
            List<DmLoaiChuongTrinhLienKetDaoTao> dmLoaiChuongTrinhLienKetDaoTaos = await ApiServices_.GetAll<DmLoaiChuongTrinhLienKetDaoTao>("/api/dm/LoaiChuongTrinhLienKetDaoTao");
            List<DmNganhDaoTao> dmNganhDaoTaos = await ApiServices_.GetAll<DmNganhDaoTao>("/api/dm/NganhDaoTao");
            List<DmQuocTich> dmQuocTiches = await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich");
            List<DmTrangThaiChuongTrinhDaoTao> dmTrangThaiChuongTrinhDaoTaos = await ApiServices_.GetAll<DmTrangThaiChuongTrinhDaoTao>("/api/dm/TrangThaiChuongTrinhDaoTao");
            List<DmTrinhDoDaoTao> dmTrinhDoDaoTaos = await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao");

            getall.ForEach(item =>
            {
                item.IdDonViCapBangNavigation = dmDonViCapBangs.FirstOrDefault(t => t.IdDonViCapBang == item.IdDonViCapBang);

                item.IdHocCheDaoTaoNavigation = dmHocCheDaoTaos.FirstOrDefault(t => t.IdHocCheDaoTao == item.IdHocCheDaoTao);

                item.IdLoaiChuongTrinhDaoTaoNavigation = dmLoaiChuongTrinhDaoTaos.FirstOrDefault(t => t.IdLoaiChuongTrinhDaoTao == item.IdLoaiChuongTrinhDaoTao);

                item.IdLoaiChuongTrinhLienKetDaoTaoNavigation = dmLoaiChuongTrinhLienKetDaoTaos.FirstOrDefault(t => t.IdLoaiChuongTrinhLienKetDaoTao == item.IdLoaiChuongTrinhLienKetDaoTao);

                item.IdNganhDaoTaoNavigation = dmNganhDaoTaos.FirstOrDefault(t => t.IdNganhDaoTao == item.IdNganhDaoTao);

                item.IdQuocGiaCuaTruSoChinhNavigation = dmQuocTiches.FirstOrDefault(t => t.IdQuocTich == item.IdQuocGiaCuaTruSoChinh);

                item.IdTrangThaiCuaChuongTrinhNavigation = dmTrangThaiChuongTrinhDaoTaos.FirstOrDefault(t => t.IdTrangThaiChuongTrinhDaoTao == item.IdTrangThaiCuaChuongTrinh);

                item.IdTrinhDoDaoTaoNavigation = dmTrinhDoDaoTaos.FirstOrDefault(t => t.IdTrinhDoDaoTao == item.IdTrinhDoDaoTao);

            }
            );
            return getall;
        }

        public async Task<IActionResult> Index(string Id, string SapXep)
        {
            try
            {

                List<TbChuongTrinhDaoTao> tbChuongTrinhDaoTaos = await TbChuongTrinhDaoTaos();
                var danhSach = tbChuongTrinhDaoTaos.Where(item => string.IsNullOrEmpty(Id) || item.IdChuongTrinhDaoTao.ToString() == Id) //  tìm kiếm theo Id CTDT
                .ToList();

                var sapXepDanhSach = danhSach; // sắp xếp
                if (SapXep == "SapXep")
                {
                    sapXepDanhSach = danhSach.OrderBy(x => x.NgayBanHanhChuanDauRa).ToList();// sắp xếp ngày ban hành chuẩn đầu ra
                }

                ViewBag.KqTimKiem = danhSach;
                ViewBag.KqSapXep = sapXepDanhSach;

                return View(sapXepDanhSach);
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // Lấy chi tiết 1 bản ghi dựa theo ID tương ứng đã truyền vào (IdChuongTrinhDaoTao)
        // Hiển thị bản ghi đó ở view Details
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                List<TbChuongTrinhDaoTao> tbChuongTrinhDaoTaos = await TbChuongTrinhDaoTaos();

                var tbChuongTrinhDaoTao = tbChuongTrinhDaoTaos.FirstOrDefault(m => m.IdChuongTrinhDaoTao == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbChuongTrinhDaoTao == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbChuongTrinhDaoTao);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: ChuongTrinhDaoTao/Create
        // Hiển thị view Create để tạo một bản ghi CTĐT mới
        // Truyền data từ các table khác hiển thị tại view Create (khóa ngoài)
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdDonViCapBang"] = new SelectList(await ApiServices_.GetAll<DmDonViCapBang>("/api/dm/DonViCapBang"), "IdDonViCapBang", "DonViCapBang");
                ViewData["IdHocCheDaoTao"] = new SelectList(await ApiServices_.GetAll<DmHocCheDaoTao>("/api/dm/HocCheDaoTao"), "IdHocCheDaoTao", "HocCheDaoTao");
                ViewData["IdLoaiChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<DmLoaiChuongTrinhDaoTao>("/api/dm/LoaiChuongTrinhDaoTao"), "IdLoaiChuongTrinhDaoTao", "LoaiChuongTrinhDaoTao");
                ViewData["IdLoaiChuongTrinhLienKetDaoTao"] = new SelectList(await ApiServices_.GetAll<DmLoaiChuongTrinhLienKetDaoTao>("/api/dm/LoaiChuongTrinhLienKetDaoTao"), "IdLoaiChuongTrinhLienKetDaoTao", "LoaiChuongTrinhLienKetDaoTao");
                ViewData["IdNganhDaoTao"] = new SelectList(await ApiServices_.GetAll<DmNganhDaoTao>("/api/dm/NganhDaoTao"), "IdNganhDaoTao", "NganhDaoTao");
                ViewData["IdQuocGiaCuaTruSoChinh"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc");
                ViewData["IdTrangThaiCuaChuongTrinh"] = new SelectList(await ApiServices_.GetAll<DmTrangThaiChuongTrinhDaoTao>("/api/dm/TrangThaiChuongTrinhDaoTao"), "IdTrangThaiChuongTrinhDaoTao", "TrangThaiChuongTrinhDaoTao");
                ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao");
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: ChuongTrinhDaoTao/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        // Thêm một CTĐT mới vào Database nếu IdChuongTrinhDaoTao truyền vào không trùng với Id đã có trong Database
        // Trong trường hợp nhập trùng IdChuongTrinhDaoTao sẽ bắt lỗi
        // Bắt lỗi ngoại lệ sao cho người nhập BẮT BUỘC phải nhập khác IdChuongTrinhDaoTao đã có
        [HttpPost]
        [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
        public async Task<IActionResult> Create([Bind("IdChuongTrinhDaoTao,MaChuongTrinhDaoTao,IdNganhDaoTao,TenChuongTrinh,TenChuongTrinhBangTiengAnh,NamBatDauTuyenSinh,TenCoSoDaoTaoNuocNgoai,IdLoaiChuongTrinhDaoTao,IdLoaiChuongTrinhLienKetDaoTao,DiaDiemDaoTao,IdHocCheDaoTao,IdQuocGiaCuaTruSoChinh,NgayBanHanhChuanDauRa,IdTrinhDoDaoTao,ThoiGianDaoTaoChuan,ChuanDauRa,IdDonViCapBang,LoaiChungChiDuocChapThuan,DonViThucHienChuongTrinh,IdTrangThaiCuaChuongTrinh,ChuanDauRaVeNgoaiNgu,ChuanDauRaVeTinHoc,HocPhiTaiVietNam,HocPhiTaiNuocNgoai")] TbChuongTrinhDaoTao tbChuongTrinhDaoTao)
        {
            try
            {
                // Nếu trùng IDChuongTrinhDaoTao sẽ báo lỗi
                check_null(tbChuongTrinhDaoTao);
                check_int(tbChuongTrinhDaoTao);

                if (await TbChuongTrinhDaoTaoExists(tbChuongTrinhDaoTao.IdChuongTrinhDaoTao)) ModelState.AddModelError("IdChuongTrinhDaoTao", "ID này đã tồn tại!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao", tbChuongTrinhDaoTao);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdDonViCapBang"] = new SelectList(await ApiServices_.GetAll<DmDonViCapBang>("/api/dm/DonViCapBang"), "IdDonViCapBang", "DonViCapBang", tbChuongTrinhDaoTao.IdDonViCapBang);
                ViewData["IdHocCheDaoTao"] = new SelectList(await ApiServices_.GetAll<DmHocCheDaoTao>("/api/dm/HocCheDaoTao"), "IdHocCheDaoTao", "HocCheDaoTao", tbChuongTrinhDaoTao.IdHocCheDaoTao);
                ViewData["IdLoaiChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<DmLoaiChuongTrinhDaoTao>("/api/dm/LoaiChuongTrinhDaoTao"), "IdLoaiChuongTrinhDaoTao", "LoaiChuongTrinhDaoTao", tbChuongTrinhDaoTao.IdLoaiChuongTrinhDaoTao);
                ViewData["IdLoaiChuongTrinhLienKetDaoTao"] = new SelectList(await ApiServices_.GetAll<DmLoaiChuongTrinhLienKetDaoTao>("/api/dm/LoaiChuongTrinhLienKetDaoTao"), "IdLoaiChuongTrinhLienKetDaoTao", "LoaiChuongTrinhLienKetDaoTao", tbChuongTrinhDaoTao.IdLoaiChuongTrinhDaoTao);
                ViewData["IdNganhDaoTao"] = new SelectList(await ApiServices_.GetAll<DmNganhDaoTao>("/api/dm/NganhDaoTao"), "IdNganhDaoTao", "NganhDaoTao", tbChuongTrinhDaoTao.IdNganhDaoTao);
                ViewData["IdQuocGiaCuaTruSoChinh"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc", tbChuongTrinhDaoTao.IdQuocGiaCuaTruSoChinh);
                ViewData["IdTrangThaiCuaChuongTrinh"] = new SelectList(await ApiServices_.GetAll<DmTrangThaiChuongTrinhDaoTao>("/api/dm/TrangThaiChuongTrinhDaoTao"), "IdTrangThaiChuongTrinhDaoTao", "TrangThaiChuongTrinhDaoTao", tbChuongTrinhDaoTao.IdTrangThaiCuaChuongTrinh);
                ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao", tbChuongTrinhDaoTao.IdTrinhDoDaoTao);
                return View(tbChuongTrinhDaoTao);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: ChuongTrinhDaoTao/Edit
        // Lấy data từ Database với Id đã có, sau đó hiển thị ở view Edit
        // Nếu không tìm thấy Id tương ứng sẽ báo lỗi NotFound
        // Phương thức này gần giống Create, nhưng nó nhập dữ liệu vào Id đã có trong database
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var tbChuongTrinhDaoTao = await ApiServices_.GetId<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao", id ?? 0);

                if (tbChuongTrinhDaoTao == null)
                {
                    return NotFound();
                }
                ViewData["IdDonViCapBang"] = new SelectList(await ApiServices_.GetAll<DmDonViCapBang>("/api/dm/DonViCapBang"), "IdDonViCapBang", "DonViCapBang", tbChuongTrinhDaoTao.IdDonViCapBang);
                ViewData["IdHocCheDaoTao"] = new SelectList(await ApiServices_.GetAll<DmHocCheDaoTao>("/api/dm/HocCheDaoTao"), "IdHocCheDaoTao", "HocCheDaoTao", tbChuongTrinhDaoTao.IdHocCheDaoTao);
                ViewData["IdLoaiChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<DmLoaiChuongTrinhDaoTao>("/api/dm/LoaiChuongTrinhDaoTao"), "IdLoaiChuongTrinhDaoTao", "LoaiChuongTrinhDaoTao", tbChuongTrinhDaoTao.IdLoaiChuongTrinhDaoTao);
                ViewData["IdLoaiChuongTrinhLienKetDaoTao"] = new SelectList(await ApiServices_.GetAll<DmLoaiChuongTrinhLienKetDaoTao>("/api/dm/LoaiChuongTrinhLienKetDaoTao"), "IdLoaiChuongTrinhLienKetDaoTao", "LoaiChuongTrinhLienKetDaoTao", tbChuongTrinhDaoTao.IdLoaiChuongTrinhDaoTao);
                ViewData["IdNganhDaoTao"] = new SelectList(await ApiServices_.GetAll<DmNganhDaoTao>("/api/dm/NganhDaoTao"), "IdNganhDaoTao", "NganhDaoTao", tbChuongTrinhDaoTao.IdNganhDaoTao);
                ViewData["IdQuocGiaCuaTruSoChinh"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc", tbChuongTrinhDaoTao.IdQuocGiaCuaTruSoChinh);
                ViewData["IdTrangThaiCuaChuongTrinh"] = new SelectList(await ApiServices_.GetAll<DmTrangThaiChuongTrinhDaoTao>("/api/dm/TrangThaiChuongTrinhDaoTao"), "IdTrangThaiChuongTrinhDaoTao", "TrangThaiChuongTrinhDaoTao", tbChuongTrinhDaoTao.IdTrangThaiCuaChuongTrinh);
                ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao", tbChuongTrinhDaoTao.IdTrinhDoDaoTao);
                return View(tbChuongTrinhDaoTao);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: ChuongTrinhDaoTao/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        // Lưu data mới (ghi đè) vào các trường Data đã có thuộc IdChuongTrinhDaoTao cần chỉnh sửa
        // Nó chỉ cập nhật khi ModelState hợp lệ
        // Nếu không hợp lệ sẽ báo lỗi, vì vậy cần có bắt lỗi.

        [HttpPost]
        [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
        public async Task<IActionResult> Edit(int id, [Bind("IdChuongTrinhDaoTao,MaChuongTrinhDaoTao,IdNganhDaoTao,TenChuongTrinh,TenChuongTrinhBangTiengAnh,NamBatDauTuyenSinh,TenCoSoDaoTaoNuocNgoai,IdLoaiChuongTrinhDaoTao,IdLoaiChuongTrinhLienKetDaoTao,DiaDiemDaoTao,IdHocCheDaoTao,IdQuocGiaCuaTruSoChinh,NgayBanHanhChuanDauRa,IdTrinhDoDaoTao,ThoiGianDaoTaoChuan,ChuanDauRa,IdDonViCapBang,LoaiChungChiDuocChapThuan,DonViThucHienChuongTrinh,IdTrangThaiCuaChuongTrinh,ChuanDauRaVeNgoaiNgu,ChuanDauRaVeTinHoc,HocPhiTaiVietNam,HocPhiTaiNuocNgoai")] TbChuongTrinhDaoTao tbChuongTrinhDaoTao)
        {
            try
            {
                if (id != tbChuongTrinhDaoTao.IdChuongTrinhDaoTao)
                {
                    return NotFound();
                }

                check_null(tbChuongTrinhDaoTao);
                check_int(tbChuongTrinhDaoTao);

                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao", id, tbChuongTrinhDaoTao);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbChuongTrinhDaoTaoExists(tbChuongTrinhDaoTao.IdChuongTrinhDaoTao) == false)
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdDonViCapBang"] = new SelectList(await ApiServices_.GetAll<DmDonViCapBang>("/api/dm/DonViCapBang"), "IdDonViCapBang", "DonViCapBang", tbChuongTrinhDaoTao.IdDonViCapBang);
                ViewData["IdHocCheDaoTao"] = new SelectList(await ApiServices_.GetAll<DmHocCheDaoTao>("/api/dm/HocCheDaoTao"), "IdHocCheDaoTao", "HocCheDaoTao", tbChuongTrinhDaoTao.IdHocCheDaoTao);
                ViewData["IdLoaiChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<DmLoaiChuongTrinhDaoTao>("/api/dm/LoaiChuongTrinhDaoTao"), "IdLoaiChuongTrinhDaoTao", "LoaiChuongTrinhDaoTao", tbChuongTrinhDaoTao.IdLoaiChuongTrinhDaoTao);
                ViewData["IdLoaiChuongTrinhLienKetDaoTao"] = new SelectList(await ApiServices_.GetAll<DmLoaiChuongTrinhLienKetDaoTao>("/api/dm/LoaiChuongTrinhLienKetDaoTao"), "IdLoaiChuongTrinhLienKetDaoTao", "LoaiChuongTrinhLienKetDaoTao", tbChuongTrinhDaoTao.IdLoaiChuongTrinhDaoTao);
                ViewData["IdNganhDaoTao"] = new SelectList(await ApiServices_.GetAll<DmNganhDaoTao>("/api/dm/NganhDaoTao"), "IdNganhDaoTao", "NganhDaoTao", tbChuongTrinhDaoTao.IdNganhDaoTao);
                ViewData["IdQuocGiaCuaTruSoChinh"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc", tbChuongTrinhDaoTao.IdQuocGiaCuaTruSoChinh);
                ViewData["IdTrangThaiCuaChuongTrinh"] = new SelectList(await ApiServices_.GetAll<DmTrangThaiChuongTrinhDaoTao>("/api/dm/TrangThaiChuongTrinhDaoTao"), "IdTrangThaiChuongTrinhDaoTao", "TrangThaiChuongTrinhDaoTao", tbChuongTrinhDaoTao.IdTrangThaiCuaChuongTrinh);
                ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao", tbChuongTrinhDaoTao.IdTrinhDoDaoTao);
                return View(tbChuongTrinhDaoTao);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: ChuongTrinhDaoTao/Delete
        // Xóa một CTĐT khỏi Database
        // Lấy data CTĐT từ Database, hiển thị Data tại view Delete
        // Hàm này để hiển thị thông tin cho người dùng trước khi xóa
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var tbChuongTrinhDaoTaos = await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao");
                var tbChuongTrinhDaoTao = tbChuongTrinhDaoTaos.FirstOrDefault(m => m.IdChuongTrinhDaoTao == id);
                if (tbChuongTrinhDaoTao == null)
                {
                    return NotFound();
                }

                return View(tbChuongTrinhDaoTao);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: ChuongTrinhDaoTao/Delete
        // Xóa CTĐT khỏi Database sau khi nhấn xác nhận 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // Lệnh xác nhận xóa hẳn một CTĐT
        {
            try
            {
                await ApiServices_.Delete<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        private async Task<bool> TbChuongTrinhDaoTaoExists(int id)
        {
            var tbChuongTrinhDaoTaos = await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao");
            return tbChuongTrinhDaoTaos.Any(e => e.IdChuongTrinhDaoTao == id);
        }

        private void check_null(TbChuongTrinhDaoTao tbChuongTrinhDaoTao)
        {
            if (tbChuongTrinhDaoTao.MaChuongTrinhDaoTao == null) ModelState.AddModelError("MaChuongTrinhDaoTao", "Vui lòng nhập vào ô trống!");
            if (tbChuongTrinhDaoTao.IdNganhDaoTao == null) ModelState.AddModelError("IdNganhDaoTao", "Vui lòng nhập vào ô trống!");
            if (tbChuongTrinhDaoTao.TenChuongTrinh == null) ModelState.AddModelError("TenChuongTrinh", "Không được bỏ trống!");
            if (tbChuongTrinhDaoTao.TenChuongTrinhBangTiengAnh == null) ModelState.AddModelError("TenChuongTrinhBangTiengAnh", "Không được bỏ trống!");
            if (tbChuongTrinhDaoTao.NamBatDauTuyenSinh == null) ModelState.AddModelError("NamBatDauTuyenSinh", "Không được bỏ trống!");
            if (tbChuongTrinhDaoTao.TenCoSoDaoTaoNuocNgoai == null) ModelState.AddModelError("TenCoSoDaoTaoNuocNgoai", "Không được bỏ trống!");
            if (tbChuongTrinhDaoTao.IdLoaiChuongTrinhDaoTao == null) ModelState.AddModelError("IdLoaiChuongTrinhDaoTao", "Không được bỏ trống!");
            if (tbChuongTrinhDaoTao.IdLoaiChuongTrinhLienKetDaoTao == null) ModelState.AddModelError("IdLoaiChuongTrinhLienKetDaoTao", "Không được bỏ trống!");
            if (tbChuongTrinhDaoTao.DiaDiemDaoTao == null) ModelState.AddModelError("DiaDiemDaoTao", "Không được bỏ trống!");
            if (tbChuongTrinhDaoTao.IdHocCheDaoTao == null) ModelState.AddModelError("IdHocCheDaoTao", "Không được bỏ trống!");
            if (tbChuongTrinhDaoTao.IdQuocGiaCuaTruSoChinh == null) ModelState.AddModelError("IdQuocGiaCuaTruSoChinh", "Không được bỏ trống!");
            if (tbChuongTrinhDaoTao.NgayBanHanhChuanDauRa == null) ModelState.AddModelError("NgayBanHanhChuanDauRa", "Không được bỏ trống!");
            if (tbChuongTrinhDaoTao.IdTrinhDoDaoTao == null) ModelState.AddModelError("IdTrinhDoDaoTao", "Không được bỏ trống!");
            if (tbChuongTrinhDaoTao.ThoiGianDaoTaoChuan == null) ModelState.AddModelError("ThoiGianDaoTaoChuan", "Không được bỏ trống!");
            if (tbChuongTrinhDaoTao.ChuanDauRa == null) ModelState.AddModelError("ChuanDauRa", "Không được bỏ trống!");
            if (tbChuongTrinhDaoTao.IdDonViCapBang == null) ModelState.AddModelError("IdDonViCapBang", "Không được bỏ trống!");
            if (tbChuongTrinhDaoTao.LoaiChungChiDuocChapThuan == null) ModelState.AddModelError("LoaiChungChiDuocChapThuan", "Không được bỏ trống!");
            if (tbChuongTrinhDaoTao.DonViThucHienChuongTrinh == null) ModelState.AddModelError("DonViThucHienChuongTrinh", "Không được bỏ trống!");
            if (tbChuongTrinhDaoTao.IdTrangThaiCuaChuongTrinh == null) ModelState.AddModelError("IdTrangThaiCuaChuongTrinh", "Không được bỏ trống!");
            if (tbChuongTrinhDaoTao.ChuanDauRaVeNgoaiNgu == null) ModelState.AddModelError("ChuanDauRaVeNgoaiNgu", "Không được bỏ trống!");
            if (tbChuongTrinhDaoTao.ChuanDauRaVeTinHoc == null) ModelState.AddModelError("ChuanDauRaVeTinHoc", "Không được bỏ trống!");
            if (tbChuongTrinhDaoTao.HocPhiTaiVietNam == null) ModelState.AddModelError("HocPhiTaiVietNam", "Không được bỏ trống!");
            if (tbChuongTrinhDaoTao.HocPhiTaiNuocNgoai == null) ModelState.AddModelError("HocPhiTaiNuocNgoai", "Không được bỏ trống!");
        }
        private void check_int(TbChuongTrinhDaoTao tbChuongTrinhDaoTao)
        {
            if (tbChuongTrinhDaoTao.HocPhiTaiVietNam.HasValue && tbChuongTrinhDaoTao.HocPhiTaiVietNam % 1 != 0) ModelState.AddModelError("HocPhiTaiVietNam", "Vui lòng nhập số nguyên vào ô trống!");
            if (tbChuongTrinhDaoTao.HocPhiTaiNuocNgoai.HasValue && tbChuongTrinhDaoTao.HocPhiTaiNuocNgoai % 1 != 0) ModelState.AddModelError("HocPhiTaiNuocNgoai", "Vui lòng nhập số nguyên vào ô trống!");
            if (tbChuongTrinhDaoTao.ThoiGianDaoTaoChuan.HasValue && tbChuongTrinhDaoTao.ThoiGianDaoTaoChuan % 1 != 0) ModelState.AddModelError("ThoiGianDaoTaoChuan", "Vui lòng nhập số nguyên vào ô trống!");
        }
    }
}
