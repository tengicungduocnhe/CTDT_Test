using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CTDT.Models;
using CTDT.API;
//using C500Hemis.Models.DM;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using ExcelDataReader;
using System.Text;


namespace C500Hemis.Controllers.CTDT
{
    [Authorize]
    public class ThongTinKiemDinhCuaChuongTrinhController : Controller
    {
        private readonly ApiServices ApiServices_;
        private readonly DbHemisC500Context _dbcontext;
        public ThongTinKiemDinhCuaChuongTrinhController(ApiServices services, DbHemisC500Context dbcontext)
        {
            ApiServices_ = services;
            _dbcontext = dbcontext;
        }

        public IActionResult chartjs()
        {
            return View(); // Nó sẽ trả về view chartjs.cshtml
        }

        private async Task<List<TbThongTinKiemDinhCuaChuongTrinh>> TbThongTinKiemDinhCuaChuongTrinhs()
        {
            List<TbThongTinKiemDinhCuaChuongTrinh> TbThongTinKiemDinhCuaChuongTrinhs = await ApiServices_.GetAll<TbThongTinKiemDinhCuaChuongTrinh>("/api/ctdt/ThongTinKiemDinhCuaChuongTrinh");
            List<TbChuongTrinhDaoTao> tbChuongTrinhDaoTaos = await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao");
            List<DmKetQuaKiemDinh> dmKetQuaKiemDinhs = await ApiServices_.GetAll<DmKetQuaKiemDinh>("/api/dm/KetQuaKiemDinh");
            List<DmToChucKiemDinh> dmToChucKiemDinhs = await ApiServices_.GetAll<DmToChucKiemDinh>("/api/dm/ToChucKiemDinh");

            TbThongTinKiemDinhCuaChuongTrinhs.ForEach(item =>
            {
                item.IdChuongTrinhDaoTaoNavigation = tbChuongTrinhDaoTaos.FirstOrDefault(t => t.IdChuongTrinhDaoTao == item.IdChuongTrinhDaoTao);
                item.IdKetQuaKiemDinhNavigation = dmKetQuaKiemDinhs.FirstOrDefault(t => t.IdKetQuaKiemDinh == item.IdKetQuaKiemDinh);
                item.IdToChucKiemDinhNavigation = dmToChucKiemDinhs.FirstOrDefault(t => t.IdToChucKiemDinh == item.IdToChucKiemDinh);
            });

            return TbThongTinKiemDinhCuaChuongTrinhs;
        }

        //private async Task Selectlist(TbThongTinKiemDinhCuaChuongTrinh? tbThongTinKiemDinhCuaChuongTrinh = null)
        //{
        //    if (tbThongTinKiemDinhCuaChuongTrinh == null)
        //    {
        //        ViewData["IdChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao"), "IdChuongTrinhDaoTao", "TenChuongTrinhDaoTao");
        //        ViewData["IdKetQuaKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmKetQuaKiemDinh>("/api/dm/KetQuaKiemDinh"), "IdKetQuaKiemDinh", "KetQuaKiemDinh");
        //        ViewData["IdToChucKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmToChucKiemDinh>("/api/dm/ToChucKiemDinh"), "IdToChucKiemDinh", "ToChucKiemDinh");
        //    }
        //    else
        //    {
        //        ViewData["IdChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao"), "IdChuongTrinhDaoTao", "TenChuongTrinhDaoTao", tbThongTinKiemDinhCuaChuongTrinh.IdChuongTrinhDaoTao);
        //        ViewData["IdKetQuaKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmKetQuaKiemDinh>("/api/dm/KetQuaKiemDinh"), "IdKetQuaKiemDinh", "KetQuaKiemDinh", tbThongTinKiemDinhCuaChuongTrinh.IdKetQuaKiemDinh);
        //        ViewData["IdToChucKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmToChucKiemDinh>("/api/dm/ToChucKiemDinh"), "IdToChucKiemDinh", "ToChucKiemDinh", tbThongTinKiemDinhCuaChuongTrinh.IdToChucKiemDinh);
        //    }
        //}
        // GET: ThongTinKiemDinhCuaChuongTrinh
        //public async Task<IActionResult> Index()
        //{
        //    List<TbThongTinKiemDinhCuaChuongTrinh> tbThongTinKiemDinhCuaChuongTrinhs = await TbThongTinKiemDinhCuaChuongTrinhs();
        //    return View(tbThongTinKiemDinhCuaChuongTrinhs);
        //}
        public async Task<IActionResult> Index(string Id, string SapXep)
        {
            try
            {
                List<TbThongTinKiemDinhCuaChuongTrinh> tbThongTinKiemDinhCuaChuongTrinhs = await TbThongTinKiemDinhCuaChuongTrinhs();

                // Kiểm tra null cho điều kiện tìm kiếm
                var danhSach = tbThongTinKiemDinhCuaChuongTrinhs
                    .Where(item => string.IsNullOrEmpty(Id) || item.IdThongTinKiemDinhCuaChuongTrinh.ToString() == Id)
                    .ToList();

                // Sắp xếp danh sách nếu yêu cầu
                var sapXepDanhSach = danhSach;
                if (SapXep == "SapXep")
                {
                    sapXepDanhSach = danhSach.OrderBy(x => x.NgayCapChungNhanKiemDinh).ToList();
                }

                ViewBag.KqTimKiem = danhSach;
                ViewBag.KqSapXep = sapXepDanhSach;

                return View(sapXepDanhSach);
            }
            catch (Exception ex)
            {
                // Bắt lỗi ngoại lệ
                return BadRequest();
            }
        }

