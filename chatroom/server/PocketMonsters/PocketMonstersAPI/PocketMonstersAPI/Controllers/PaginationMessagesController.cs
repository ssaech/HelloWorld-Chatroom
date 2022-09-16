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
            return Ok( items );
        }

    }
}

