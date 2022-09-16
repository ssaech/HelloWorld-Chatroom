using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using Dapper;
using System.Web.Http.Cors;
using PocketMonstersAPI.Models;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Dynamic;

namespace PocketMonstersAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageListController : ControllerBase
    {
        private readonly IConfiguration _config;

        public MessageListController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<MessageList>>> GetMessageList()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<MessageList> mess =  await connection.QueryAsync<MessageList>("select * from AngChat where Active = '1'");
            return Ok(mess);
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

