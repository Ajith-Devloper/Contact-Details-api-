using ContactDetails_Api.Custom_Execption_Filtter;
using ContactDetails_Api.Entity;
using ContactDetails_Api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContactDetails_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [CustomExpextion]
    public class ContactDat_Controller : ControllerBase
    {
        private readonly ILogger<ContactDat_Controller> _logger;
        private readonly Icontact_Repository contact_repository;
        private readonly IConfiguration _configuration;
        public ContactDat_Controller(Icontact_Repository icontact_Repository, IConfiguration configuration,ILogger<ContactDat_Controller> logger)
        {
            this.contact_repository = icontact_Repository;
            this._configuration = configuration;
            this._logger = logger;

        }

        [HttpGet]

        public async Task<IActionResult> GetallDetails()
        {

            _logger.LogInformation("Fetching all contact details.");
            try
            {
                var contacts = await contact_repository.GetAll_Details();
                _logger.LogInformation("Successfully fetched {Count} contacts.", contacts?.Count() ?? 0);
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all contacts.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetByid(int id)
        {
           
            _logger.LogInformation("Fetching contact details for ID: {Id}", id);
            try
            {
                var contact = await contact_repository.GetById_Details(id);
                if (contact == null)
                {
                    _logger.LogWarning("Contact with ID {Id} not found.", id);
                    return NotFound(new { Message = $"Contact with ID {id} not found." });
                }
                _logger.LogInformation("Contact found: {@Contact}", contact);
                return Ok(contact);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching contact with ID: {Id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]

        public async Task<IActionResult> Create([FromBody] ContactDto contactDto)
        {
            _logger.LogInformation("Creating new contact: {@ContactDto}", contactDto);
            try
            {
                var contact = new Contact
                {
                    FirstName = contactDto.FirstName,
                    LastName = contactDto.LastName,
                    Email = contactDto.Email,
                    PhoneNumber = contactDto.PhoneNumber,
                    Address = contactDto.Address,
                    City = contactDto.City,
                    State = contactDto.State,
                    Country = contactDto.Country,
                    PostalCode = contactDto.PostalCode
                };

                await contact_repository.Add_Details(contact);
                _logger.LogInformation("Contact created successfully with ID: {Id}", contact.Id);
                return Ok(contact);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating contact.");
                return StatusCode(500, "Internal Server Error");
            }
        }

            [HttpPut("{id:int}")]

        public async Task<IActionResult> Update(int id, ContactDto contactDto)
        {
            _logger.LogInformation("Updating contact with ID: {Id}", id);
            try
            {
                var contact = await contact_repository.GetById_Details(id);
                if (contact == null)
                {
                    _logger.LogWarning("Contact with ID {Id} not found for update.", id);
                    return NotFound(new { Message = $"Contact with ID {id} not found." });
                }

                contact.FirstName = contactDto.FirstName;
                contact.LastName = contactDto.LastName;
                contact.Email = contactDto.Email;
                contact.PhoneNumber = contactDto.PhoneNumber;
                contact.Address = contactDto.Address;
                contact.City = contactDto.City;
                contact.State = contactDto.State;
                contact.Country = contactDto.Country;
                contact.PostalCode = contactDto.PostalCode;

                await contact_repository.Update_Details(contact);
                _logger.LogInformation("Contact with ID {Id} updated successfully.", id);
                return Ok("Updated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating contact with ID: {Id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id:int}")]

        public async Task<IActionResult> Remove(int id)

        {
            _logger.LogInformation("Deleting contact with ID: {Id}", id);
            try
            {
                var contact = await contact_repository.GetById_Details(id);
                if (contact == null)
                {
                    _logger.LogWarning("Contact with ID {Id} not found for deletion.", id);
                    return NotFound($"Contact with ID {id} not found.");
                }

                await contact_repository.Delete_Details(id);
                _logger.LogInformation("Contact with ID {Id} deleted successfully.", id);
                return Ok($"{id} ID is deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting contact with ID: {Id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }


        [AllowAnonymous]
        [HttpPost("Login")]
            public async Task<IActionResult> Login([FromBody]LoginDto logindto)
            {


                if (logindto == null || string.IsNullOrWhiteSpace(logindto.FirstName) || string.IsNullOrWhiteSpace(logindto.Email))
                {
                    return BadRequest("Invalid input. Firstname and Email are required.");
                }

                var user = await contact_repository.Login(logindto.FirstName, logindto.Email);

                if (user == null)
                {
                    return NotFound("User not found with the provided Firstname and Email.");
                }

                var token = GenerateJwtToken(user.FirstName, user.Email);

                return Ok(new JwtS { Token = token });
            }
     

    private string GenerateJwtToken(string firstname, string email)
    {

        var claims = new[]
        {
          new Claim(ClaimTypes.Name, firstname),
         new Claim(ClaimTypes.Email, email),
         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
     };


        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );


        return new JwtSecurityTokenHandler().WriteToken(token);
    }


}
   
}
