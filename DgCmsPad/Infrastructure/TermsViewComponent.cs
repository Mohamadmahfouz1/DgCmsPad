using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DgCmsPad.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DgCmsPad.Infrastructure
{
    public class TermsViewComponent : ViewComponent
    {

        private readonly DgCmsPadContext context;
        public TermsViewComponent(DgCmsPadContext context)
        {
            this.context = context;

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var terms = await GetTermsAsync();
            return View(terms);
        }

        private Task<List<Term>> GetTermsAsync()
        {
            return context.Terms.OrderBy(x => x.Sorting).ToListAsync();
        }
    }
}
