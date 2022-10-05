using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DgCmsPad.Models
{
    public class Taxonomy
    {
        public int Id { get; set; }
        [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
        public string Name { get; set; }
        
        public string Slug { get; set; }
        [Required, MinLength(4, ErrorMessage = "Minimum length is 4")]
        public string Code { get; set; }
        public int Sorting { get; set; }
       // public ICollection<PostTypeTaxonomy> postTypeTaxonomies { get; set; }

    }
}
