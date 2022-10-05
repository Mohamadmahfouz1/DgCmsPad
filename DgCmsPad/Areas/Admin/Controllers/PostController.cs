using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DgCmsPad.Infrastructure;
using DgCmsPad.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DgCmsPad.Areas.Admin.Controllers
{
    //[Authorize(Roles = "admin")]
    [Area("Admin")]
    public class PostController : Controller
    {
        private readonly DgCmsPadContext context;

        private readonly IWebHostEnvironment webHostEnvironment;


        public PostController(DgCmsPadContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;


         
        }

        //get/admin/posts

        public async Task<IActionResult> Index()
        {
            return View(await context.Posts.OrderByDescending(x=>x.Id).Include(x=>x.Term).ToListAsync());


        }


        //get/admin/posts/create
        public IActionResult Create()
        {
            ViewBag.PostTypeId = new SelectList(context.Terms, "Id", "Name");
            //ViewBag.PostTypeId = new SelectList(context.Posts.OrderBy(x => x.Term), "Id", "Name");

            return View();

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        //post/admin/posts/create
        public async Task<IActionResult> Create(Post post)
        {

            ViewBag.PostTypeId = new SelectList(context.Posts.OrderBy(x => x.Term), "Id", "Name");
            if (ModelState.IsValid)
            {
                post.Slug = post.Name.ToLower().Replace(" ", "-");
                
                var slug = await context.Posts.FirstOrDefaultAsync(x => x.Slug == post.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The post Already Exists");
                    return View(post);
                }

                string imageName = "noimage.png";
                if(post.ImageUpload !=null)
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/posts");
                    imageName = Guid.NewGuid().ToString() + "_" + post.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await post.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    
                }
                post.Image = imageName;
                context.Add(post);
                await context.SaveChangesAsync();

                TempData["Success"] = "The post has been added";


                return RedirectToAction("Index");
            }
            return View(post);

        }

        //get/admin/posts/details/5
        public async Task<IActionResult> Details(int id)
        {
            Post post = await context.Posts.Include(x=>x.PostTypeId).FirstOrDefaultAsync(x => x.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);

        }

        //get/admin/posts/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Post post = await context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            ViewBag.PostTypeId = new SelectList(context.Posts.OrderBy(x => x.Term), "Id", "Name",post.PostTypeId);


            return View(post);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        //post/admin/posts/edit/5
        public async Task<IActionResult> Edit(int id,Post post)
        {

            ViewBag.PostTypeId = new SelectList(context.Posts.OrderBy(x => x.Term), "Id", "Name",post.PostTypeId);
            if (ModelState.IsValid)
            {
                post.Slug = post.Name.ToLower().Replace(" ", "-");

                var slug = await context.Posts.Where(x=>x.Id !=id).FirstOrDefaultAsync(x => x.Slug == post.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The post Already Exists");
                    return View(post);
                }

                if (post.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/posts");

                    if(!string.Equals(post.Image,"noimage.png"))
                    {
                        string oldImagePath = Path.Combine(uploadsDir, post.Image);
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    string imageName = Guid.NewGuid().ToString() + "_" + post.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await post.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    post.Image = imageName;
                }
               
                context.Update(post);
                await context.SaveChangesAsync();

                TempData["Success"] = "The post has been edited";


                return RedirectToAction("Index");
            }
            return View(post);

        }

        //get/admin/posts/delete/5
        public async Task<IActionResult> Delete(int id)
        {
            Post post = await context.Posts.FindAsync(id);
            if (post == null)
            {
                TempData["Error"] = "The post does not exist";
            }
            else
            {
                if (!string.Equals(post.Image, "noimage.png"))
                {

                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/posts");
                    string oldImagePath = Path.Combine(uploadsDir, post.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                context.Posts.Remove(post);
                await context.SaveChangesAsync();
                TempData["Success"] = "The post has been deleted";
            }
            return RedirectToAction("Index");
        }
    }
}
