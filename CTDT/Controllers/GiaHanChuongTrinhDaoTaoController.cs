
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CTDT.Models;
using CTDT.API;
//using CTDT.Models.DM;
namespace CTDT.Controllers
{
    public class GiaHanChuongTrinhDaoTaoController : Controller
    {
        private readonly ApiServices ApiServices_;
        // Lấy từ HemisContext 
        public GiaHanChuongTrinhDaoTaoController(ApiServices services)
        {
            ApiServices_ = services;
        }

        // GET: ChuongTrinhDaoTao
        // Lấy danh sách CTĐT từ database, trả về view Index.
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbGiaHanChuongTrinhDaoTao> getall = await ApiServices_.GetAll<TbGiaHanChuongTrinhDaoTao>("/api/ctdt/GiaHanChuongTrinhDaoTao");
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);

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
                var tbGiaHanChuongTrinhDaoTaos = await ApiServices_.GetAll<TbGiaHanChuongTrinhDaoTao>("/api/ctdt/GiaHanChuongTrinhDaoTao");
                var tb = tbGiaHanChuongTrinhDaoTaos.FirstOrDefault(m => m.IdGiaHanChuongTrinhDaoTao == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tb == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tb);
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

        // Thêm một CTĐT mới vào Database nếu IdChuongTrinhDaoTao truyền vào không trùng với Id đã có trong Database
        // Trong trường hợp nhập trùng IdChuongTrinhDaoTao sẽ bắt lỗi
        // Bắt lỗi ngoại lệ sao cho người nhập BẮT BUỘC phải nhập khác IdChuongTrinhDaoTao đã có
        [HttpPost]
        [ValidateAntiForgeryToken] // Một phương thức bảo mật thông qua Token được tạo tự động cho các Form khác nhau
        public async Task<IActionResult> Create([Bind("IdGiaHanChuongTrinhDaoTao,IdChuongTrinhDaoTao,SoQuyetDinhGiaHan,NgayBanHanhVanBanGiaHan,GiaHanLanThu")] TbGiaHanChuongTrinhDaoTao tbGiaHanChuongTrinhDaoTao)
        {
            try
            {
                // Nếu trùng IDChuongTrinhDaoTao sẽ báo lỗi
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

        private async Task<bool> TbGiaHanChuongTrinhDaoTaoExists(int id)
        {
            var TbGiaHanChuongTrinhDaoTaos = await ApiServices_.GetAll<TbGiaHanChuongTrinhDaoTao>("/api/ctdt/GiaHanChuongTrinhDaoTao");
            return TbGiaHanChuongTrinhDaoTaos.Any(e => e.IdGiaHanChuongTrinhDaoTao == id);
        }
    }
}
