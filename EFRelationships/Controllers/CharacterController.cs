using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFRelationships.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {

        private readonly DataContext dataContext;
        
        public CharacterController(DataContext context)
        {
            dataContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Character>>> Get(int userId)
        {
            var characters = await dataContext.Characters
                            .Where(c => c.UserId == userId)
                            .Include(c=>c.Weapon)
                            .Include(c=>c.Skills)
                            .ToListAsync();

            return characters;
        }

        [HttpPost]
        public async Task<ActionResult<List<Character>>> Create(CreateCharacterDto request)
        {
            var user = await dataContext.Users.FindAsync(request.UserId);
            if (user == null) return NotFound();

            var newCharacter = new Character { name = request.Name, RpgClass = request.RpgClass,
                               User = user};

            dataContext.Characters.Add(newCharacter);
            await dataContext.SaveChangesAsync();

            return await Get(newCharacter.UserId);
        }

        [HttpPost("weapon")]
        public async Task<ActionResult<Character>> AddWeapon(AddWeaponDto request)
        {
            var character = await dataContext.Characters.FindAsync(request.CharacterId);
            if (character == null) return NotFound();

            var newWeapon= new Weapon
            {
                Name = request.Name,
                Damage = request.Damage,
                Character = character
            };

            dataContext.Weapons.Add(newWeapon);
            await dataContext.SaveChangesAsync();

            return character;
        }

        [HttpPost("skill")]
        public async Task<ActionResult<Character>> AddCharacterSkill(AddCharacterSkillDto request)
        {
            //eager loading
            //var character = await dataContext.Characters
            //                .Where(c => c.id == request.CharacterId)
            //                .Include(c => c.Skills)
            //                .FirstOrDefaultAsync();
            
            // explicit loading (counts as lazy)
            var character = await dataContext.Characters.FindAsync(request.CharacterId);
            
            if (character == null) return NotFound();
            var skill = await dataContext.Skills.FindAsync(request.SkillId);
            if (skill == null) return NotFound();
            dataContext.Set<Skill>();
            //explicit:
            dataContext.Entry(character).Collection(c => c.Skills).Load();

            character.Skills.Add(skill);

            await dataContext.SaveChangesAsync();

            return character;
        }
    }
}
