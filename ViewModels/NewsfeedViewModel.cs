using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reggie.Models;


namespace Reggie.ViewModels
{
    public class NewsfeedViewModel
    {

        public int Id { get; set; }
        public string Payor { get; set; }
        public string Plan { get; set; }
        public string News { get; set; }
        public bool IsNewsReleased { get; set; }

    }
}