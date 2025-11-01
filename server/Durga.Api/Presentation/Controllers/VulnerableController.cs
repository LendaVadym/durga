using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Xml;
using System.Text;
using System.Security.Cryptography;

namespace Durga.Api.Presentation.Controllers;

/// <summary>
/// WARNING: This controller contains intentional security vulnerabilities for educational purposes only.
/// DO NOT use this code in production environments.
/// Demonstrates OWASP Top 10 vulnerabilities.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class VulnerableController : ControllerBase
{
    private readonly IConfiguration _configuration;
    
    public VulnerableController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // 1. A01:2021 – Broken Access Control
    [HttpGet("user/{userId}/profile")]
    public IActionResult GetUserProfile(int userId)
    {
        // Vulnerability: No authentication or authorization check
        // Any user can access any other user's profile
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        
        var command = new SqlCommand($"SELECT * FROM Users WHERE Id = {userId}", connection);
        var reader = command.ExecuteReader();
        
        if (reader.Read())
        {
            return Ok(new { 
                Id = reader["Id"], 
                Email = reader["Email"],
                Password = reader["Password"], // Exposing password hash
                SSN = reader["SSN"] // Exposing sensitive data
            });
        }
        
        return NotFound();
    }

    // 2. A02:2021 – Cryptographic Failures
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Vulnerability: Storing password in plain text and weak hashing
        var passwordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.Password)); // Weak encoding, not hashing
        
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        
        // SQL Injection vulnerability combined with cryptographic failure
        var query = $"SELECT * FROM Users WHERE Username = '{request.Username}' AND Password = '{passwordHash}'";
        var command = new SqlCommand(query, connection);
        var reader = command.ExecuteReader();
        
        if (reader.Read())
        {
            // Sending sensitive data without encryption
            return Ok(new { 
                Token = Guid.NewGuid().ToString(), // Predictable token
                CreditCard = reader["CreditCard"] // Sending unencrypted sensitive data
            });
        }
        
        return Unauthorized();
    }

    // 3. A03:2021 – Injection (SQL Injection)
    [HttpGet("search")]
    public IActionResult SearchUsers([FromQuery] string searchTerm)
    {
        // Vulnerability: Direct SQL injection
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        
        var query = $"SELECT * FROM Users WHERE Username LIKE '%{searchTerm}%' OR Email LIKE '%{searchTerm}%'";
        var command = new SqlCommand(query, connection);
        var reader = command.ExecuteReader();
        
        var results = new List<object>();
        while (reader.Read())
        {
            results.Add(new { 
                Username = reader["Username"], 
                Email = reader["Email"] 
            });
        }
        
        return Ok(results);
    }

    // 4. A04:2021 – Insecure Design
    [HttpPost("reset-password")]
    public IActionResult ResetPassword([FromBody] PasswordResetRequest request)
    {
        // Vulnerability: No verification of user identity, predictable reset mechanism
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        
        // Direct password reset without verification
        var query = $"UPDATE Users SET Password = '{request.NewPassword}' WHERE Email = '{request.Email}'";
        var command = new SqlCommand(query, connection);
        command.ExecuteNonQuery();
        
        return Ok(new { Message = "Password reset successful" });
    }

    // 5. A05:2021 – Security Misconfiguration
    [HttpGet("config")]
    public IActionResult GetConfiguration()
    {
        // Vulnerability: Exposing sensitive configuration data
        return Ok(new {
            DatabaseConnection = _configuration.GetConnectionString("DefaultConnection"),
            ApiKey = _configuration["ApiKey"],
            SecretKey = _configuration["SecretKey"],
            Debug = true,
            StackTrace = Environment.StackTrace
        });
    }

    // 6. A06:2021 – Vulnerable and Outdated Components
    [HttpPost("parse-xml")]
    public IActionResult ParseXml([FromBody] string xmlContent)
    {
        // Vulnerability: XXE (XML External Entity) attack
        var xmlDoc = new XmlDocument();
        xmlDoc.XmlResolver = new XmlUrlResolver(); // Enables external entity resolution
        xmlDoc.LoadXml(xmlContent);
        
        return Ok(new { Result = xmlDoc.InnerText });
    }

    // 7. A07:2021 – Identification and Authentication Failures
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        // Vulnerabilities: No password complexity requirements, no rate limiting
        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest("Username and password required");
        }
        
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        
        // Storing password in plain text
        var query = $"INSERT INTO Users (Username, Password) VALUES ('{request.Username}', '{request.Password}')";
        var command = new SqlCommand(query, connection);
        command.ExecuteNonQuery();
        
        // No session management, weak authentication
        return Ok(new { Message = "User registered", SessionId = "12345" });
    }

    // 8. A08:2021 – Software and Data Integrity Failures
    [HttpPost("upload")]
    public IActionResult UploadFile([FromForm] IFormFile file)
    {
        // Vulnerability: No validation of file type, content, or integrity
        var filePath = Path.Combine("/tmp", file.FileName); // Directory traversal possible
        
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }
        
        // Executing uploaded file without validation
        return Ok(new { Message = "File uploaded", Path = filePath });
    }

    // 9. A09:2021 – Security Logging and Monitoring Failures
    [HttpPost("admin/delete-user")]
    public IActionResult DeleteUser([FromQuery] int userId)
    {
        // Vulnerability: No logging of critical operations, no monitoring
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        
        var query = $"DELETE FROM Users WHERE Id = {userId}";
        var command = new SqlCommand(query, connection);
        command.ExecuteNonQuery();
        
        // Critical operation with no audit trail
        return Ok(new { Message = "User deleted" });
    }

    // 10. A10:2021 – Server-Side Request Forgery (SSRF)
    [HttpGet("fetch-url")]
    public async Task<IActionResult> FetchUrl([FromQuery] string url)
    {
        // Vulnerability: SSRF - allows fetching arbitrary URLs including internal resources
        using var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(30);
        
        try
        {
            var response = await httpClient.GetStringAsync(url);
            return Ok(new { Content = response });
        }
        catch (Exception ex)
        {
            // Vulnerability: Exposing internal error details
            return BadRequest(new { 
                Error = ex.Message, 
                StackTrace = ex.StackTrace,
                InnerException = ex.InnerException?.Message
            });
        }
    }

    // Additional: Command Injection
    [HttpPost("execute")]
    public IActionResult ExecuteCommand([FromBody] CommandRequest request)
    {
        // Vulnerability: Command injection
        var process = new System.Diagnostics.Process();
        process.StartInfo.FileName = "/bin/bash";
        process.StartInfo.Arguments = $"-c \"{request.Command}\"";
        process.StartInfo.RedirectStandardOutput = true;
        process.Start();
        
        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        
        return Ok(new { Output = output });
    }

    // Additional: Path Traversal
    [HttpGet("read-file")]
    public IActionResult ReadFile([FromQuery] string fileName)
    {
        // Vulnerability: Path traversal attack
        var filePath = Path.Combine("/app/files", fileName);
        
        if (System.IO.File.Exists(filePath))
        {
            var content = System.IO.File.ReadAllText(filePath);
            return Ok(new { Content = content });
        }
        
        return NotFound();
    }
}

// Request models
public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class PasswordResetRequest
{
    public string Email { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

public class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class CommandRequest
{
    public string Command { get; set; } = string.Empty;
}
