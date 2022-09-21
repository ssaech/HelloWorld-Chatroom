using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using Dapper;
using System.Web.Http.Cors;
using PocketMonstersAPI.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;



namespace PocketMonstersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaginationMessagesController : ControllerBase
    {
        private readonly IConfiguration _config;

        public PaginationMessagesController(IConfiguration config)
        {
            _config = config;
        }


        [HttpGet("messageData")]
        public async Task<ActionResult<List<MessageList>>> GetMessageListJustData(int pg = 1)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<MessageList> mess = await connection.QueryAsync<MessageList>("select * from AngChat where Active = '1' Order by dateCreated desc");
            List<MessageList> messages = mess.ToList();
            const int pageSize = 5;
            if (pg < 1)
                pg = 1;

            int recsCount = messages.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = messages.Skip(recSkip).Take(pager.PageSize).ToList();


            return Ok(data);

        }

        [HttpGet("paginationData")]
        public async Task<ActionResult> GetMessageJustPage(int pg = 1)
        {   
                using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
                IEnumerable<MessageList> mess = await connection.QueryAsync<MessageList>("select * from AngChat where Active = '1' Order by dateCreated desc");
                List<MessageList> messages = mess.ToList();
                const int pageSize = 5;
                if (pg < 1)
                    pg = 1;

                int recsCount = messages.Count();
                var pager = new Pager(recsCount, pg, pageSize);
                int recSkip = (pg - 1) * pageSize;
                var data = messages.Skip(recSkip).Take(pager.PageSize).ToList();

                List<Output> items = new List<Output>
                {   
                    new Output { TotalItems=pager.TotalItems, CurrentPage=pager.CurrentPage, PageSize=pager.PageSize,
                            TotalPages = pager.TotalPages, StartPage = pager.StartPage, EndPage = pager.EndPage}

                 };

                int[] vari = new int[pager.PageSize];
                return Ok(items);
            
        }

        [HttpPost]
        public async Task<ActionResult<List<MessageList>>> PostMessageList(MessageList mess)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("INSERT INTO dbo.AngChat (DateCreated, DisplayName, UserID, PictureLink, Message, Active, MessageID) " +
                                          "VALUES (@DateCreated, @DisplayName, @UserID, @PictureLink, @Message, '1', @MessageID)", mess);
            return Ok();

        }

        [HttpPut("delete")]
        public async Task<ActionResult<List<MessageList>>> DeleteMesage(MessageOutput mess)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("UPDATE dbo.AngChat SET Active = '0' WHERE  UserID=@UserID AND MessageID=@MessageID", mess);
            return Ok();

        }

    }
}

