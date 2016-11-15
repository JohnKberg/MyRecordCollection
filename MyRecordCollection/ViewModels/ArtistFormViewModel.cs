using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyRecordCollection.ViewModels
{
    public class ArtistFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public string Bio { get; set; }


        public String PageTitle 
        { 
            get { return this.Id != 0 ? "Edit Artist" : "Add Artist"; } 
        }
    }
}