using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Reggie.Data;
using Reggie.ViewModels;
using Reggie.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;


namespace Reggie.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
     
        }

        //Add Allow Anonymous to the home controller so anonymous users can get information about the site before they register.
        [ResponseCache(CacheProfileName = "HalfDaily")]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            List<NewsfeedViewModel> newsfeedList = new List<NewsfeedViewModel>();
            DateTime currentDateTime = DateTime.Now;
            var listData = await (from n in _context.Insurances
                            select new
                            {
                                n.Id,
                                n.Payor,
                                n.Plan,
                                n.News,
                                n.NewsfeedStartDate,
                                n.NewsfeedEndDate,
                                n.ReggiePublishStartDate,
                                n.ReggiePublishEndDate
 
                            }
                          ).ToListAsync();
            listData.ForEach(x =>
            {
                NewsfeedViewModel Obj = new NewsfeedViewModel();

                if (x.NewsfeedStartDate == null || x.ReggiePublishStartDate == null)
                {
                    Obj.IsNewsReleased = false;
                } 
                else if (x.NewsfeedStartDate > currentDateTime || x.ReggiePublishStartDate > currentDateTime || x.NewsfeedEndDate < currentDateTime || x.ReggiePublishEndDate < currentDateTime || String.IsNullOrEmpty(x.News))
                {
                    Obj.IsNewsReleased = false;
                }
                else
                {
                    Obj.IsNewsReleased = true;
                    Obj.Id = x.Id;
                    Obj.Payor = x.Payor;
                    Obj.Plan = x.Plan;
                    Obj.News = x.News;
                    newsfeedList.Add(Obj);
                }
            });
            return View(newsfeedList);
        }

        [ResponseCache(CacheProfileName = "Daily")] //Daily
        public IActionResult About()
        {

            return View();
        }


        [ResponseCache(CacheProfileName = "Daily")] //Daily
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
