using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;
using Airline;

namespace Airline.Models
{
  public class Flight
  {
    private int _id;
    private int _departureTime;
    private string _arrivalCity;
    private string _flightStatus;

    public Flight (int departureTime, string arrivalCity, string flightStatus, int id = 0)
    {
      _id = id;
      _departureTime = departureTime;
      _arrivalCity = arrivalCity;
      _flightStatus = flightStatus;

    }
    public int GetId()
    {
      return _id;
    }
    public int GetDepartureTime()
    {
      return _departureTime;
    }
    public string GetArrivalCity()
    {
      return _arrivalCity;
    }
    public string GetFlightStatus()
    {
      return _flightStatus;
    }

    public override bool Equals(System.Object otherFlight)
    {
      if (!(otherFlight is Flight))
      {
        return false;
      }
      else
      {
        Flight newFlight = (Flight) otherFlight;
        bool idEquality = (this.GetId() == newFlight.GetId());
        bool descriptionEquality = (this.GetArrivalCity() == newFlight.GetArrivalCity());
        return (idEquality && descriptionEquality);
      }
    }

    public static List<Flight> GetAll()
    {
      List<Flight> allFlights = new List<Flight> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM flights;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int FlightId = rdr.GetInt32(0);
        int FlightDepartureTime = rdr.GetInt32(1);
        string FlightArrivalCity = rdr.GetString(2);
        string FlightFlightStatus = rdr.GetString(3);
        Flight newFlight = new Flight(FlightDepartureTime, FlightArrivalCity, FlightFlightStatus, FlightId);
        allFlights.Add(newFlight);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allFlights;
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO flights (departure_time, arrival_city, flight_status) VALUES (@departureTime, @arrivalCity, @flightStatus);";
      MySqlParameter departure_time = new MySqlParameter();
      departure_time.ParameterName = "@departureTime";
      departure_time.Value = this._departureTime;
      cmd.Parameters.Add(departure_time);
      MySqlParameter arrival_city = new MySqlParameter();
      arrival_city.ParameterName = "@arrivalCity";
      arrival_city.Value = this._arrivalCity;
      cmd.Parameters.Add(arrival_city);
      MySqlParameter flight_status = new MySqlParameter();
      flight_status.ParameterName = "@flightStatus";
      flight_status.Value = this._flightStatus;
      cmd.Parameters.Add(flight_status);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

    }
    public static Flight Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM flights WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int FlightId = 0;
      int FlightDepartureTime = 0;
      string FlightArrivalCity = "";
      string FlightFlightStatus = "";

      while (rdr.Read())
      {
        FlightId = rdr.GetInt32(0);
        FlightDepartureTime = rdr.GetInt32(1);
        FlightArrivalCity = rdr.GetString(2);
        FlightFlightStatus = rdr.GetString(3);
      }

      Flight foundFlight = new Flight(FlightDepartureTime, FlightArrivalCity, FlightFlightStatus, FlightId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundFlight;
    }
    public List<City> GetCities()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT cities.* FROM flights
          JOIN cities_flights ON (flights.id = cities_flights.flight_id)
          JOIN cities ON (cities_flights.city_id = cities.id)
          WHERE flights.id = @FlightId;";

      MySqlParameter flightIdParameter = new MySqlParameter();
      flightIdParameter.ParameterName = "@FlightId";
      flightIdParameter.Value = _id;
      cmd.Parameters.Add(flightIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<City> cities = new List<City>{};


      while(rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        string cityDescription = rdr.GetString(1);
        City newCity = new City(cityDescription, cityId);
        cities.Add(newCity);
      }
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return cities;
    }
    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM flights;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }
    public void AddCity(City newCity)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT into cities_flights (city_id, flight_id) VALUES (@CityId, @FlightId);";

      MySqlParameter city_id = new MySqlParameter();
      city_id.ParameterName = "@CityId";
      city_id.Value = newCity.GetId();
      cmd.Parameters.Add(city_id);

      MySqlParameter flight_id = new MySqlParameter();
      flight_id.ParameterName = "@FlightId";
      flight_id.Value = _id;
      cmd.Parameters.Add(flight_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public void Delete()
   {
     MySqlConnection conn = DB.Connection();
     conn.Open();
     var cmd = conn.CreateCommand() as MySqlCommand;
     cmd.CommandText = @"DELETE FROM flights WHERE id = @FlightId; DELETE FROM cities_flights WHERE flight_id = @FlightId;";

     MySqlParameter flightIdParameter = new MySqlParameter();
     flightIdParameter.ParameterName = "@FlightId";
     flightIdParameter.Value = this.GetId();
     cmd.Parameters.Add(flightIdParameter);

     cmd.ExecuteNonQuery();
     if (conn != null)
     {
       conn.Close();
     }
   }
  }
}
