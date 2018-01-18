using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reggie.Models;


namespace Reggie.ViewModels
{
    public class SearchResultsViewModel
    {

        public int Id { get; set; }
        public string Payor { get; set; }
        public string Plan { get; set; }
        public string Keywords { get; set; }
        public bool IsReggiePublished { get; set; }
        public bool IsPlanActive { get; set; }


    }
}