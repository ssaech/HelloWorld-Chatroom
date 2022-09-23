using System.Data.SqlClient;
using Dapper;
using PocketMonstersAPI.Models;
using PocketMonstersAPI.Services;

namespace PocketMonstersAPI.Services;

public class PaginationMessagesService: IPaginationMessagesService
{
    private readonly IConfiguration _config;


    public PaginationMessagesService(IConfiguration config)
    {
        _config = config;
    }


    public async Task<IEnumerable<MessageList>> GetAllAsync(int pg = 1)
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

        return data;
    }


    public async Task<IEnumerable<Output>> GetPagination(int pg = 1)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        IEnumerable<MessageList> mess = await connection.QueryAsync<MessageList>("select * from AngChat where Active = '1' Order by dateCreated desc");
        List<MessageList> messages = mess.ToList();
        const int pageSize = 5;
        if (pg < 1)
            pg = 1;

        int recsCount = messages.Count();

        var pager = new Pager(recsCount, pg, pageSize);


        List<Output> items = new List<Output>
        {
            new Output {
                        TotalItems=pager.TotalItems, CurrentPage=pager.CurrentPage, PageSize=pager.PageSize,
                        TotalPages = pager.TotalPages, StartPage = pager.StartPage, EndPage = pager.EndPage
                        }

        };
        return items; 
    }


    public async Task<IResult> CreateAsync(MessageList mess)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await connection.ExecuteAsync("INSERT INTO dbo.AngChat (DateCreated, DisplayName, UserID, PictureLink, Message, Active, MessageID) " +
                                      "VALUES (@DateCreated, @DisplayName, @UserID, @PictureLink, @Message, '1', @MessageID)", mess);
        return Results.Ok();
    }


    public async Task<IResult> DeleteMesage(MessageOutput mess)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await connection.ExecuteAsync("UPDATE dbo.AngChat SET Active = '0' WHERE  UserID=@UserID AND MessageID=@MessageID", mess);

        return Results.Ok();
    }

}


