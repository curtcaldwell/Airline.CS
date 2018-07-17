using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Airline.Models;

namespace Airline.Controllers
{
  public class FlightController : Controller
  {

    [HttpGet("/flights")]
    public ActionResult Index()
    {
      List<Flight> allFlights = Flight.GetAll();
      return View(allFlights);
    }
    [HttpGet("/flights/new")]
    public ActionResult CreateForm()
    {
      return View(City.GetAll());
    }
    [HttpPost("/flights")]
    public ActionResult Create()
    {
      Flight newFlight = new Flight(int.Parse(Request.Form["depart-time"]), Request.Form["arrival-city"], Request.Form["flight-status"]);
      newFlight.Save();
      return RedirectToAction("Index", Flight.GetAll());
    }
    [HttpGet("/flights/{id}")]
    public ActionResult Details(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Flight flightDestination = Flight.Find(id);
      List<City> departureCity = flightDestination.GetCities();
      List<City> allCities = City.GetAll();
      model.Add("flightDestination", flightDestination);
      model.Add("departureCity", departureCity);
      model.Add("allCities", allCities);
      return View(model);
    }
    [HttpPost("/flights/{flightId}/cities/new")]
    public ActionResult AddCity(int flightId)
    {
      Flight flight = Flight.Find(flightId);
      City city = City.Find(int.Parse(Request.Form["city-id"]));
      flight.AddCity(city);
      return RedirectToAction("Details", new { id = flightId});
    }
  }
}
