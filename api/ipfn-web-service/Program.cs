using ipfn_web_service.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        cors => cors.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Postcode API", Version = "v1" });
});

var app = builder.Build();

// Enable middleware to serve Swagger UI and OpenAPI docs
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Postcode API v1");
    });
}

// Use CORS middleware
app.UseCors("AllowAnyOrigin");

app.MapGet("/", () => "Welcome to Postcode API!");

app.MapGet("/autocomplete/{partialPostcode}", async (string partialPostcode) =>
{
    var client = new HttpClient();
    var response = await client.GetAsync($"https://api.postcodes.io/postcodes/{partialPostcode}/autocomplete");
    
    if (!response.IsSuccessStatusCode)
        return Results.Problem("Error communicating with Postcodes.io API");
    
    var autocompleteResult = await response.Content.ReadFromJsonAsync<object>();
    return Results.Ok(autocompleteResult);
});

app.MapGet("/lookup/{postcode}", async (string postcode) =>
{
    var client = new HttpClient();
    var response = await client.GetAsync($"https://api.postcodes.io/postcodes/{postcode}");
    
    if (!response.IsSuccessStatusCode)
        return Results.Problem("Error communicating with Postcodes.io API");
    
    var lookupResult = await response.Content.ReadFromJsonAsync<PostcodeResponse>();
    
    
    

    if (lookupResult?.Result == null)
        return Results.Problem("No data found for the provided postcode");

    // Extract the latitude from the response
    var latitude = lookupResult.Result.Latitude;
    
    // Determine the "Area" based on latitude
    var area = latitude switch
    {
        < 52.229466 => "South",
        >= 52.229466 and < 53.27169 => "Midlands",
        >= 53.27169 => "North",
        _ => "Unknown"
    };
    
    // Add the "Area" to the response
    var resultWithArea = new
    {
        PostcodeData = lookupResult.Result,
        Area = area
    };
    
    return Results.Ok(resultWithArea);
});

app.Run();
