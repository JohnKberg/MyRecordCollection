using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyRecordCollection.Models
{
    public class Album
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public Int16 Year { get; set; }

        [StringLength(100)]
        public string Label { get; set; }

        [StringLength(20)]
        public string Format { get; set; }

        public string Description { get; set; }

        [Url]
        public string ImageUrl { get; set; }

        public Artist Artist { get; set; }
        public int ArtistId { get; set; }

    }
}