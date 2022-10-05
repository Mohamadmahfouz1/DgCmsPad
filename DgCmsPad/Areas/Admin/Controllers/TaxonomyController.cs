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
    public class TaxonomyController : Controller
    {
        private readonly DgCmsPadContext context;
        public TaxonomyController(DgCmsPadContext context)
        {
            this.context = context;

        }

        //get/admin/taxonomies
        public async Task<IActionResult> Index()
        {
            IQueryable<Taxonomy> taxonomies = from p in context.Taxonomies orderby p.Sorting select p;
            List<Taxonomy> pageList = await taxonomies.ToListAsync();
            return View(pageList);
        }

        //get/admin/taxonomies/details/5
        public async Task<IActionResult> Details(int id)
        {
            Taxonomy taxonomy = await context.Taxonomies.FirstOrDefaultAsync(x => x.Id == id);
            if (taxonomy == null)
            {
                return NotFound();
            }
            return View(taxonomy);

        }
        //get/admin/taxonomies/create
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        //post/admin/taxonomies/create
        public async Task<IActionResult> Create(Taxonomy taxonomy)
        {
            if (ModelState.IsValid)
            {
                taxonomy.Slug = taxonomy.Name.ToLower().Replace(" ", "-");
                taxonomy.Sorting = 100;
                var slug = await context.Taxonomies.FirstOrDefaultAsync(x => x.Slug == taxonomy.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The Page Already Exists");
                    return View(taxonomy);
                }
                context.Add(taxonomy);
                await context.SaveChangesAsync();

                TempData["Success"] = "The page has been added";


                return RedirectToAction("Index");
            }
            return View(taxonomy);

        }
        //get/admin/taxonomies/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Taxonomy taxonomy = await context.Taxonomies.FindAsync(id);
            if (taxonomy == null)
            {
                return NotFound();
            }
            return View(taxonomy);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        //post/admin/taxonomies/edit
        public async Task<IActionResult> Edit(Taxonomy taxonomy)
        {
            if (ModelState.IsValid)
            {
                taxonomy.Slug = taxonomy.Id == 1 ? "home " : taxonomy.Name.ToLower().Replace(" ", "-");


                var slug = await context.Taxonomies.Where(x => x.Id != taxonomy.Id).FirstOrDefaultAsync(x => x.Slug == taxonomy.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The Page Already Exists");
                    return View(taxonomy);
                }
                context.Update(taxonomy);
                await context.SaveChangesAsync();

                TempData["Success"] = "The page has been edited";


                return RedirectToAction("Edit", new { id = taxonomy.Id });
            }
            return View(taxonomy);


        }

        //get/admin/taxonomies/delete/5
        public async Task<IActionResult> Delete(int id)
        {
            Taxonomy taxonomy = await context.Taxonomies.FindAsync(id);
            if (taxonomy == null)
            {
                TempData["Error"] = "The page does not exist";
            }
            else
            {
                context.Taxonomies.Remove(taxonomy);
                await context.SaveChangesAsync();
                TempData["Success"] = "The page has been deleted";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]

        //post/admin/taxonomies/reorder
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;

            foreach (var pageId in id)
            {
                Taxonomy taxonomy = await context.Taxonomies.FindAsync(pageId);
                taxonomy.Sorting = count;
                context.Update(taxonomy);
                await context.SaveChangesAsync();
                count++;

            }
            return Ok();
        }
    }
}
