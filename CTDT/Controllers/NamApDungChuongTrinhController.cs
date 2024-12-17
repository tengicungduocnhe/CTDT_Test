
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
    public class NamApDungChuongTrinhController : Controller
    {
        private readonly ApiServices ApiServices_;
        private readonly DbHemisC500Context _dbcontext;
        public NamApDungChuongTrinhController(ApiServices services, DbHemisC500Context dbcontext)
        {
            ApiServices_ = services;
            _dbcontext = dbcontext;
        }
        public IActionResult chartjs()
        {
            return View(); // Nó sẽ trả về view chartjs.cshtml
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
        public async Task<JsonResult> GetChartData(string type)
        {
            try
            {
                if (string.IsNullOrEmpty(type) ||
                    !(type == "Số tín chỉ tối thiểu để tốt nghiệp" ||
                    type == "Chỉ tiêu tuyển sinh hằng năm" ||
                    type == "Tổng học phí toàn khoá"))
                {
                    return Json(new { error = "Invalid type parameter." });
                }

                // Lấy dữ liệu từ API
                var data = ApiServices_.GetAll<TbNamApDungChuongTrinh>("/api/ctdt/GiaHanChuongTrinhDaoTao");
                var dataList = await data;
                List<TbChuongTrinhDaoTao> chuongTrinhDaoTaos = await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao");
                List<TbNamApDungChuongTrinh> namApDungList = await ApiServices_.GetAll<TbNamApDungChuongTrinh>("/api/ctdt/NamApDungChuongTrinh");

                // Gán navigation property
                namApDungList.ForEach(item =>
                {
                    item.IdChuongTrinhDaoTaoNavigation = chuongTrinhDaoTaos.FirstOrDefault(t => t.IdChuongTrinhDaoTao == item.IdChuongTrinhDaoTao);
                });
                var resultFiltered = namApDungList.Select(s => new
                {
                    TenChuongTrinh = s.IdChuongTrinhDaoTaoNavigation?.TenChuongTrinh ?? "Không xác định",  // Trả về "Không có tên chương trình" nếu null

                    Value = type == "Số tín chỉ tối thiểu để tốt nghiệp" ? (s.SoTinChiToiThieuDeTotNghiep ?? 0) :
                            type == "Chỉ tiêu tuyển sinh hằng năm" ? (s.ChiTieuTuyenSinhHangNam ?? 0) :
                            type == "Tổng học phí toàn khoá" ? (s.TongHocPhiToanKhoa ?? 0) : 0
                }).ToList();

                return Json(resultFiltered);
            }

            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
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
                                TbNamApDungChuongTrinh tb = new TbNamApDungChuongTrinh
                                {
                                    IdNamApDungChuongTrinh = reader.GetValue(1) != null ? Convert.ToInt32(reader.GetValue(1).ToString()) : 0,
                                    IdChuongTrinhDaoTao = reader.GetValue(2) != null ? Convert.ToInt32(reader.GetValue(2).ToString()) : 0,
                                    
                                    
                                    SoTinChiToiThieuDeTotNghiep = reader.GetValue(3) != null ? Convert.ToInt32(reader.GetValue(3).ToString()) : 0,
                                    TongHocPhiToanKhoa = reader.GetValue(4) != null ? Convert.ToInt32(reader.GetValue(4).ToString()) : 0,
                                    NamApDung = reader.GetValue(5) != null ? DateOnly.FromDateTime(DateTime.Parse(reader.GetValue(5).ToString())) : (DateOnly?)null,
                                    ChiTieuTuyenSinhHangNam = reader.GetValue(6) != null ? Convert.ToInt32(reader.GetValue(6).ToString()) : 0

                                };


                                _dbcontext.TbNamApDungChuongTrinhs.Add(tb);
                                await ApiServices_.Create<TbNamApDungChuongTrinh>("/api/nadct/NamApDungChuongTrinh", tb);

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



