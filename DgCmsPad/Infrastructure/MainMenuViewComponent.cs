using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DgCmsPad.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DgCmsPad.Infrastructure
{
    public class MainMenuViewComponent : ViewComponent
    {
        private readonly DgCmsPadContext context;
        public MainMenuViewComponent(DgCmsPadContext context)
        {
            this.context = context;

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pages = await GetPagesAsync();  
            return View(pages);
        }

        private Task<List<Taxonomy>> GetPagesAsync()
        {
            return context.Taxonomies.OrderBy(x => x.Sorting).ToListAsync();
        }
    }
}
