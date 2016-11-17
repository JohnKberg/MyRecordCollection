using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyRecordCollection.Models;

namespace MyRecordCollection.ViewModels
{
    public class HomePageViewModel
    {
        public string PageTitle { get; set; }
        public string PageSubTitle { get; set; }

        public IEnumerable<Album> Albums { get; set; }
    }
}