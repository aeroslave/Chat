namespace SignalRChat.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;

    using SignalRChat.Authentication;
    using SignalRChat.Models;

    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly UsersContext _context;

        public ChatController(UsersContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Проверить пользователя на активность.
        /// </summary>
        /// <param name="person">Пользователь.</param>
        [Route("isActive")]
        [HttpPut]
        public async Task<IActionResult> CheckActivityPerson(Person person)
        {
            var dBPerson = await _context.Persons.FirstOrDefaultAsync(it => it.Name == person.Name);

            if (dBPerson == null)
                return NotFound();

            if (dBPerson.IsActive)
                return Ok();

            var now = DateTime.UtcNow;
            var identity = GetIdentity(person);

            var jwt = new JwtSecurityToken(
                AuthOptions.ISSUER,
                AuthOptions.AUDIENCE,
                identity.Claims,
                now,
                now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(encodedJwt);
        }

        /// <summary>
        /// Проверить пользователя на наличие.
        /// </summary>
        /// <param name="person">Пользователь</param>
        [HttpPut]
        public async Task<IActionResult> CheckPerson(Person person)
        {
            var isPersonExist = await _context.Persons.AnyAsync(it => it.Name == person.Name);
            if (isPersonExist)
                return Ok();

            return NotFound();
        }

        /// <summary>
        /// Получить список сообщений.
        /// </summary>
        /// <returns></returns>
        [Route("getMessages")]
        [HttpGet]
        public async Task<List<Message>> GetMessages()
        {
            var messages = await _context.Messages.ToListAsync();
            return messages.TakeLast(10).ToList();
        }

        /// <summary>
        /// Получить список пользователей.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<Person>> GetPersons()
        {
            return await _context.Persons.ToListAsync();
        }

        /// <summary>
        /// Добавить пользователя.
        /// </summary>
        /// <param name="person">Пользователь.</param>
        [HttpPost]
        public async Task<long> Post(Person person)
        {
            var isPersonExist =
                await _context.Persons.AnyAsync(it => it.Name == person.Name);
            if (isPersonExist)
                throw new ChatControllerException("Такой пользователь уже создан!");

            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return person.Id;
        }

        /// <summary>
        /// Добавить сообщение.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        [Route("addMessage")]
        [HttpPost]
        public async Task<long> PostMessage(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return message.Id;
        }

        /// <summary>
        /// Разлогинить пользователя.
        /// </summary>
        /// <param name="person">Пользователь.</param>
        [Route("setactivityfalse")]
        [HttpPut]
        public async Task<IActionResult> SetPersonActivity(Person person)
        {
            var dBPerson = await _context.Persons.FirstOrDefaultAsync(it => it.Name == person.Name);

            if (dBPerson == null)
                return NotFound();

            dBPerson.IsActive = false;
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Получить идентификатор.
        /// </summary>
        /// <param name="person">Пользователь.</param>
        private ClaimsIdentity GetIdentity(Person person)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, person.Name),
            };

            var claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}