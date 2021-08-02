using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MainForm.Models;
using MainForm.Models.Setting;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MainForm.Controllers
{
    public class SettingController : Controller
    {
        public IActionResult Machine()
        {
            MachineContext context = HttpContext.RequestServices.GetService(typeof(MachineContext)) as MachineContext;
            return View();
            //return View(context.GetAllAlbums());/
        }

        public IActionResult Repository()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        // POST: Positions/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create([Bind("Gr_name,Gr_id")] MaterialGroup group)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Context.Creat(group);
        //        //Context.SaveChanges();
        //    }
        //    return View();
        //}
        
        
    }
}
