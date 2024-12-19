﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CTDT.Models;
using CTDT.API;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using ExcelDataReader;
using System.Text;
//using CTDT.Models.DM;
namespace CTDT.Controllers
{
    [Authorize]
    public class QuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoaiController : Controller
    {
        private readonly ApiServices ApiServices_;
        private readonly DbHemisC500Context _dbcontext;
        public QuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoaiController(ApiServices services, DbHemisC500Context dbcontext)
        {
            ApiServices_ = services;
            _dbcontext = dbcontext;
        }
        public IActionResult chartjs()
        {
            return View(); // Nó sẽ trả về view chartjs.cshtml
        }
        private async Task<List<TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai>> TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoais()
        {
            List<TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai> getall = await ApiServices_.GetAll<TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai>("/api/ctdt/QuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai");

            List<TbChuongTrinhDaoTao> tbChuongTrinhDaoTaos = await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao");


            List<DmHinhThucDaoTao> dmHinhThucDaoTaos = await ApiServices_.GetAll<DmHinhThucDaoTao>("/api/dm/HinhThucDaoTao");

            List<DmLoaiQuyetDinh> dmLoaiQuyetDinhs = await ApiServices_.GetAll<DmLoaiQuyetDinh>("/api/dm/LoaiQuyetDinh");

            getall.ForEach(item =>
            {

                item.IdChuongTrinhDaoTaoNavigation = tbChuongTrinhDaoTaos.FirstOrDefault(t => t.IdChuongTrinhDaoTao == item.IdChuongTrinhDaoTao);

                item.IdLoaiQuyetDinhNavigation = dmLoaiQuyetDinhs.FirstOrDefault(t => t.IdLoaiQuyetDinh == item.IdLoaiQuyetDinh);

                item.IdHinhThucDaoTaoNavigation = dmHinhThucDaoTaos.FirstOrDefault(t => t.IdHinhThucDaoTao == item.IdHinhThucDaoTao);
            }
            );
            return getall;
        }
        // GET: Quyết định cấp phép chương trình dùng cho chương trình nước ngoài
        // Lấy danh sách QuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai từ database, trả về view Index.
        public async Task<IActionResult> Index(string Id, string SapXep)
        {
            try
            {

                List<TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai> tbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoais = await TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoais();

                var danhSach = tbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoais.Where(item => string.IsNullOrEmpty(Id) || item.IdQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai.ToString() == Id) //  tìm kiếm theo Id GHCTDT
                .ToList();

                var sapXepDanhSach = danhSach; // sắp xếp
                if (SapXep == "SapXep")
                {
                    sapXepDanhSach = danhSach.OrderBy(x => x.NgayBanHanhQuyetDinh).ToList();// sắp xếp ngày ban hành Quyết định
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

        // Lấy chi tiết 1 bản ghi dựa theo ID tương ứng đã truyền vào ()
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
                List<TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai> tbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoais = await TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoais();

                var tb = tbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoais.FirstOrDefault(m => m.IdQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tb == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết QuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai thành công
                return View(tb);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: ChuongTrinhDaoTao/Create
        // Hiển thị view Create để tạo một bản ghi QuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai mới
        // Truyền data từ các table khác hiển thị tại view Create (khóa ngoài)
        public async Task<IActionResult> Create()
        {
            try
            {

                ViewData["IdChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao"), "IdChuongTrinhDaoTao", "TenChuongTrinh");

                ViewData["IdHinhThucDaoTao"] = new SelectList(await ApiServices_.GetAll<DmHinhThucDaoTao>("/api/dm/HinhThucDaoTao"), "IdHinhThucDaoTao", "HinhThucDaoTao");
                //CHÚ Ý CÁI NÀY
                ViewData["IdLoaiQuyetDinh"] = new SelectList(await ApiServices_.GetAll<DmLoaiQuyetDinh>("/api/dm/LoaiQuyetDinh"), "IdLoaiQuyetDinh", "LoaiQuyetDinh");
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

        // Thêm một QuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai mới vào Database nếu IdQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai truyền vào không trùng với Id đã có trong Database
        // Trong trường hợp nhập trùng IdQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai sẽ bắt lỗi
        // Bắt lỗi ngoại lệ sao cho người nhập BẮT BUỘC phải nhập khác IdQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai đã có
        [HttpPost]
        [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
        public async Task<IActionResult> Create([Bind("IdQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai,IdChuongTrinhDaoTao,IdLoaiQuyetDinh,SoQuyetDinh,NgayBanHanhQuyetDinh,IdHinhThucDaoTao")] TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai tbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai)
        {
            try
            {
                // Nếu trùng IdQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai sẽ báo lỗi
                if (await TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoaiExists(tbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai.IdQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai)) ModelState.AddModelError("IdQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai", "ID này đã tồn tại!");

                check_null(tbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai);
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai>("/api/ctdt/QuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai", tbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai);
                    return RedirectToAction(nameof(Index));
                }

                ViewData["IdChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao"), "IdChuongTrinhDaoTao", "TenChuongTrinh");

                ViewData["IdHinhThucDaoTao"] = new SelectList(await ApiServices_.GetAll<DmHinhThucDaoTao>("/api/dm/HinhThucDaoTao"), "IdHinhThucDaoTao", "HinhThucDaoTao");
                //CHÚ Ý CÁI NÀY
                ViewData["IdLoaiQuyetDinh"] = new SelectList(await ApiServices_.GetAll<DmLoaiQuyetDinh>("/api/dm/LoaiQuyetDinh"), "IdLoaiQuyetDinh", "LoaiQuyetDinh");
                return View(tbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai);
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

                var tb = await ApiServices_.GetId<TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai>("/api/ctdt/QuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai", id ?? 0);
                if (tb == null)
                {
                    return NotFound();
                }
                ViewData["IdChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao"), "IdChuongTrinhDaoTao", "TenChuongTrinh");

                ViewData["IdHinhThucDaoTao"] = new SelectList(await ApiServices_.GetAll<DmHinhThucDaoTao>("/api/dm/HinhThucDaoTao"), "IdHinhThucDaoTao", "HinhThucDaoTao");
                //CHÚ Ý CÁI NÀY
                ViewData["IdLoaiQuyetDinh"] = new SelectList(await ApiServices_.GetAll<DmLoaiQuyetDinh>("/api/dm/LoaiQuyetDinh"), "IdLoaiQuyetDinh", "LoaiQuyetDinh");
                return View(tb);
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
        public async Task<IActionResult> Edit(int id, [Bind("IdQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai,IdChuongTrinhDaoTao,IdLoaiQuyetDinh,SoQuyetDinh,NgayBanHanhQuyetDinh,IdHinhThucDaoTao")] TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai tbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai)
        {
            try
            {
                if (id != tbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai.IdQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai)
                {
                    return NotFound();
                }

                check_null(tbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai);
                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai>("/api/ctdt/QuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai", id, tbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoaiExists(tbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai.IdQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai) == false)
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

                ViewData["IdChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao"), "IdChuongTrinhDaoTao", "TenChuongTrinh");

                ViewData["IdHinhThucDaoTao"] = new SelectList(await ApiServices_.GetAll<DmHinhThucDaoTao>("/api/dm/HinhThucDaoTao"), "IdHinhThucDaoTao", "HinhThucDaoTao");
                //CHÚ Ý CÁI NÀY
                ViewData["IdLoaiQuyetDinh"] = new SelectList(await ApiServices_.GetAll<DmLoaiQuyetDinh>("/api/dm/LoaiQuyetDinh"), "IdLoaiQuyetDinh", "LoaiQuyetDinh");
                return View(tbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: ChuongTrinhDaoTao/Delete
        // Xóa một QuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai khỏi Database
        // Lấy data QuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai từ Database, hiển thị Data tại view Delete
        // Hàm này để hiển thị thông tin cho người dùng trước khi xóa
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var tbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoais = await ApiServices_.GetAll<TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai>("/api/ctdt/QuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai");
                var tb = tbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoais.FirstOrDefault(m => m.IdQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai == id);
                if (tb == null)
                {
                    return NotFound();
                }

                return View(tb);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: IdQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai/Delete
        // Xóa QuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai khỏi Database sau khi nhấn xác nhận 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // Lệnh xác nhận xóa hẳn một QuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai
        {
            try
            {
                await ApiServices_.Delete<TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai>("/api/ctdt/QuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        private async Task<bool> TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoaiExists(int id)
        {
            var TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoais = await ApiServices_.GetAll<TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai>("/api/ctdt/QuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai");
            return TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoais.Any(e => e.IdQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai == id);
        }
        private void check_null(TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai tbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai)
        {
            if (tbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai.IdChuongTrinhDaoTao == null) ModelState.AddModelError("IdChuongTrinhDaoTao", "Vui lòng nhập vào ô trống!");
            if (tbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai.SoQuyetDinh == null) ModelState.AddModelError("SoQuyetDinh", "Vui lòng nhập vào ô trống!");
            if (tbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai.NgayBanHanhQuyetDinh == null) ModelState.AddModelError("NgayBanHanhQuyetDinh", "Không được bỏ trống!");
            if (tbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai.IdHinhThucDaoTao == null) ModelState.AddModelError("IdHinhThucDaoTao", "Không được bỏ trống!");


        }

        [HttpPost]
        public IActionResult Receive_Excel(string jsonExcel)
        {
            try
            {
                List<List<string>> dataList = JsonConvert.DeserializeObject<List<List<string>>>(jsonExcel);
                dataList.ForEach(s => {
                    TbChuongTrinhDaoTao new_ = new TbChuongTrinhDaoTao();
                    // new_.IdChuongTrinhDaoTao = Int32.Parse(s[0]);
                    // new_.MaChuongTrinhDaoTao = s[1];
                    // new_.IdNganhDaoTao = s[1];
                });
                string message = "Thành công";
                return Accepted(Json(new { msg = message }));
            }
            catch (Exception ex)
            {
                return BadRequest(Json(new { msg = ex.Message, }));
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetChartData(string type)
        {
            try
            {
                // Kiểm tra tham số 'type' có hợp lệ hay không
                if (string.IsNullOrEmpty(type) ||
                    !(type == "Số quyết định" ||
                    type == "Hình thức đào tạo" ||
                    type == "Loại quyết định"))
                {
                    return Json(new { error = "Invalid type parameter." });
                }

                // Lấy dữ liệu từ API
                var data = ApiServices_.GetAll<TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai>("/api/ctdt/QuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai");
                var dataList = await data;

                List<TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai> TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoais = await ApiServices_.GetAll<TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai>("/api/ctdt/QuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai");
                List<TbChuongTrinhDaoTao> tbChuongTrinhDaoTaos = await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao");
                List<DmHinhThucDaoTao> dmHinhThucDaoTaos = await ApiServices_.GetAll<DmHinhThucDaoTao>("/api/dm/HinhThucDaoTao");
                List<DmLoaiQuyetDinh> dmLoaiQuyetDinhs = await ApiServices_.GetAll<DmLoaiQuyetDinh>("/api/dm/LoaiQuyetDinh");

                // Gán navigation properties
                TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoais.ForEach(item =>
                {
                    item.IdChuongTrinhDaoTaoNavigation = tbChuongTrinhDaoTaos.FirstOrDefault(t => t.IdChuongTrinhDaoTao == item.IdChuongTrinhDaoTao);
                    item.IdHinhThucDaoTaoNavigation = dmHinhThucDaoTaos.FirstOrDefault(t => t.IdHinhThucDaoTao == item.IdHinhThucDaoTao);
                    item.IdLoaiQuyetDinhNavigation = dmLoaiQuyetDinhs.FirstOrDefault(t => t.IdLoaiQuyetDinh == item.IdLoaiQuyetDinh);
                });

                // Kiểm tra loại biểu đồ
                if (type == "Số quyết định")
                {
                    var resultFiltered = TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoais.Select(s => new
                    {
                        TenChuongTrinh = s.IdChuongTrinhDaoTaoNavigation?.TenChuongTrinh ?? "Không xác định",
                        Value = s.SoQuyetDinh ?? "Không xác định"
                    }).ToList();

                    return Json(resultFiltered);
                }
                else if (type == "Hình thức đào tạo")
                {
                    var resultFiltered = TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoais.Select(s => new
                    {
                        TenChuongTrinh = s.IdChuongTrinhDaoTaoNavigation?.TenChuongTrinh ?? "Không xác định",
                        Value = s.IdHinhThucDaoTao?? 0
                    }).ToList();

                    return Json(resultFiltered);
                }
                else if (type == "Loại quyết định")
                {
                    var resultFiltered = TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoais.Select(s => new
                    {
                        TenChuongTrinh = s.IdChuongTrinhDaoTaoNavigation?.TenChuongTrinh ?? "Không xác định",
                        Value = s.IdLoaiQuyetDinh??0 
                    }).ToList();

                    return Json(resultFiltered);
                }

                return Json(new { error = "Type not handled." });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (file != null && file.Length > 0)
            {
                // Đảm bảo thư mục Uploads tồn tại
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Tạo đường dẫn file và đảm bảo tên file hợp lệ
                var filePath = Path.Combine(uploadsFolder, Path.GetFileName(file.FileName));

                // Lưu file vào thư mục Uploads
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Đọc file Excel sau khi đã lưu
                try
                {
                    using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            bool isHeaderSkipped = false;
                            while (reader.Read())
                            {
                                // Bỏ qua header
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }

                                // Đọc từng dòng dữ liệu
                                TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai tb = new TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai
                                {
                                    IdQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai = reader.GetValue(1) != null ? Convert.ToInt32(reader.GetValue(1).ToString()) : 0,
                                    IdChuongTrinhDaoTao = reader.GetValue(2) != null ? Convert.ToInt32(reader.GetValue(2).ToString()) : 0,
                                    IdLoaiQuyetDinh = reader.GetValue(3) != null ? Convert.ToInt32(reader.GetValue(3).ToString()) : 0,
                                    SoQuyetDinh = reader.GetValue(4) != null ? reader.GetValue(4).ToString() : null,
                                    NgayBanHanhQuyetDinh = reader.GetValue(5) != null ? DateOnly.FromDateTime(DateTime.Parse(reader.GetValue(5).ToString())) : (DateOnly?)null,
                                    IdHinhThucDaoTao = reader.GetValue(6) != null ? Convert.ToInt32(reader.GetValue(6).ToString()) : 0
                                    
                                };


                                _dbcontext.TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoais.Add(tb);
                                await ApiServices_.Create<TbQuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai>("/api/ctdt/QuyetDinhCapPhepChuongTrinhDungChoChuongTrinhNuocNgoai", tb);

                            }
                            await _dbcontext.SaveChangesAsync();

                        }
                    }
                    TempData["Success"] = "File đã được import thành công!";
                }
                catch (Exception ex)
                {
                    var errorMessage = ex.InnerException?.Message ?? ex.Message;
                    Console.WriteLine(errorMessage);
                    TempData["Error"] = "Lỗi khi lưu dữ liệu: " + errorMessage;
                }
            }
            else
            {
                TempData["Error"] = "Vui lòng chọn file để upload.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
