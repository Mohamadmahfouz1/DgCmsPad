using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using DgCmsPad.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace DgCmsPad.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
        [ForeignKey("Code")]
        public string Name { get; set; }
        public string Slug { get; set; }

        [Required, MinLength(4, ErrorMessage = "Minimum length is 4")]
        public string Details { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Date { get; set; }

        [Display(Name = "Category")]
        [Range(1, int.MaxValue, ErrorMessage = "You must choose a term")]
        public int PostTypeId { get; set; }

        [FileExtension]
        public string Image { get; set; }

        [ForeignKey("PostTypeId")]
        public virtual Term Term { get; set; }
        [NotMapped]
        
        public IFormFile ImageUpload { get; set; }

    }
}
