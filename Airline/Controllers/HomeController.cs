using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Airline.Models;

namespace Airline.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }
  }
}
