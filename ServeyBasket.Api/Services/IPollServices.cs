using ServeyBasket.Api.Models;

namespace ServeyBasket.Api.Services;

public interface IPollServices
{
    IEnumerable<Poll> GetAll();
    Poll? Get(int id);
    Poll Add(Poll poll);
    bool Update(int id, Poll poll);
    bool Deleted(int id);

}
