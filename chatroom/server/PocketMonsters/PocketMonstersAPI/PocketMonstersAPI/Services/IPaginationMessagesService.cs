using PocketMonstersAPI.Models;
namespace PocketMonstersAPI.Services;

public interface IPaginationMessagesService
{
    Task<IEnumerable<MessageList>> GetAllAsync(int pg);

    Task<IEnumerable<Output>> GetPagination(int pg );

    Task<IResult> CreateAsync(MessageList mess);

    Task<IResult> DeleteMesage(MessageOutput mess);

}
