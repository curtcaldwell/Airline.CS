using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;
using Airline;

namespace Airline.Models
{
  public class City
  {
    private int _id;
    private string _city;
    public City (string city, int id = 0)
    {
      _id = id;
      _city = city;

    }
    public int GetId()
    {
      return _id;
    }
    public string GetCity()
    {
      return _city;
    }
    public override bool Equals(System.Object otherCity)
    {
      if (!(otherCity is City))
      {
        return false;
      }
      else
      {
        City newCity = (City) otherCity;
        return this.GetId().Equals(newCity.GetId());
      }
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO cities (city) VALUES (@city);";

      MySqlParameter city = new MySqlParameter();
      city.ParameterName = "@city";
      city.Value = this._city;
      cmd.Parameters.Add(city);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public static List<City> GetAll()
    {
      List<City> allCities = new List<City> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cities;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int CityId = rdr.GetInt32(0);
        string CityCity = rdr.GetString(1);
        City newCity = new City(CityCity, CityId);
        allCities.Add(newCity);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCities;
    }
    public static City Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cities WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int CityId = 0;
      string CityName = "";

      while(rdr.Read())
      {
        CityId = rdr.GetInt32(0);
        CityName = rdr.GetString(1);
      }
      City newCity = new City(CityName, CityId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newCity;
    }

    public List<Flight> GetFlights()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT flights.* FROM cities
                JOIN cities_flights ON (cities.id = cities_flights.category_id)
                JOIN flights ON (cities_flights.flight_id = flights.id)
                WHERE cities.id = @CityId;";

            MySqlParameter cityIdParameter = new MySqlParameter();
            cityIdParameter.ParameterName = "@CityId";
            cityIdParameter.Value = _id;
            cmd.Parameters.Add(cityIdParameter);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Flight> flights = new List<Flight>{};

            while(rdr.Read())
            {
              int FlightId = rdr.GetInt32(0);
              int FlightDepartureTime = rdr.GetInt32(1);
              string FlightDepartureCity = rdr.GetString(2);
              string FlightArrivalCity = rdr.GetString(3);
              string FlightFlightStatus = rdr.GetString(4);
              Flight newFlight = new Flight(FlightDepartureTime, FlightDepartureCity, FlightArrivalCity, FlightFlightStatus, FlightId);
              flights.Add(newFlight);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return flights;
        }
    public void AddFlight(Flight newFlight)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO cities_items (category_id, flight_id) VALUES (@CityId, @FlightId);";

            MySqlParameter category_id = new MySqlParameter();
            category_id.ParameterName = "@CityId";
            category_id.Value = _id;
            cmd.Parameters.Add(category_id);

            MySqlParameter flight_id = new MySqlParameter();
            flight_id.ParameterName = "@FlightId";
            flight_id.Value = newFlight.GetId();
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

      MySqlCommand cmd = new MySqlCommand("DELETE FROM cities WHERE id = @CityId; DELETE FROM cities_items WHERE category_id = @CityId;", conn);
      MySqlParameter cityIdParameter = new MySqlParameter();
      cityIdParameter.ParameterName = "@CityId";
      cityIdParameter.Value = this.GetId();

      cmd.Parameters.Add(cityIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM cities;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
