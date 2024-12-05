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


namespace C500Hemis.Controllers.CTDT
{
    public class NgonNguGiangDayController : Controller
    {
        private readonly ApiServices ApiServices_;

        public NgonNguGiangDayController(ApiServices services)
        {
            ApiServices_ = services;
        }

        private async Task<List<TbNgonNguGiangDay>> TbNgonNguGiangDays()
        {
            List<TbNgonNguGiangDay> TbNgonNguGiangDays = await ApiServices_.GetAll<TbNgonNguGiangDay>("/api/ctdt/NgonNguGiangDay");
            List<TbChuongTrinhDaoTao> tbChuongTrinhDaoTaos = await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao");
            List<DmNgoaiNgu> dmNgoaiNgus = await ApiServices_.GetAll<DmNgoaiNgu>("/api/dm/NgoaiNgu");
            List<DmKhungNangLucNgoaiNgu> dmKhungNangLucNgoaiNgus = await ApiServices_.GetAll<DmKhungNangLucNgoaiNgu>("/api/dm/KhungNangLucNgoaiNgu");
            TbNgonNguGiangDays.ForEach(item => {
                item.IdChuongTrinhDaoTaoNavigation = tbChuongTrinhDaoTaos.FirstOrDefault(t => t.IdChuongTrinhDaoTao == item.IdChuongTrinhDaoTao);
                item.IdNgonNguNavigation = dmNgoaiNgus.FirstOrDefault(t => t.IdNgoaiNgu == item.IdNgonNgu);
                item.IdTrinhDoNgonNguDauVaoNavigation = dmKhungNangLucNgoaiNgus.FirstOrDefault(t => t.IdKhungNangLucNgoaiNgu == item.IdTrinhDoNgonNguDauVao);
            });
            return TbNgonNguGiangDays;
        }
        private async Task Selectlist(TbNgonNguGiangDay? tbNgonNguGiangDay = null)
        {
            if (tbNgonNguGiangDay == null)
            {
                ViewData["IdChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao"), "IdChuongTrinhDaoTao", "TenChuongTrinhDaoTao");
                ViewData["IdNgonNgu"] = new SelectList(await ApiServices_.GetAll<DmNgoaiNgu>("/api/dm/NgoaiNgu"), "IdNgonNgu", "NgoaiNgu");
                ViewData["IdTrinhDoNgonNguDauVao"] = new SelectList(await ApiServices_.GetAll<DmKhungNangLucNgoaiNgu>("/api/dm/KhungNangLucNgoaiNgu"), "IdTrinhDoNgonNguDauVao", "TenKhungNangLucNgoaiNgu");
            }
            else
            {
                ViewData["IdChuongTrinhDaoTao"] = new SelectList(await ApiServices_.GetAll<TbChuongTrinhDaoTao>("/api/ctdt/ChuongTrinhDaoTao"), "IdChuongTrinhDaoTao", "TenChuongTrinhDaoTao", tbNgonNguGiangDay.IdChuongTrinhDaoTao);
                ViewData["IdNgonNgu"] = new SelectList(await ApiServices_.GetAll<DmNgoaiNgu>("/api/dm/NgoaiNgu"), "IdNgonNgu", "NgoaiNgu", tbNgonNguGiangDay.IdNgonNgu);
                ViewData["IdTrinhDoNgonNguDauVao"] = new SelectList(await ApiServices_.GetAll<DmKhungNangLucNgoaiNgu>("/api/dm/KhungNangLucNgoaiNgu"), "IdTrinhDoNgonNguDauVao", "TenKhungNangLucNgoaiNgu", tbNgonNguGiangDay.IdTrinhDoNgonNguDauVao);
            }
        }
        public async Task<IActionResult> Index()
        {
            List<TbNgonNguGiangDay> tbNgonNguGiangDays = await TbNgonNguGiangDays();
            return View(tbNgonNguGiangDays);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbNgonNguGiangDays = await TbNgonNguGiangDays();
            var tbNgonNguGiangDay = tbNgonNguGiangDays.FirstOrDefault(m => m.IdNgonNguGiangDay == id);
            if (tbNgonNguGiangDay == null)
            {
                return NotFound();
            }

            return View(tbNgonNguGiangDay);
        }

        public async Task<IActionResult> Create()
        {
            await Selectlist();
            return View();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNgonNguGiangDay,IdChuongTrinhDaoTao,IdNgonNgu,IdTrinhDoNgonNguDauVao")] TbNgonNguGiangDay tbNgonNguGiangDay)
        {
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    return Content(error.ErrorMessage);
                }
            }
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbNgonNguGiangDay>("/api/ctdt/NgonNguGiangDay", tbNgonNguGiangDay);
                return RedirectToAction(nameof(Index));
            }
            await Selectlist(tbNgonNguGiangDay);
            return View(tbNgonNguGiangDay);
        }

        // GET: ThongTinKiemDinhCuaChuongTrinh/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbNgonNguGiangDay = await ApiServices_.GetId<TbNgonNguGiangDay>("/api/ctdt/NgonNguGiangDay", id ?? 0);
            if (tbNgonNguGiangDay == null)
            {
                return NotFound();
            }
            await Selectlist(tbNgonNguGiangDay);
            return View(tbNgonNguGiangDay);
        }

        // POST: ThongTinKiemDinhCuaChuongTrinh/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNgonNguGiangDay,IdChuongTrinhDaoTao,IdNgonNgu,IdTrinhDoNgonNguDauVao")] TbNgonNguGiangDay tbNgonNguGiangDay)
        {
            if (id != tbNgonNguGiangDay.IdNgonNguGiangDay)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbNgonNguGiangDay>("/api/ctdt/NgonNguGiangDay", id, tbNgonNguGiangDay);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await TbNgonNguGiangDayExists(tbNgonNguGiangDay.IdNgonNguGiangDay)))
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
            await Selectlist(tbNgonNguGiangDay);
            return View(tbNgonNguGiangDay);
        }

        // GET: ThongTinKiemDinhCuaChuongTrinh/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbNgonNguGiangDays = await TbNgonNguGiangDays();
            var tbNgonNguGiangDay = tbNgonNguGiangDays.FirstOrDefault(m => m.IdNgonNguGiangDay == id);
            if (tbNgonNguGiangDay == null)
            {
                return NotFound();
            }

            return View(tbNgonNguGiangDay);
        }

        // POST: ThongTinKiemDinhCuaChuongTrinh/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await ApiServices_.Delete<TbNgonNguGiangDay>("/api/ctdt/NgonNguGiangDay", id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TbNgonNguGiangDayExists(int id)
        {
            var tbNgonNguGiangDay = await ApiServices_.GetId<TbNgonNguGiangDay>("/api/ctdt/NgonNguGiangDay", id);
            if (tbNgonNguGiangDay == null)
            {
                return false;
            }
            return true;
        }
    }
}
