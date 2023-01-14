using DAL.Model;
using DAL.Repositories;

namespace DotNet7MinimalApi.Extensions
{
    public static class MapApis
    {
        public static void MapApi(this WebApplication app)
        {
            // WeatherForecast

            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            app.MapGet("/weatherforecast", () =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]
                    ))
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi();

            // Person
            app.MapGet("/api/person", async (IPersonRepository personRepo) =>
            {
                var people = await personRepo.GetAll();
                return Results.Ok(people);
            });

            app.MapPost("/api/person", async (IPersonRepository personRepo, Person person) =>
            {
                var result = await personRepo.Add(person);
                if (result)
                {
                    return Results.Ok("Saved Successfully");
                }
                return Results.Problem();
            });


            app.MapPut("/api/person", async (IPersonRepository personRepo, Person person) =>
            {
                var result = await personRepo.Update(person);
                if (result)
                {
                    return Results.Ok("Updated Successfully");
                }
                return Results.Problem();
            });

            app.MapGet("/api/person/{id}", async (int id, IPersonRepository personRepo) =>
            {
                var person = await personRepo.GetById(id);
                if (person is null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(person);
            });



            app.MapDelete("/api/person/{id}", async (IPersonRepository personRepo, int id) =>
            {
                var result = await personRepo.Delete(id);
                if (result)
                {
                    return Results.Ok("Deleted Successfully");
                }
                return Results.Problem();
            });

        }


        internal record WeatherForecast(DateOnly Date, int TemperatureC, string Summary)
        {
            public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        }

    }
}
