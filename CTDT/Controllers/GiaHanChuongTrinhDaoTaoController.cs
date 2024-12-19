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
    public class GiaHanChuongTrinhDaoTaoController : Controller
    {
        private readonly ApiServices ApiServices_;
        private readonly DbHemisC500Context _dbcontext;
        public GiaHanChuongTrinhDaoTaoController(ApiServices services, DbHemisC500Context dbcontext)
        {
            ApiServices_ = services;
            _dbcontext = dbcontext;
        }

        public IActionResult chartjs()
        {
            return View(); // Nó sẽ trả về view chartjs.cshtml
        }

        private async Task<List<TbGiaHanChuongTrinhDaoTao>> TbGiaHanChuongTrinhDaos()
        {
            List<TbGiaHanChuongTrinhDaoTao> tbGiaHanChuongTrinhDaoTaos = await ApiServices_.GetAll<TbGiaHanChuongTrinhDaoTao>("/api/ctdt/GiaHanChuongTrinhDaoTao");
            List<TbChuongTrinhDaoTao> tbChuongTrinhDaoTaos = await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao");
            tbGiaHanChuongTrinhDaoTaos.ForEach(item => {
                item.IdChuongTrinhDaoTaoNavigation = tbChuongTrinhDaoTaos.FirstOrDefault(t => t.IdChuongTrinhDaoTao == item.IdChuongTrinhDaoTao);

            });
            return tbGiaHanChuongTrinhDaoTaos;
        }

        public async Task<IActionResult> Index(string Id, string SapXep)
        {
            try
            {

                List<TbGiaHanChuongTrinhDaoTao> tbGiaHanChuongTrinhDaoTaos = await TbGiaHanChuongTrinhDaos();
                var danhSach = tbGiaHanChuongTrinhDaoTaos.Where(item => string.IsNullOrEmpty(Id) || item.IdGiaHanChuongTrinhDaoTao.ToString() == Id) //  tìm kiếm theo Id GHCTDT
                .ToList();

                var sapXepDanhSach = danhSach; // sắp xếp
                if (SapXep == "SapXep")
                {
                    sapXepDanhSach = danhSach.OrderBy(x => x.NgayBanHanhVanBanGiaHan).ToList();// sắp xếp ngày ban hành Vb GIA HẠN
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
                List<TbGiaHanChuongTrinhDaoTao> tbGiaHanChuongTrinhDaoTaos = await TbGiaHanChuongTrinhDaos();

                var tb = tbGiaHanChuongTrinhDaoTaos.FirstOrDefault(m => m.IdGiaHanChuongTrinhDaoTao == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tb == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết GHCTDT thành công
                return View(tb);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: ChuongTrinhDaoTao/Create
        // Hiển thị view Create để tạo một bản ghi GHCTDT mới
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

        // Thêm một GHCTDT mới vào Database nếu IdGiaHanChuongTrinhDaoTao truyền vào không trùng với Id đã có trong Database
        // Trong trường hợp nhập trùng IdChuongTrinhDaoTao sẽ bắt lỗi
        // Bắt lỗi ngoại lệ sao cho người nhập BẮT BUỘC phải nhập khác IdGiaHanChuongTrinhDaoTao đã có
        [HttpPost]
        [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
        public async Task<IActionResult> Create([Bind("IdGiaHanChuongTrinhDaoTao,IdChuongTrinhDaoTao,SoQuyetDinhGiaHan,NgayBanHanhVanBanGiaHan,GiaHanLanThu")] TbGiaHanChuongTrinhDaoTao tbGiaHanChuongTrinhDaoTao)
        {
            try
            {
                check_null(tbGiaHanChuongTrinhDaoTao);
                // Nếu trùng IdGiaHanChuongTrinhDaoTao sẽ báo lỗi
                if (await TbGiaHanChuongTrinhDaoTaoExists(tbGiaHanChuongTrinhDaoTao.IdGiaHanChuongTrinhDaoTao)) ModelState.AddModelError("IdGiaHanChuongTrinhDaoTao", "ID này đã tồn tại!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbGiaHanChuongTrinhDaoTao>("/api/ctdt/GiaHanChuongTrinhDaoTao", tbGiaHanChuongTrinhDaoTao);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao"), "IdChuongTrinhDaoTao", "TenChuongTrinh");

                return View(tbGiaHanChuongTrinhDaoTao);
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

                var TbGiaHanChuongTrinhDaoTao = await ApiServices_.GetId<TbGiaHanChuongTrinhDaoTao>("/api/ctdt/GiaHanChuongTrinhDaoTao", id ?? 0);
                if (TbGiaHanChuongTrinhDaoTao == null)
                {
                    return NotFound();
                }
                ViewData["IdChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao"), "IdChuongTrinhDaoTao", "TenChuongTrinh");
                return View(TbGiaHanChuongTrinhDaoTao);
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
        public async Task<IActionResult> Edit(int id, [Bind("IdGiaHanChuongTrinhDaoTao,IdChuongTrinhDaoTao,SoQuyetDinhGiaHan,NgayBanHanhVanBanGiaHan,GiaHanLanThu")] TbGiaHanChuongTrinhDaoTao tbGiaHanChuongTrinhDaoTao)
        {
            try
            {
                if (id != tbGiaHanChuongTrinhDaoTao.IdGiaHanChuongTrinhDaoTao)
                {
                    return NotFound();
                }
                check_null(tbGiaHanChuongTrinhDaoTao);
                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbGiaHanChuongTrinhDaoTao>("/api/ctdt/GiaHanChuongTrinhDaoTao", id, tbGiaHanChuongTrinhDaoTao);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbGiaHanChuongTrinhDaoTaoExists(tbGiaHanChuongTrinhDaoTao.IdGiaHanChuongTrinhDaoTao) == false)
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
                return View(tbGiaHanChuongTrinhDaoTao);
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
                var tbGiaHanChuongTrinhDaoTaos = await ApiServices_.GetAll<TbGiaHanChuongTrinhDaoTao>("/api/ctdt/GiaHanChuongTrinhDaoTao");
                var tbGiaHanChuongTrinhDaoTao = tbGiaHanChuongTrinhDaoTaos.FirstOrDefault(m => m.IdGiaHanChuongTrinhDaoTao == id);
                if (tbGiaHanChuongTrinhDaoTao == null)
                {
                    return NotFound();
                }

                return View(tbGiaHanChuongTrinhDaoTao);
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
                await ApiServices_.Delete<TbGiaHanChuongTrinhDaoTao>("/api/ctdt/GiaHanChuongTrinhDaoTao", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        // hàm ktra xem có tồn tại khong
        private async Task<bool> TbGiaHanChuongTrinhDaoTaoExists(int id)
        {
            var TbGiaHanChuongTrinhDaoTaos = await ApiServices_.GetAll<TbGiaHanChuongTrinhDaoTao>("/api/ctdt/GiaHanChuongTrinhDaoTao");
            return TbGiaHanChuongTrinhDaoTaos.Any(e => e.IdGiaHanChuongTrinhDaoTao == id);
        }
        ///hàm check null
        private void check_null(TbGiaHanChuongTrinhDaoTao tbGiaHanChuongTrinhDaoTao)
        {
            if (tbGiaHanChuongTrinhDaoTao.IdChuongTrinhDaoTao == null) ModelState.AddModelError("IdChuongTrinhDaoTao", "Vui lòng nhập vào ô trống!");
            if (tbGiaHanChuongTrinhDaoTao.SoQuyetDinhGiaHan == null) ModelState.AddModelError("SoQuyetDinhGiaHan", "Vui lòng nhập vào ô trống!");
            if (tbGiaHanChuongTrinhDaoTao.NgayBanHanhVanBanGiaHan == null) ModelState.AddModelError("NgayBanHanhVanBanGiaHan", "Không được bỏ trống!");
            if (tbGiaHanChuongTrinhDaoTao.GiaHanLanThu == null) ModelState.AddModelError("GiaHanLanThu", "Không được bỏ trống!");


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
                    !(type == "Số quyết định gia hạn" ||
                        type == "Gia hạn lần thứ"))
                {
                    return Json(new { error = "Invalid type parameter." });
                }

                // Lấy dữ liệu từ API
                var data = ApiServices_.GetAll<TbGiaHanChuongTrinhDaoTao>("/api/ctdt/GiaHanChuongTrinhDaoTao");
                var dataList = await data;
                List<TbChuongTrinhDaoTao> chuongTrinhDaoTaos = await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao");
                List<TbGiaHanChuongTrinhDaoTao> giaHanList = await ApiServices_.GetAll<TbGiaHanChuongTrinhDaoTao>("/api/ctdt/GiaHanChuongTrinhDaoTao");

                // Gán navigation property
                giaHanList.ForEach(item =>
                {
                    item.IdChuongTrinhDaoTaoNavigation = chuongTrinhDaoTaos.FirstOrDefault(t => t.IdChuongTrinhDaoTao == item.IdChuongTrinhDaoTao);
                });
                // Kiểm tra loại biểu đồ là "Số quyết định gia hạn"
                if (type == "Số quyết định gia hạn")
                {
                    var resultFiltered = dataList
                        .GroupBy(s => s.SoQuyetDinhGiaHan ?? "Không xác định")
                        .Select(g => new
                        {
                            TenChuongTrinh = g.Key,
                            Value = g.Count()
                        })
                        .ToList();

                    return Json(resultFiltered);
                }
                else if (type == "Gia hạn lần thứ")
                {
                    var resultFiltered = giaHanList.Select(s => new
                    {
                        TenChuongTrinh = s.IdChuongTrinhDaoTaoNavigation?.TenChuongTrinh ?? "Không xác định",
                        Value = s.GiaHanLanThu ?? 0
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
                                TbGiaHanChuongTrinhDaoTao tb = new TbGiaHanChuongTrinhDaoTao
                                {
                                    IdGiaHanChuongTrinhDaoTao = reader.GetValue(1) != null ? Convert.ToInt32(reader.GetValue(1).ToString()) : 0,
                                    IdChuongTrinhDaoTao = reader.GetValue(2) != null ? Convert.ToInt32(reader.GetValue(2).ToString()) : 0,
                                    
                                    SoQuyetDinhGiaHan = reader.GetValue(3) != null ? reader.GetValue(3).ToString() : null,

                                    NgayBanHanhVanBanGiaHan = reader.GetValue(4) != null ? DateOnly.FromDateTime(DateTime.Parse(reader.GetValue(4).ToString())) : (DateOnly?)null,
                                    GiaHanLanThu = reader.GetValue(5) != null ? Convert.ToInt32(reader.GetValue(5).ToString()) : 0
                                    
                                };


                                _dbcontext.TbGiaHanChuongTrinhDaoTaos.Add(tb);
                                await ApiServices_.Create<TbGiaHanChuongTrinhDaoTao>("/api/ctdt/GiaHanChuongTrinhDaoTao", tb);

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




   
