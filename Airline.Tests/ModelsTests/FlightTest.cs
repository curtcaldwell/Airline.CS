using Microsoft.VisualStudio.TestTools.UnitTesting;
using Airline.Models;
using System;
using System.Collections.Generic;

namespace Airline.Tests
{
  [TestClass]
  public class FlightTests : IDisposable
  {
    public FlightTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=Airline_test;";
    }
    public void Dispose()
    {
      Flight.DeleteAll();
    }
    [TestMethod]
    public void GetAll_DbStartsEmpty_0()
    {
      //Arrange
      //arrivalCity
      int result = Flight.GetAll().Count;

      //Assert
      Assert.AreEqual(0,result);
    }
    [TestMethod]
    public void Equals_TrueForSameDepartureCity_Item()
    {
      //Arrange, Act
      Flight firstFlight = new Flight (1, "Seattle", "", "");
      Flight secondFlight = new Flight (1, "Seattle", "", "");

      //Assert
      Assert.AreEqual(firstFlight, secondFlight);
    }
    [TestMethod]
    public void HowToComputer()
    {
      //Arrange
      Flight firstFlight = new Flight(2, "North Korea", "", "");
      firstFlight.Save();
      //Act
      List<Flight> results = Flight.GetAll();
      List<Flight> testList = new List<Flight>{firstFlight};
      //Assert
      CollectionAssert.AreEqual(testList, results);
    }
    [TestMethod]
    public void Find_FindsFlightInDataBase_Flight()
    {
      //Arrange
      Flight testFlight = new Flight(420, "LALALAND", "", "");
      testFlight.Save();

      //Act
      Flight result = Flight.Find(testFlight.GetId());

      //Assert
      Assert.AreEqual(testFlight, result);
    }
    [TestMethod]
    public void AddCity_AddsCitytoFlight_CityList()
    {
      //Arrange
      Flight testFlight = new Flight(69, "Moscow", "", "");
      testFlight.Save();

      City testCity = new City("Moscow");
      testCity.Save();

      //Act
      testFlight.AddCity(testCity);

      List<City> result = testFlight.GetCities();
      List<City> testList = new List<City>{testCity};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

  }
}
