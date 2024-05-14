using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Xml.Linq;
using System.Xml;
using CurrencyCalculator.Models.Entities;
using CurrencyCalculator.Models.ViewModels;
using CurrencyCalculator.Extensions;

namespace CurrencyCalculator.Controllers
{
    public class CalculatorController : Controller
    {        
        public IActionResult Index(CalculatorViewModel model)
        {
            return View(model);
        }
                
    }
}
