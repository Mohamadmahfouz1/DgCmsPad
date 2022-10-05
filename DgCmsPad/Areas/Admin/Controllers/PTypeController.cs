using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DgCmsPad.Infrastructure;
using DgCmsPad.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DgCmsPad.Areas.Admin.Controllers
{
    //[Authorize(Roles ="admin")]
    [Area("Admin")]
    public class TestController : Controller
    {
        private readonly DgCmsPadContext context;
        public TestController(DgCmsPadContext context)
        {
            this.context = context;

        }

        //get/admin/pTypes
        public async Task<IActionResult> Index()
        {
            return View(await context.PTypes.OrderBy(x => x.Sorting).ToListAsync());
        }

        //get/admin/pTypes/details/5
        public async Task<IActionResult> Details(int id)
        {
            PType pTypes = await context.PTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (pTypes == null)
            {
                return NotFound();
            }
            return View(pTypes);

        }
        //get/admin/pTypes/create
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        //post/admin/pTypes/create
        public async Task<IActionResult> Create(PType pType)
        {
            if (ModelState.IsValid)
            {
                pType.Slug = pType.Title.ToLower().Replace(" ", "-");
                pType.Sorting = 100;
                var slug = await context.PTypes.FirstOrDefaultAsync(x => x.Slug == pType.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The Page Already Exists");
                    return View(pType);
                }
                context.Add(pType);
                await context.SaveChangesAsync();

                TempData["Success"] = "The page has been added";


                return RedirectToAction("Index");
            }
            return View(pType);

        }
        //get/admin/pTypes/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            PType pType = await context.PTypes.FindAsync(id);
            if (pType == null)
            {
                return NotFound();
            }
            return View(pType);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        //post/admin/pTypes/edit
        public async Task<IActionResult> Edit(PType pType)
        {
            if (ModelState.IsValid)
            {
                pType.Slug = pType.Id == 1 ? "home " : pType.Title.ToLower().Replace(" ", "-");


                var slug = await context.PTypes.Where(x => x.Id != pType.Id).FirstOrDefaultAsync(x => x.Slug == pType.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The Page Already Exists");
                    return View(pType);
                }
                context.Update(pType);
                await context.SaveChangesAsync();

                TempData["Success"] = "The page has been edited";


                return RedirectToAction("Edit", new { id = pType.Id });
            }
            return View(pType);


        }

        //get/admin/pTypes/delete/5
        public async Task<IActionResult> Delete(int id)
        {
            PType pType = await context.PTypes.FindAsync(id);
            if (pType == null)
            {
                TempData["Error"] = "The page does not exist";
            }
            else
            {
                context.PTypes.Remove(pType);
                await context.SaveChangesAsync();
                TempData["Success"] = "The page has been deleted";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]

        //post/admin/pTypes/reorder
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;

            foreach (var pageId in id)
            {
                PType pType = await context.PTypes.FindAsync(pageId);
                pType.Sorting = count;
                context.Update(pType);
                await context.SaveChangesAsync();
                count++;

            }
            return Ok();
        }
    }
}
