using Api.Models;
using System.Text.Json.Serialization;

namespace Api.Dtos.Dependent;

public class GetDependentDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Relationship Relationship { get; set; }
}
