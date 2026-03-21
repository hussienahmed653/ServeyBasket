using ServeyBasket.Models;

namespace ServeyBasket.Services;

public class PollServices : IPollServices
{
    private static readonly List<Poll> _polls = 
        [
            new Poll
            {
                Id = 1,
                Title = "Hussien",
                Description = "SoftwareDeveloper"
            }
        ];

    public Poll Add(Poll poll)
    {
        poll.Id = _polls.Max(i => i.Id) + 1;
        _polls.Add(poll);
        return poll;
    }

    public bool Deleted(int id)
    {
        var getpoll = Get(id);
        if (getpoll is null)
            return false;

        _polls.Remove(getpoll);

        return true;
    }

    public Poll? Get(int id)
    {
        return _polls.SingleOrDefault(p => p.Id == id);
    }

    public IEnumerable<Poll> GetAll()
    {
        return _polls;
    }

    public bool Update(int id, Poll poll)
    {
        var getpoll = Get(id);
        if (getpoll is null)
            return false;

        getpoll.Title = poll.Title;
        getpoll.Description = poll.Description;

        return true;
    }
}