        // GET: ThongTinKiemDinhCuaChuongTrinh/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbThongTinKiemDinhCuaChuongTrinhs = await TbThongTinKiemDinhCuaChuongTrinhs();
            var tbThongTinKiemDinhCuaChuongTrinh = tbThongTinKiemDinhCuaChuongTrinhs.FirstOrDefault(m => m.IdThongTinKiemDinhCuaChuongTrinh == id);
            if (tbThongTinKiemDinhCuaChuongTrinh == null)
            {
                return NotFound();
            }

            return View(tbThongTinKiemDinhCuaChuongTrinh);
        }

        //  GET: ThongTinKiemDinhCuaChuongTrinh/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao"), "IdChuongTrinhDaoTao", "TenChuongTrinh");
                ViewData["IdKetQuaKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmKetQuaKiemDinh>("/api/dm/KetQuaKiemDinh"), "IdKetQuaKiemDinh", "KetQuaKiemDinh");
                ViewData["IdToChucKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmToChucKiemDinh>("/api/dm/ToChucKiemDinh"), "IdToChucKiemDinh", "ToChucKiemDinh");
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdThongTinKiemDinhCuaChuongTrinh,IdChuongTrinhDaoTao,IdToChucKiemDinh,IdKetQuaKiemDinh,SoQuyetDinh,NgayCapChungNhanKiemDinh,ThoiHanKiemDinh")] TbThongTinKiemDinhCuaChuongTrinh tbThongTinKiemDinhCuaChuongTrinh)
        {
            try
            {
                check_null(tbThongTinKiemDinhCuaChuongTrinh);
                // Nếu trùng IdGiaHanChuongTrinhDaoTao sẽ báo lỗi
                if (await TbThongTinKiemDinhCuaChuongTrinhExists(tbThongTinKiemDinhCuaChuongTrinh.IdThongTinKiemDinhCuaChuongTrinh)) ModelState.AddModelError("IdThongTinKiemDinhCuaChuongTrinh", "ID này đã tồn tại!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbThongTinKiemDinhCuaChuongTrinh>("/api/ctdt/ThongTinKiemDinhCuaChuongTrinh", tbThongTinKiemDinhCuaChuongTrinh);
                    return RedirectToAction(nameof(Index));
                }

                ViewData["IdChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao"), "IdChuongTrinhDaoTao", "TenChuongTrinh");
                ViewData["IdKetQuaKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmKetQuaKiemDinh>("/api/dm/KetQuaKiemDinh"), "IdKetQuaKiemDinh", "KetQuaKiemDinh");
                ViewData["IdToChucKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmToChucKiemDinh>("/api/dm/ToChucKiemDinh"), "IdToChucKiemDinh", "ToChucKiemDinh");
                return View(tbThongTinKiemDinhCuaChuongTrinh);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbThongTinKiemDinhCuaChuongTrinh = await ApiServices_.GetId<TbThongTinKiemDinhCuaChuongTrinh>("/api/ctdt/ThongTinKiemDinhCuaChuongTrinh", id ?? 0);


            if (tbThongTinKiemDinhCuaChuongTrinh == null)
            {
                return NotFound();
            }


            ViewData["IdChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao"), "IdChuongTrinhDaoTao", "TenChuongTrinh");
            ViewData["IdKetQuaKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmKetQuaKiemDinh>("/api/dm/KetQuaKiemDinh"), "IdKetQuaKiemDinh", "KetQuaKiemDinh");
            ViewData["IdToChucKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmToChucKiemDinh>("/api/dm/ToChucKiemDinh"), "IdToChucKiemDinh", "ToChucKiemDinh");

            //   await LoadDropdownData();
            return View(tbThongTinKiemDinhCuaChuongTrinh);
        }

        // POST: ThongTinKiemDinhCuaChuongTrinh/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdThongTinKiemDinhCuaChuongTrinh,IdChuongTrinhDaoTao,IdToChucKiemDinh,IdKetQuaKiemDinh,SoQuyetDinh,NgayCapChungNhanKiemDinh,ThoiHanKiemDinh")] TbThongTinKiemDinhCuaChuongTrinh tbThongTinKiemDinhCuaChuongTrinh)
        {
            if (id != tbThongTinKiemDinhCuaChuongTrinh.IdThongTinKiemDinhCuaChuongTrinh)
            {
                return NotFound();
            }

            check_null(tbThongTinKiemDinhCuaChuongTrinh);

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbThongTinKiemDinhCuaChuongTrinh>("/api/ctdt/ThongTinKiemDinhCuaChuongTrinh", id, tbThongTinKiemDinhCuaChuongTrinh);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await TbThongTinKiemDinhCuaChuongTrinhExists(tbThongTinKiemDinhCuaChuongTrinh.IdThongTinKiemDinhCuaChuongTrinh)))
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
            ViewData["IdKetQuaKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmKetQuaKiemDinh>("/api/dm/KetQuaKiemDinh"), "IdKetQuaKiemDinh", "KetQuaKiemDinh");
            ViewData["IdToChucKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmToChucKiemDinh>("/api/dm/ToChucKiemDinh"), "IdToChucKiemDinh", "ToChucKiemDinh");

            //await LoadDropdownData();
            return View(tbThongTinKiemDinhCuaChuongTrinh);
        }

        // GET: ThongTinKiemDinhCuaChuongTrinh/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbThongTinKiemDinhCuaChuongTrinhs = await TbThongTinKiemDinhCuaChuongTrinhs();
            var tbThongTinKiemDinhCuaChuongTrinh = tbThongTinKiemDinhCuaChuongTrinhs.FirstOrDefault(m => m.IdThongTinKiemDinhCuaChuongTrinh == id);
            if (tbThongTinKiemDinhCuaChuongTrinh == null)
            {
                return NotFound();
            }

            return View(tbThongTinKiemDinhCuaChuongTrinh);
        }

        // POST: ThongTinKiemDinhCuaChuongTrinh/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await ApiServices_.Delete<TbThongTinKiemDinhCuaChuongTrinh>("/api/ctdt/ThongTinKiemDinhCuaChuongTrinh", id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TbThongTinKiemDinhCuaChuongTrinhExists(int id)
        {
            var tbThongTinKiemDinhCuaChuongTrinh = await ApiServices_.GetId<TbThongTinKiemDinhCuaChuongTrinh>("/api/ctdt/ThongTinKiemDinhCuaChuongTrinh", id);
            if (tbThongTinKiemDinhCuaChuongTrinh == null)
            {
                return false;
            }
            return true;
        }

        private void check_null(TbThongTinKiemDinhCuaChuongTrinh tbThongTinKiemDinhCuaChuongTrinh)
        {
            if (tbThongTinKiemDinhCuaChuongTrinh.IdChuongTrinhDaoTao == null) ModelState.AddModelError("IdChuongTrinhDaoTao", "Vui lòng nhập vào ô trống!");
            if (tbThongTinKiemDinhCuaChuongTrinh.IdToChucKiemDinh == null) ModelState.AddModelError("IdToChucKiemDinh", "Vui lòng nhập vào ô trống!");
            if (tbThongTinKiemDinhCuaChuongTrinh.IdKetQuaKiemDinh == null) ModelState.AddModelError("IdKetQuaKiemDinh", "Không được bỏ trống!");
            if (tbThongTinKiemDinhCuaChuongTrinh.SoQuyetDinh == null) ModelState.AddModelError("SoQuyetDinh", "Không được bỏ trống!");
            if (tbThongTinKiemDinhCuaChuongTrinh.NgayCapChungNhanKiemDinh == null) ModelState.AddModelError("NgayCapChungNhanKiemDinh", "Không được bỏ trống!");
            if (tbThongTinKiemDinhCuaChuongTrinh.ThoiHanKiemDinh == null) ModelState.AddModelError("ThoiHanKiemDinh", "Không được bỏ trống!");
        }

        [HttpPost]
        public IActionResult Receive_Excel(string jsonExcel)
        {
            try
            {
                List<List<string>> dataList = JsonConvert.DeserializeObject<List<List<string>>>(jsonExcel);
                dataList.ForEach(s =>
                {
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
                    type == "Tổ Chức Kiểm Định" ||
                    type == "Kết Quả Kiểm Định"))
                {
                    return Json(new { error = "Invalid type parameter." });
                }

                // Lấy dữ liệu từ API
                var data = ApiServices_.GetAll<TbThongTinKiemDinhCuaChuongTrinh>("/api/ctdt/ThongTinKiemDinhCuaChuongTrinh");
                var dataList = await data;

                List<TbThongTinKiemDinhCuaChuongTrinh> TbThongTinKiemDinhCuaChuongTrinhs = await ApiServices_.GetAll<TbThongTinKiemDinhCuaChuongTrinh>("/api/ctdt/ThongTinKiemDinhCuaChuongTrinh");
                List<TbChuongTrinhDaoTao> tbChuongTrinhDaoTaos = await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao");
                List<DmKetQuaKiemDinh> dmKetQuaKiemDinhs = await ApiServices_.GetAll<DmKetQuaKiemDinh>("/api/dm/KetQuaKiemDinh");
                List<DmToChucKiemDinh> dmToChucKiemDinhs = await ApiServices_.GetAll<DmToChucKiemDinh>("/api/dm/ToChucKiemDinh");

                // Gán navigation properties
                TbThongTinKiemDinhCuaChuongTrinhs.ForEach(item =>
                {
                    item.IdChuongTrinhDaoTaoNavigation = tbChuongTrinhDaoTaos.FirstOrDefault(t => t.IdChuongTrinhDaoTao == item.IdChuongTrinhDaoTao);
                    item.IdKetQuaKiemDinhNavigation = dmKetQuaKiemDinhs.FirstOrDefault(t => t.IdKetQuaKiemDinh == item.IdKetQuaKiemDinh);
                    item.IdToChucKiemDinhNavigation = dmToChucKiemDinhs.FirstOrDefault(t => t.IdToChucKiemDinh == item.IdToChucKiemDinh);
                });

                // Kiểm tra loại biểu đồ
                if (type == "Số quyết định")
                {
                    var resultFiltered = TbThongTinKiemDinhCuaChuongTrinhs.Select(s => new
                    {
                        TenChuongTrinh = s.IdChuongTrinhDaoTaoNavigation?.TenChuongTrinh ?? "Không xác định",
                        Value = s.SoQuyetDinh ?? "Không xác định"
                    }).ToList();

                    return Json(resultFiltered);
                }
                else if (type == "Kết Quả Kiểm Định")
                {
                    var resultFiltered = TbThongTinKiemDinhCuaChuongTrinhs.Select(s => new
                    {
                        TenChuongTrinh = s.IdChuongTrinhDaoTaoNavigation?.TenChuongTrinh ?? "Không xác định",
                        Value = s.IdKetQuaKiemDinh ?? 0
                    }).ToList();

                    return Json(resultFiltered);
                }
                else if (type == "Tổ Chức Kiểm Định")
                {
                    var resultFiltered = TbThongTinKiemDinhCuaChuongTrinhs.Select(s => new
                    {
                        TenChuongTrinh = s.IdChuongTrinhDaoTaoNavigation?.TenChuongTrinh ?? "Không xác định",
                        Value = s.IdToChucKiemDinh ?? 0
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

        public IActionResult UploadExcel()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadExcel(IFormFile file)
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
                                try
                                {
                                    // Bỏ qua header
                                    if (!isHeaderSkipped)
                                    {
                                        isHeaderSkipped = true;
                                        continue;
                                    }

                                    // Đọc từng dòng dữ liệu
                                    TbThongTinKiemDinhCuaChuongTrinh tb = new TbThongTinKiemDinhCuaChuongTrinh
                                    {
                                        IdThongTinKiemDinhCuaChuongTrinh = reader.GetValue(1) != null ? Convert.ToInt32(reader.GetValue(1).ToString()) : 0,
                                        IdChuongTrinhDaoTao = reader.GetValue(2) != null ? Convert.ToInt32(reader.GetValue(2).ToString()) : 0,
                                        IdToChucKiemDinh = reader.GetValue(3) != null ? Convert.ToInt32(reader.GetValue(3).ToString()) : 0,
                                        IdKetQuaKiemDinh = reader.GetValue(4) != null ? Convert.ToInt32(reader.GetValue(4).ToString()) : 0,
                                        //SoQuyetDinh = reader.GetValue(4) != null ? Convert.ToInt32(reader.GetValue(4).ToString()) : 0,
                                        //NgayCapChungNhanKiemDinh = reader.GetValue(4) != null ? Convert.ToInt32(reader.GetValue(4).ToString()) : 0,
                                        //ThoiHanKiemDinh = reader.GetValue(4) != null ? Convert.ToInt32(reader.GetValue(4).ToString()) : 0


                                        SoQuyetDinh = reader.GetValue(5) != null ? reader.GetValue(5).ToString() : null,
                                        NgayCapChungNhanKiemDinh = reader.GetValue(6) != null ? DateOnly.Parse(reader.GetValue(6).ToString()) : (DateOnly?)null,
                                        ThoiHanKiemDinh = reader.GetValue(7) != null ? DateOnly.Parse(reader.GetValue(7).ToString()) : (DateOnly?)null
                                    };


                                    _dbcontext.TbThongTinKiemDinhCuaChuongTrinhs.Add(tb);
                                    await ApiServices_.Create<TbThongTinKiemDinhCuaChuongTrinh>("/api/ctdt/ThongTinKiemDinhCuaChuongTrinh", tb);

                                }
                                catch (Exception ex)
                                {
                                    // Nếu gặp lỗi khi đọc và xử lý dòng dữ liệu, ghi log và tiếp tục với dòng tiếp theo
                                    Console.WriteLine($"Lỗi khi đọc dòng dữ liệu: {ex.Message}");
                                    continue;
                                }
                                await _dbcontext.SaveChangesAsync();
                            }
                        }
                        TempData["Success"] = "File đã được import thành công!";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Lỗi khi đọc file: {ex.Message}";
                }
                    
                }
            else
            {
                TempData["Error"] = "Vui lòng chọn file để upload.";
            }
            return RedirectToAction("UploadExcel");
            }

            
        }

    }






#region CMT

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using CTDT.Models;
//using CTDT.API;
////using CTDT.Models.DM;
//namespace CTDT.Controllers
//{
//    public class ThongTinKiemDinhCuaChuongTrinhController : Controller
//    {
//        private readonly ApiServices ApiServices_;
//        // Lấy từ HemisContext 
//        public ThongTinKiemDinhCuaChuongTrinhController(ApiServices services)
//        {
//            ApiServices_ = services;
//        }

//        // GET: ChuongTrinhDaoTao
//        // Lấy danh sách NADCT từ database, trả về view Index.
//        private async Task<List<TbThongTinKiemDinhCuaChuongTrinh>> TbThongTinKiemDinhCuaChuongTrinh()
//        {
//            List<TbThongTinKiemDinhCuaChuongTrinh> tbThongTinKiemDinhCuaChuongTrinhs = await ApiServices_.GetAll<TbThongTinKiemDinhCuaChuongTrinh>("/api/ctdt/ThongTinKiemDinhCuaChuongTrinh");


//            List<TbChuongTrinhDaoTao> tbChuongTrinhDaoTaos = await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao");
//            List<DmKetQuaKiemDinh> dmKetQuaKiemDinhs = await ApiServices_.GetAll<DmKetQuaKiemDinh>("/api/dm/KetQuaKiemDinh");
//            List<DmToChucKiemDinh> dmToChucKiemDinhs = await ApiServices_.GetAll<DmToChucKiemDinh>("/api/dm/ToChucKiemDinh");

//            tbThongTinKiemDinhCuaChuongTrinhs.ForEach(item =>
//            {
//                item.IdChuongTrinhDaoTaoNavigation = tbChuongTrinhDaoTaos.FirstOrDefault(t => t.IdChuongTrinhDaoTao == item.IdChuongTrinhDaoTao);
//                item.IdKetQuaKiemDinhNavigation = dmKetQuaKiemDinhs.FirstOrDefault(t => t.IdKetQuaKiemDinh == item.IdKetQuaKiemDinh);
//                item.IdToChucKiemDinhNavigation = dmToChucKiemDinhs.FirstOrDefault(t => t.IdToChucKiemDinh == item.IdToChucKiemDinh);
//            });

//                return tbThongTinKiemDinhCuaChuongTrinhs;

//           }
//        public async Task<IActionResult> Index(string Id, string SapXep)
//        {
//            try
//            {

//                List<TbThongTinKiemDinhCuaChuongTrinh> tbThongTinKiemDinhCuaChuongTrinhs = await TbThongTinKiemDinhCuaChuongTrinh();
//                var danhSach = tbThongTinKiemDinhCuaChuongTrinhs.Where(item => string.IsNullOrEmpty(Id) || item.IdThongTinKiemDinhCuaChuongTrinh.ToString() == Id) //  tìm kiếm theo Id NADCT
//                .ToList();

//                var sapXepDanhSach = danhSach; // sắp xếp
//                if (SapXep == "SapXep")
//                {
//                    sapXepDanhSach = danhSach.OrderBy(x => x.NgayCapChungNhanKiemDinh).ToList();// Năm áp dụng
//                }

//                ViewBag.KqTimKiem = danhSach;
//                ViewBag.KqSapXep = sapXepDanhSach;

//                return View(sapXepDanhSach);
//                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
//                // Bắt lỗi các trường hợp ngoại lệ
//            }
//            catch (Exception ex)
//            {
//                return BadRequest();
//            }
//        }

//        // Lấy chi tiết 1 bản ghi dựa theo ID tương ứng đã truyền vào (IdChuongTrinhDaoTao)
//        // Hiển thị bản ghi đó ở view Details
//        public async Task<IActionResult> Details(int? id)
//        {
//            try
//            {
//                if (id == null)
//                {
//                    return NotFound();
//                }

//                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details

//                List<TbThongTinKiemDinhCuaChuongTrinh> tbThongTinKiemDinhCuaChuongTrinhs = await TbThongTinKiemDinhCuaChuongTrinh();

//                var tb = tbThongTinKiemDinhCuaChuongTrinhs.FirstOrDefault(m => m.IdThongTinKiemDinhCuaChuongTrinh == id);
//                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
//                if (tb == null)
//                {
//                    return NotFound();
//                }
//                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
//                // Hiển thị thông thi chi tiết NADCT thành công
//                return View(tb);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest();
//            }

//        }

//        // GET: ChuongTrinhDaoTao/Create
//        // Hiển thị view Create để tạo một bản ghi NADCT mới
//        // Truyền data từ các table khác hiển thị tại view Create (khóa ngoài)
//        public async Task<IActionResult> Create()
//        {
//            try
//            {
//                ViewData["IdChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao"), "IdChuongTrinhDaoTao", "TenChuongTrinh");
//                ViewData["IdKetQuaKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmKetQuaKiemDinh>("/api/dm/KetQuaKiemDinh"), "IdKetQuaKiemDinh", "KetQuaKiemDinh");
//                ViewData["IdToChucKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmToChucKiemDinh>("/api/dm/ToChucKiemDinh"), "IdToChucKiemDinh", "ToChucKiemDinh");
//                return View();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest();
//            }

//        }

//        // POST: ChuongTrinhDaoTao/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

//        // Thêm một NADCT mới vào Database nếu IdChuongTrinhDaoTao truyền vào không trùng với Id đã có trong Database
//        // Trong trường hợp nhập trùng IdChuongTrinhDaoTao sẽ bắt lỗi
//        // Bắt lỗi ngoại lệ sao cho người nhập BẮT BUỘC phải nhập khác IdChuongTrinhDaoTao đã có
//        [HttpPost]
//        [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
//        public async Task<IActionResult> Create([Bind("IdThongTinKiemDinhCuaChuongTrinh,IdChuongTrinhDaoTao,IdToChucKiemDinh,IdKetQuaKiemDinh,SoQuyetDinh,NgayCapChungNhanKiemDinh,ThoiHanKiemDinh")] TbThongTinKiemDinhCuaChuongTrinh tbThongTinKiemDinhCuaChuongTrinh)
//        {
//            try
//            {
//                check_null(tbThongTinKiemDinhCuaChuongTrinh);
//                // Nếu trùng IdThongTinKiemDinhCuaChuongTrinh sẽ báo lỗi
//                if (await TbThongTinKiemDinhCuaChuongTrinhExists(tbThongTinKiemDinhCuaChuongTrinh.IdThongTinKiemDinhCuaChuongTrinh)) ModelState.AddModelError("IdThongTinKiemDinhCuaChuongTrinh", "ID này đã tồn tại!");
//                if (ModelState.IsValid)
//                {
//                    await ApiServices_.Create<TbThongTinKiemDinhCuaChuongTrinh>("/api/ctdt/ThongTinKiemDinhCuaChuongTrinh", tbThongTinKiemDinhCuaChuongTrinh);
//                    return RedirectToAction(nameof(Index));
//                }

//                ViewData["IdChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao"), "IdChuongTrinhDaoTao", "TenChuongTrinh");
//                ViewData["IdKetQuaKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmKetQuaKiemDinh>("/api/dm/KetQuaKiemDinh"), "IdKetQuaKiemDinh", "KetQuaKiemDinh");
//                ViewData["IdToChucKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmToChucKiemDinh>("/api/dm/ToChucKiemDinh"), "IdToChucKiemDinh", "ToChucKiemDinh");

//                return View(tbThongTinKiemDinhCuaChuongTrinh);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest();
//            }

//        }

//        // GET: ChuongTrinhDaoTao/Edit
//        // Lấy data từ Database với Id đã có, sau đó hiển thị ở view Edit
//        // Nếu không tìm thấy Id tương ứng sẽ báo lỗi NotFound
//        // Phương thức này gần giống Create, nhưng nó nhập dữ liệu vào Id đã có trong database
//        public async Task<IActionResult> Edit(int? id)
//        {
//            try
//            {
//                if (id == null)
//                {
//                    return NotFound();
//                }

//                var TbThongTinKiemDinhCuaChuongTrinh = await ApiServices_.GetId<TbThongTinKiemDinhCuaChuongTrinh>("/api/ctdt/ThongTinKiemDinhCuaChuongTrinh", id ?? 0);
//                if (TbThongTinKiemDinhCuaChuongTrinh == null)
//                {
//                    return NotFound();
//                }


//                ViewData["IdChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao"), "IdChuongTrinhDaoTao", "TenChuongTrinh");
//                ViewData["IdKetQuaKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmKetQuaKiemDinh>("/api/dm/KetQuaKiemDinh"), "IdKetQuaKiemDinh", "KetQuaKiemDinh");
//                ViewData["IdToChucKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmToChucKiemDinh>("/api/dm/ToChucKiemDinh"), "IdToChucKiemDinh", "ToChucKiemDinh");


//                return View(TbThongTinKiemDinhCuaChuongTrinh);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest();
//            }

//        }

//        // POST: ChuongTrinhDaoTao/Edit
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

//        // Lưu data mới (ghi đè) vào các trường Data đã có thuộc IdThongTinKiemDinhCuaChuongTrinh cần chỉnh sửa
//        // Nó chỉ cập nhật khi ModelState hợp lệ
//        // Nếu không hợp lệ sẽ báo lỗi, vì vậy cần có bắt lỗi.

//        [HttpPost]
//        [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
//        public async Task<IActionResult> Edit(int id, [Bind("IdThongTinKiemDinhCuaChuongTrinh,IdChuongTrinhDaoTao,IdToChucKiemDinh,IdKetQuaKiemDinh,SoQuyetDinh,NgayCapChungNhanKiemDinh,ThoiHanKiemDinh")] TbThongTinKiemDinhCuaChuongTrinh tbThongTinKiemDinhCuaChuongTrinh)
//        {
//            try
//            {
//                if (id != tbThongTinKiemDinhCuaChuongTrinh.IdThongTinKiemDinhCuaChuongTrinh)
//                {
//                    return NotFound();
//                }
//                check_null(tbThongTinKiemDinhCuaChuongTrinh);
//                if (ModelState.IsValid)
//                {
//                    try
//                    {
//                        await ApiServices_.Update<TbThongTinKiemDinhCuaChuongTrinh>("/api/ctdt/ThongTinKiemDinhCuaChuongTrinh", id, tbThongTinKiemDinhCuaChuongTrinh);
//                    }
//                    catch (DbUpdateConcurrencyException)
//                    {
//                        if (await TbThongTinKiemDinhCuaChuongTrinhExists(tbThongTinKiemDinhCuaChuongTrinh.IdThongTinKiemDinhCuaChuongTrinh) == false)
//                        {
//                            return NotFound();
//                        }
//                        else
//                        {
//                            throw;
//                        }
//                    }
//                    return RedirectToAction(nameof(Index));
//                }
//                ViewData["IdChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao"), "IdChuongTrinhDaoTao", "TenChuongTrinh");
//                ViewData["IdKetQuaKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmKetQuaKiemDinh>("/api/dm/KetQuaKiemDinh"), "IdKetQuaKiemDinh", "KetQuaKiemDinh");
//                ViewData["IdToChucKiemDinh"] = new SelectList(await ApiServices_.GetAll<DmToChucKiemDinh>("/api/dm/ToChucKiemDinh"), "IdToChucKiemDinh", "ToChucKiemDinh");
//                return View(tbThongTinKiemDinhCuaChuongTrinh);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest();
//            }

//        }

//        // GET: ChuongTrinhDaoTao/Delete
//        // Xóa một NADCT khỏi Database
//        // Lấy data NADCT từ Database, hiển thị Data tại view Delete
//        // Hàm này để hiển thị thông tin cho người dùng trước khi xóa
//        public async Task<IActionResult> Delete(int? id)
//        {
//            try
//            {
//                if (id == null)
//                {
//                    return NotFound();
//                }
//                var tbThongTinKiemDinhCuaChuongTrinhs = await ApiServices_.GetAll<TbThongTinKiemDinhCuaChuongTrinh>("/api/ctdt/ThongTinKiemDinhCuaChuongTrinh");
//                var tbThongTinKiemDinhCuaChuongTrinh = tbThongTinKiemDinhCuaChuongTrinhs.FirstOrDefault(m => m.IdThongTinKiemDinhCuaChuongTrinh == id);
//                if (tbThongTinKiemDinhCuaChuongTrinh == null)
//                {
//                    return NotFound();
//                }

//                return View(tbThongTinKiemDinhCuaChuongTrinh);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest();
//            }

//        }

//        // POST: ChuongTrinhDaoTao/Delete
//        // Xóa NADCT khỏi Database sau khi nhấn xác nhận 
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id) // Lệnh xác nhận xóa hẳn một NADCT
//        {
//            try
//            {
//                await ApiServices_.Delete<TbThongTinKiemDinhCuaChuongTrinh>("/api/ctdt/ThongTinKiemDinhCuaChuongTrinh", id);
//                return RedirectToAction(nameof(Index));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest();
//            }

//        }
//        // hàm check xem tồn tại không
//        private async Task<bool> TbThongTinKiemDinhCuaChuongTrinhExists(int id)
//        {
//            var TbThongTinKiemDinhCuaChuongTrinhs = await ApiServices_.GetAll<TbThongTinKiemDinhCuaChuongTrinh>("/api/ctdt/ThongTinKiemDinhCuaChuongTrinh");
//            return TbThongTinKiemDinhCuaChuongTrinhs.Any(e => e.IdThongTinKiemDinhCuaChuongTrinh == id);
//        }
//        private void check_null(TbThongTinKiemDinhCuaChuongTrinh tbThongTinKiemDinhCuaChuongTrinh)
//        {
//            if (tbThongTinKiemDinhCuaChuongTrinh.IdChuongTrinhDaoTao == null) ModelState.AddModelError("IdChuongTrinhDaoTao", "Vui lòng nhập vào ô trống!");
//            if (tbThongTinKiemDinhCuaChuongTrinh.IdToChucKiemDinh == null) ModelState.AddModelError("IdToChucKiemDinh", "Vui lòng nhập vào ô trống!");
//            if (tbThongTinKiemDinhCuaChuongTrinh.IdKetQuaKiemDinh == null) ModelState.AddModelError("IdKetQuaKiemDinh", "Không được bỏ trống!");
//            if (tbThongTinKiemDinhCuaChuongTrinh.SoQuyetDinh == null) ModelState.AddModelError("SoQuyetDinh", "Không được bỏ trống!");
//            if (tbThongTinKiemDinhCuaChuongTrinh.NgayCapChungNhanKiemDinh == null) ModelState.AddModelError("NgayCapChungNhanKiemDinh", "Không được bỏ trống!");

//            if (tbThongTinKiemDinhCuaChuongTrinh.ThoiHanKiemDinh == null) ModelState.AddModelError("ThoiHanKiemDinh", "Không được bỏ trống!");
//        }
//    }
//}
#endregion