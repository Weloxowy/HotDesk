using HotDeskApp.Server.Models.Desk.Dtos;
using HotDeskApp.Server.Models.Desk.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotDeskApp.Server.Controllers.Desk;

[ApiController]
[Route("desk/")]
public class DeskController: ControllerBase
{
        private readonly IDeskService _deskService;

        public DeskController(IDeskService deskService)
        {
            _deskService = deskService;
        }

        
        
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<DeskDto>>> GetAllDesks()
        {
            var desks = await _deskService.GetAllDesksInfo();

            if (desks == null || !desks.Any())
            {
                return NotFound();
            }

            return Ok(desks);
        }
        
        
        [HttpGet("{id}")]
        public async Task<DeskDto> GetDeskById(Guid id)
        {
            return await _deskService.GetDeskInfo(id);
        }
        
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateDesk([FromBody]Models.Desk.Entities.Desk desk)
        {
            var userId = await _deskService.CreateNewDesk(desk);
            return Ok(userId);
        }
        
        [HttpPost("/bulk")]
        public async Task<ActionResult<Guid>> CreateManyDesks([FromBody]Models.Desk.Entities.Desk desk, int numberOfDesks)
        {
            var userId = await _deskService.CreateNewDesks(desk,numberOfDesks);
            return Ok(userId);
        }
        
        [HttpPut]
        public async Task<ActionResult<Guid>> UpdateDesk([FromBody]Models.Desk.Entities.Desk desk)
        {
            await _deskService.UpdateDesk(desk);
            return Ok();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<Guid>> DeleteDesk(Guid id)
        {
            await _deskService.DeleteDesk(id);
            return Ok();
        }

}