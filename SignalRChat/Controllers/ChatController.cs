namespace SignalRChat.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;

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

        [HttpPut]
        public async Task<IActionResult> CheckPerson(Person person)
        {
            var isPersonExist = await _context.Persons.AnyAsync(it => it.Name == person.Name);
            if (isPersonExist)
                return Ok();

            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<string> GetNumber(long id)
        {
            var person = await _context.Persons.FirstOrDefaultAsync();
            var jsonString = JsonConvert.SerializeObject(person);
            return jsonString;
        }

        [HttpPost]
        public async Task<long> Post(Person person)
        {
            var isPersonExist = await _context.Persons.AnyAsync(it => it.Name == person.Name);
            if (isPersonExist)
                throw new ChatControllerException("Пользователь с таким именем уже существует.");

            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return person.Id;
        }
    }
}