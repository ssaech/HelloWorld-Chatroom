using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using Dapper;
using System.Web.Http.Cors;
using PocketMonstersAPI.Models;
using PocketMonstersAPI.Services;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace PocketMonstersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaginationMessagesController : ControllerBase
    {
        private readonly IPaginationMessagesService _pageService;

        public PaginationMessagesController(IPaginationMessagesService connection)
        {
            _pageService = connection;
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

