using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Areas.Admin.Controllers;
/*
 * Create 
 * 1. Translations
 * 2. TenantInfo (for editing info, customizing email templates, page SEO and headers)
 * 3. Add role based auth to all controllers
 */
[Area("Admin")]
public class HomeController : Controller
{
    // GET: /<controller>/
    public IActionResult Index()
    {
        return View();
    }
}
