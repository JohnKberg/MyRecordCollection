using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyRecordCollection.Models
{
    public class Artist
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        
        public string Bio { get; set; }

        // Declared as virtual to enable "Lazy Loading"
        // Which means you don't have to load this collection explicitly 
        // with .Include() statement on Artist collection
        public virtual ICollection<Album> Albums { get; set; }
    }
}