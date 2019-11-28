﻿namespace SignalRChat.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

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

            dBPerson.IsActive = true;
            await _context.SaveChangesAsync();

            return NotFound();
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
        /// Добавить пользователя.
        /// </summary>
        /// <param name="person">Пользователь.</param>
        [HttpPost]
        public async Task<long> Post(Person person)
        {
            var isPersonExist =
                await _context.Persons.AnyAsync(it => it.Name == person.Name && it.BirthDate == person.BirthDate);
            if (isPersonExist)
                throw new ChatControllerException("Такой пользователь уже создан!");

            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return person.Id;
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
    }
}