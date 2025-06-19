using Microsoft.EntityFrameworkCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<UrlDbContext>(options => 
    options.UseInMemoryDatabase("UrlShortenerDb"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// API logic
// This helper function creates a random 6-character string for our short URL.
string GenerateShortCode()
{
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    var random = new Random();
    var code = new StringBuilder(6);
    for (int i = 0; i < 6; i++)
    {
        code.Append(chars[random.Next(chars.Length)]);
    }
    return code.ToString();
}

app.MapPost("/shorten", async (ShortenUrlRequest request, UrlDbContext db, HttpContext httpContext) =>
{
    // Checking if the URL is valid or not.
    if (string.IsNullOrWhiteSpace(request.Url) || !Uri.TryCreate(request.Url, UriKind.Absolute, out _))
    {
        return Results.BadRequest("A valid URL must be provided.");
    }

    // Create a unique short code.
    var shortCode = GenerateShortCode();

    // Create the new mapping to save in our database.
    var urlMapping = new UrlMapping
    {
        Id = shortCode,
        OriginalUrl = request.Url
    };

    // Add it to the database and save the changes.
    db.UrlMappings.Add(urlMapping);
    await db.SaveChangesAsync();

    // Build the full short URL to send back to the user (e.g., "https://localhost:7171/aB1xYz")
    var resultUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/{shortCode}";

    return Results.Ok(new { ShortUrl = resultUrl });
});

// Endpoint 2: Redirect to the original URL
// This listens for GET requests and uses the path as the short code.
app.MapGet("/{shortCode}", async (string shortCode, UrlDbContext db) =>
{
    // Find the corresponding mapping in the database.
    var urlMapping = await db.UrlMappings.FindAsync(shortCode);

    // If we find it, redirect the user to the original URL.
    if (urlMapping is not null)
    {
        // The 308 status code tells the browser this is a permanent redirect.
        return Results.Redirect(urlMapping.OriginalUrl, permanent: true);
    }

    // If the code doesn't exist in our database, return a "Not Found" error.
    return Results.NotFound();
});


app.Run();

// Defines the data I'm storing for each link.
public class UrlMapping
{
    public string Id { get; set; } = string.Empty; 
    public string OriginalUrl { get; set; } = string.Empty;
}

// This is how the app talks to the database.
public class UrlDbContext : DbContext
{
    public UrlDbContext(DbContextOptions<UrlDbContext> options) : base(options) { }

    public DbSet<UrlMapping> UrlMappings { get; set; }
}


// class for the request we get from the user

public class ShortenUrlRequest
{
    public string Url { get; set; } = string.Empty;
 }