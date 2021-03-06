﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xfy.GraduationPhoto.Web.Models;

namespace Xfy.GraduationPhoto.Web.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 应用程序入口页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Main()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 回顾
        /// </summary>
        /// <returns></returns>
        public IActionResult Forward()
        {
            return View();
        }

        /// <summary>
        /// 公用头部
        /// </summary>
        /// <returns></returns>
        public IActionResult _Header()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
