
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
//using CTDT.Models.DM;
namespace CTDT.Controllers
{
    [Authorize]
    public class NamApDungChuongTrinhController : Controller
    {
        private readonly ApiServices ApiServices_;
        // Lấy từ HemisContext 
        public NamApDungChuongTrinhController(ApiServices services)
        {
            ApiServices_ = services;
        }

        // GET: ChuongTrinhDaoTao
        // Lấy danh sách NADCT từ database, trả về view Index.
        private async Task<List<TbNamApDungChuongTrinh>> TbNamApDungChuongTrinh()
        {
            List<TbNamApDungChuongTrinh> tbNamApDungChuongTrinhs = await ApiServices_.GetAll<TbNamApDungChuongTrinh>("/api/ctdt/NamApDungChuongTrinh");
            List<TbChuongTrinhDaoTao> tbChuongTrinhDaoTaos = await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao");
            tbNamApDungChuongTrinhs.ForEach(item => {
                item.IdChuongTrinhDaoTaoNavigation = tbChuongTrinhDaoTaos.FirstOrDefault(t => t.IdChuongTrinhDaoTao == item.IdChuongTrinhDaoTao);

            });
            return tbNamApDungChuongTrinhs;
        }
        public async Task<IActionResult> Index(string Id, string SapXep)
        {
            try
            {

                List<TbNamApDungChuongTrinh> tbNamApDungChuongTrinhs = await TbNamApDungChuongTrinh();
                var danhSach = tbNamApDungChuongTrinhs.Where(item => string.IsNullOrEmpty(Id) || item.IdNamApDungChuongTrinh.ToString() == Id) //  tìm kiếm theo Id NADCT
                .ToList();

                var sapXepDanhSach = danhSach; // sắp xếp
                if (SapXep == "SapXep")
                {
                    sapXepDanhSach = danhSach.OrderBy(x => x.NamApDung).ToList();// Năm áp dụng
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

                List<TbNamApDungChuongTrinh> tbNamApDungChuongTrinhs = await TbNamApDungChuongTrinh();
                var tb = tbNamApDungChuongTrinhs.FirstOrDefault(m => m.IdNamApDungChuongTrinh == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tb == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết NADCT thành công
                return View(tb);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: ChuongTrinhDaoTao/Create
        // Hiển thị view Create để tạo một bản ghi NADCT mới
        // Truyền data từ các table khác hiển thị tại view Create (khóa ngoài)
        public async Task<IActionResult> Create()
        {
            try
            {
              ViewData["IdChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao"), "IdChuongTrinhDaoTao", "TenChuongTrinh");
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

        // Thêm một NADCT mới vào Database nếu IdChuongTrinhDaoTao truyền vào không trùng với Id đã có trong Database
        // Trong trường hợp nhập trùng IdChuongTrinhDaoTao sẽ bắt lỗi
        // Bắt lỗi ngoại lệ sao cho người nhập BẮT BUỘC phải nhập khác IdChuongTrinhDaoTao đã có
        [HttpPost]
        [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
        public async Task<IActionResult> Create([Bind("IdNamApDungChuongTrinh,IdChuongTrinhDaoTao,SoTinChiToiThieuDeTotNghiep,TongHocPhiToanKhoa,NamApDung,ChiTieuTuyenSinhHangNam")] TbNamApDungChuongTrinh tbNamApDungChuongTrinh)
        {
            try
            {
                check_null(tbNamApDungChuongTrinh);
                // Nếu trùng IdNamApDungChuongTrinh sẽ báo lỗi
                if (await TbNamApDungChuongTrinhExists(tbNamApDungChuongTrinh.IdNamApDungChuongTrinh)) ModelState.AddModelError("IdNamApDungChuongTrinh", "ID này đã tồn tại!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbNamApDungChuongTrinh>("/api/ctdt/NamApDungChuongTrinh", tbNamApDungChuongTrinh);
                    return RedirectToAction(nameof(Index));
                }
              ViewData["IdChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao"), "IdChuongTrinhDaoTao", "TenChuongTrinh");

                return View(tbNamApDungChuongTrinh);
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

                var TbNamApDungChuongTrinh = await ApiServices_.GetId<TbNamApDungChuongTrinh>("/api/ctdt/NamApDungChuongTrinh", id ?? 0);
                if (TbNamApDungChuongTrinh == null)
                {
                    return NotFound();
                }
              ViewData["IdChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao"), "IdChuongTrinhDaoTao", "TenChuongTrinh");
                return View(TbNamApDungChuongTrinh);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: ChuongTrinhDaoTao/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        // Lưu data mới (ghi đè) vào các trường Data đã có thuộc IdNamApDungChuongTrinh cần chỉnh sửa
        // Nó chỉ cập nhật khi ModelState hợp lệ
        // Nếu không hợp lệ sẽ báo lỗi, vì vậy cần có bắt lỗi.

        [HttpPost]
        [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
        public async Task<IActionResult> Edit(int id, [Bind("IdNamApDungChuongTrinh,IdChuongTrinhDaoTao,SoTinChiToiThieuDeTotNghiep,TongHocPhiToanKhoa,NamApDung,ChiTieuTuyenSinhHangNam")] TbNamApDungChuongTrinh tbNamApDungChuongTrinh)
        {
            try
            {
                if (id != tbNamApDungChuongTrinh.IdNamApDungChuongTrinh)
                {
                    return NotFound();
                }
                check_null(tbNamApDungChuongTrinh);
                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbNamApDungChuongTrinh>("/api/ctdt/NamApDungChuongTrinh", id, tbNamApDungChuongTrinh);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbNamApDungChuongTrinhExists(tbNamApDungChuongTrinh.IdNamApDungChuongTrinh) == false)
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
                return View(tbNamApDungChuongTrinh);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: ChuongTrinhDaoTao/Delete
        // Xóa một NADCT khỏi Database
        // Lấy data NADCT từ Database, hiển thị Data tại view Delete
        // Hàm này để hiển thị thông tin cho người dùng trước khi xóa
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var tbNamApDungChuongTrinhs = await ApiServices_.GetAll<TbNamApDungChuongTrinh>("/api/ctdt/NamApDungChuongTrinh");
                var tbNamApDungChuongTrinh = tbNamApDungChuongTrinhs.FirstOrDefault(m => m.IdNamApDungChuongTrinh == id);
                if (tbNamApDungChuongTrinh == null)
                {
                    return NotFound();
                }

                return View(tbNamApDungChuongTrinh);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: ChuongTrinhDaoTao/Delete
        // Xóa NADCT khỏi Database sau khi nhấn xác nhận 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // Lệnh xác nhận xóa hẳn một NADCT
        {
            try
            {
                await ApiServices_.Delete<TbNamApDungChuongTrinh>("/api/ctdt/NamApDungChuongTrinh", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        // hàm check xem tồn tại không
        private async Task<bool> TbNamApDungChuongTrinhExists(int id)
        {
            var TbNamApDungChuongTrinhs = await ApiServices_.GetAll<TbNamApDungChuongTrinh>("/api/ctdt/NamApDungChuongTrinh");
            return TbNamApDungChuongTrinhs.Any(e => e.IdNamApDungChuongTrinh == id);
        }
        private void check_null(TbNamApDungChuongTrinh tbNamApDungChuongTrinh)
        {
            if (tbNamApDungChuongTrinh.IdChuongTrinhDaoTao == null) ModelState.AddModelError("IdChuongTrinhDaoTao", "Vui lòng nhập vào ô trống!");
            if (tbNamApDungChuongTrinh.SoTinChiToiThieuDeTotNghiep == null) ModelState.AddModelError("SoTinChiToiThieuDeTotNghiep", "Vui lòng nhập vào ô trống!");
            if (tbNamApDungChuongTrinh.TongHocPhiToanKhoa == null) ModelState.AddModelError("TongHocPhiToanKhoa", "Không được bỏ trống!");
            if (tbNamApDungChuongTrinh.NamApDung == null) ModelState.AddModelError("NamApDung", "Không được bỏ trống!");
            if (tbNamApDungChuongTrinh.ChiTieuTuyenSinhHangNam == null) ModelState.AddModelError("ChiTieuTuyenSinhHangNam", "Không được bỏ trống!");

        }
        [HttpPost]
        public IActionResult Receive_Excel(string jsonExcel) {
            try {
                List<List<string>> dataList = JsonConvert.DeserializeObject<List<List<string>>>(jsonExcel);
                dataList.ForEach(s => {
                    TbChuongTrinhDaoTao new_ = new TbChuongTrinhDaoTao();
                    // new_.IdChuongTrinhDaoTao = Int32.Parse(s[0]);
                    // new_.MaChuongTrinhDaoTao = s[1];
                    // new_.IdNganhDaoTao = s[1];
                });
                string message = "Thành công";
                return Accepted(Json(new {msg = message}));
            } catch (Exception ex) {
                return BadRequest(Json(new { msg = ex.Message,}));
            }
        }
    }
}
