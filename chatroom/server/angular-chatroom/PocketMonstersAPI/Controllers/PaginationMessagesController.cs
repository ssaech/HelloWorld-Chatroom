using Microsoft.AspNetCore.Mvc;
using PocketMonstersAPI.Models;
using PocketMonstersAPI.Services;


namespace PocketMonstersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaginationMessagesController : ControllerBase, IPaginationMessagesController
    {
        private readonly IPaginationMessagesService _pageService;

        public PaginationMessagesController(IPaginationMessagesService serv)
        {
            _pageService = serv;
        }

        [HttpGet("messageData")]
        public async Task<ActionResult<List<MessageList>>> GetMessageListJustData(int pg = 1)
        {
            var data = await _pageService.GetAllAsync(pg);
            if (data is null)
            {
                return NotFound();
            }
            return Ok(data);

        }

        [HttpGet("paginationData")]
        public async Task<ActionResult> GetMessageJustPage(int pg = 1)
        {

            var items = await _pageService.GetPagination(pg);
            return Ok(items);

        }

        [HttpPost]
        public async Task<IResult> PostMessageList(MessageList mess)
        {
            var post =  await _pageService.CreateAsync(mess);
            return Results.Ok();

        }

        [HttpPut("delete")]
        public async Task<IResult> DeleteMesage(MessageOutput mess)
        {
            var post = await _pageService.DeleteMesage(mess);
            return Results.Ok();

        }

    }
}