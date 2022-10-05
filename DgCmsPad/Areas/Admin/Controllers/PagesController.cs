using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DgCmsPad.Infrastructure;
using DgCmsPad.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DgCmsPad.Areas.Admin.Controllers.Pages
{
    public class PagesController : Controller
    {

        private readonly DgCmsPadContext context;
        public PagesController(DgCmsPadContext context)
        {
            this.context = context;

        }
        //get/or/slug
        public async Task<IActionResult> Page(string slug)
        {
            if (slug == null)
            {
                return View(await context.Taxonomies.Where(x => x.Slug == "home").FirstOrDefaultAsync());
            }

            Taxonomy taxonomy = await context.Taxonomies.Where(x => x.Slug == slug).FirstOrDefaultAsync();

            if (taxonomy == null)
            {
                return NotFound();
            }

            return View(taxonomy);
        }
    }
}
