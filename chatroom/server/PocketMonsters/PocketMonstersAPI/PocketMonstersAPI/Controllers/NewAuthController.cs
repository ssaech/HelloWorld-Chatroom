using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using Dapper;
using System.Web.Http.Cors;
using PocketMonstersAPI.Models;


namespace PocketMonstersAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("api/[controller]")]
    [ApiController]
    public class NewAuthController : ControllerBase
    {
        public static User user = new User();

        private readonly IConfiguration _config;
        private readonly IUserService _userService;

        public NewAuthController(IConfiguration config, IUserService userService)
        {
            _config = config;
            _userService = userService;

        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt, out byte[] passwordHashSalt);

            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await connection.ExecuteAsync("INSERT INTO dbo.Login4 (Username, PasswordHash ,PasswordSalt) " +
                                          "values(@Username, @PasswordHash, @PasswordSalt)", user);

            return Ok(user);

        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            var pokemon = await connection.QueryFirstAsync<User>("select * from Login4 where Username=@Username", new { Username = request.Username });

            if (pokemon.Username != request.Username)
            {
                return BadRequest("User not found.");
            }

            if (!VerifyPasswordHash(request.Password, pokemon.PasswordHash, pokemon.PasswordSalt))
            {
                return BadRequest("No password");
            }

            string token = CreateToken(pokemon);
            return Ok(token);
        }


        [HttpGet]
        public async Task<ActionResult<List<NewAuth>>> GetAllPokemon()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<NewAuth> pokemons = await SelectAllPokemons(connection);
            return Ok(pokemons);
        }

        private static async Task<IEnumerable<NewAuth>> SelectAllPokemons(SqlConnection connection)
        {
            return await connection.QueryAsync<NewAuth>("select * from pocketmonsters");
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt, out byte[] passwordHashSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                passwordHashSalt = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)).Concat(hmac.Key).ToArray();
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")

            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


    }
}
