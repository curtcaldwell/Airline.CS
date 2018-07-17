using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Airline.Models;

namespace Airline.Controllers
{
  public class CityController : Controller
  {
    [HttpGet("/cities")]
    public ActionResult Index()
    {
      List<City> allCities = City.GetAll();
      return View(allCities);
    }
    [HttpGet("/cities/new")]
    public ActionResult CreateForm()
    {
      return View();
    }
    [HttpPost("/cities")]
    public ActionResult Create()
    {
      City newCity = new City(Request.Form["new-city"]);
      newCity.Save();
      return RedirectToAction("Index", City.GetAll());
    }
    [HttpGet("/cities/{id}")]
    public ActionResult Details(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      City departureCity = City.Find(id);
      List<Flight> flightDestination = departureCity.GetFlights();
      List<Flight> allDestinations = Flight.GetAll();
      model.Add("departureCity", departureCity);
      model.Add("flightDestination", flightDestination);
      model.Add("allDestinations", allDestinations);
      return View(model);
    }
    [HttpPost("/cities/{cityId}/flights/new")]
    public ActionResult AddFlight(int cityId)
    {
      City city = City.Find(cityId);
      Flight flight = Flight.Find(int.Parse(Request.Form["flight-id"]));
      city.AddFlight(flight);
      return RedirectToAction("Details", new { id = cityId});
    }
  }
}
