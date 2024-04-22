using System.Data;
using Cw6.Models;
using Cw6.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Cw6.Controllers;

[ApiController]
// [Route("api/animals")]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public AnimalsController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult GetAnimals(string par)
    {
        //otwieramy polaczenie
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        // definicja command
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM Animal ORDER BY=" + par;
        
        // wykonanie zapytania
        var reader = command.ExecuteReader();

        List<Animal> animals = new List<Animal>();

        int idAnimalOrdinal = reader.GetOrdinal("IdAnimal");
        int nameOrdinal = reader.GetOrdinal("Name");
        int descriptionOrdinal = reader.GetOrdinal("Description");
        int categoryOrdinal = reader.GetOrdinal("Category");
        int areaOrdinal = reader.GetOrdinal("Area");
        

        while (reader.Read())
        {
            animals.Add(new Animal()
            {
                IdAnimal = reader.GetInt32(idAnimalOrdinal),
                Name  = reader.GetString(nameOrdinal),
                Description = reader.GetString(descriptionOrdinal),
                Category = reader.GetString(categoryOrdinal),
                Area = reader.GetString(areaOrdinal)
            });
        }

        return Ok(animals);
    }

    [HttpPost]
    public IActionResult AddAnimal(AddAnimalRequest addAnimalRequest)
    {
        //otwieramy polaczenie
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        // definicja command
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "INSERT INTO Animal VALUES(@animalName,@animalDescription,@animalCategory,@animalArea)";
        command.Parameters.AddWithValue("@animalName", addAnimalRequest.Name);
        command.Parameters.AddWithValue("@animalDescription", addAnimalRequest.Description);
        command.Parameters.AddWithValue("@animalCategory", addAnimalRequest.Category);
        command.Parameters.AddWithValue("@animalArea", addAnimalRequest.Area);
        
        // wykonanie
        command.ExecuteNonQuery();
        
        return Created();
    }
    
    [HttpPut]
    public IActionResult UpdateAnimal(int idAnimal, UpdateAnimalRequest updateAnimalRequest)
    {
        //otwieramy połączenie
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        //definicja komendy
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "UPDATE Animal SET Description=@animalDescription, Category=@animalCategory, Area=@animalArea WHERE IdAnimal=@idAnimal";
        command.Parameters.AddWithValue("@animalName", updateAnimalRequest.Name);
        command.Parameters.AddWithValue("@animalDescription", updateAnimalRequest.Description);
        command.Parameters.AddWithValue("@animalCategory", updateAnimalRequest.Category);
        command.Parameters.AddWithValue("@animalArea", updateAnimalRequest.Area);
        command.Parameters.AddWithValue("@idAnimal", idAnimal);

        //wykonanie
        command.ExecuteNonQuery();

        return NoContent();
    }
    
    [HttpDelete("{idAnimal}")]
    public IActionResult DeleteAnimal(int idAnimal)
    {
        //otwieramy połączenie
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        //definicja komendy
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "DELETE FROM Animal WHERE IdAnimal=@idAnimal";
        command.Parameters.AddWithValue("@idAnimal", idAnimal);

        //wykonanie
        command.ExecuteNonQuery();

        return NoContent();
    }
}