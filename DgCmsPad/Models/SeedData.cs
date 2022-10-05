using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DgCmsPad.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DgCmsPad.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new DgCmsPadContext(serviceProvider.GetRequiredService<DbContextOptions<DgCmsPadContext>>()))
            {
                if (context.Taxonomies.Any())
                {
                    return;
                }
                context.Taxonomies.AddRange(
                    new Taxonomy
                    {
                        Name = "Home",
                        Slug = "home",
                        Code = "home page",
                        Sorting = 0
                    }, new Taxonomy
                    {
                        Name = "About Us",
                        Slug = "about-us",
                        Code = "about us page",
                        Sorting = 100
                    }, new Taxonomy
                    {
                        Name = "Services",
                        Slug = "services",
                        Code = "services page",
                        Sorting = 100
                    }, new Taxonomy
                    {
                        Name = "Contact",
                        Slug = "contact",
                        Code = "contact page",
                        Sorting = 100
                    }
                    );
                context.SaveChanges();
            }
        }
    }
}
