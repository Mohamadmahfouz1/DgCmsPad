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
   // [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class TermController : Controller
    {

        private readonly DgCmsPadContext context;
        public TermController(DgCmsPadContext context)
        {
            this.context = context;

        }
        //get/admin/terms
        public async Task< IActionResult> Index()
        {
            return View(await context.Terms.OrderBy(x => x.Sorting).ToListAsync());
        }
        //get/admin/terms/create
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        //post/admin/terms/create
        public async Task<IActionResult> Create(Term term)
        {
            if (ModelState.IsValid)
            {
                term.Slug = term.Name.ToLower().Replace(" ", "-");
                term.Sorting = 100;
                term.Code = term.Name.ToLower().Replace(" ", "_");
                var slug = await context.Terms.FirstOrDefaultAsync(x => x.Slug == term.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The term Already Exists");
                    return View(term);
                }
                context.Add(term);
                await context.SaveChangesAsync();

                TempData["Success"] = "The term has been added";


                return RedirectToAction("Index");
            }
            
            return View(term);

        }

        //get/admin/terms/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Term term = await context.Terms.FindAsync(id);
            if (term == null)
            {
                return NotFound();
            }
            return View(term);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        //post/admin/terms/edit/5
        public async Task<IActionResult> Edit(int id, Term term)
        {
            if (ModelState.IsValid)
            {
                term.Slug = term.Name.ToLower().Replace(" ", "-");


                var slug = await context.Terms.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == term.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The term Already Exists");
                    return View(term);
                }
                context.Update(term);
                await context.SaveChangesAsync();

                TempData["Success"] = "The term has been edited";


                return RedirectToAction("Edit", new { id });
            }
            return View(term);


        }

        //get/admin/terms/delete/5
        public async Task<IActionResult> Delete(int id)
        {
            Term term = await context.Terms.FindAsync(id);
            if (term == null)
            {
                TempData["Error"] = "The term does not exist";
            }
            else
            {
                context.Terms.Remove(term);
                await context.SaveChangesAsync();
                TempData["Success"] = "The term has been deleted";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]

        //post/admin/terms/reorder
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;

            foreach (var termId in id)
            {
                Term term = await context.Terms.FindAsync(termId);
                term.Sorting = count;
                context.Update(term);
                await context.SaveChangesAsync();
                count++;

            }
            return Ok();
        }
    }
}
