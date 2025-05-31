using EventsAPI.Models;

namespace EventsAPI.Services;

public interface IEventService
{
    Task<Event> CreateEventAsync(EventCreateRequest request);
    Task<Event?> GetEventAsync(string id);
    Task<IEnumerable<Event>> GetEventsAsync(DateOnly? date = null, string? location = null);
    Task<Event?> UpdateEventAsync(string id, EventUpdateRequest request);
    Task<bool> DeleteEventAsync(string id);
}

public class EventService : IEventService
{
    private readonly List<Event> _events = new();
    private readonly object _lock = new();

    public Task<Event> CreateEventAsync(EventCreateRequest request)
    {
        var eventItem = new Event
        {
            Name = request.Name,
            Location = request.Location,
            Date = request.Date,
            StartTime = request.StartTime
        };

        lock (_lock)
        {
            _events.Add(eventItem);
        }

        return Task.FromResult(eventItem);
    }

    public Task<Event?> GetEventAsync(string id)
    {
        lock (_lock)
        {
            var eventItem = _events.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(eventItem);
        }
    }

    public Task<IEnumerable<Event>> GetEventsAsync(DateOnly? date = null, string? location = null)
    {
        lock (_lock)
        {
            var query = _events.AsQueryable();

            if (date.HasValue)
            {
                query = query.Where(e => e.Date == date.Value);
            }

            if (!string.IsNullOrWhiteSpace(location))
            {
                query = query.Where(e => e.Location.Contains(location, StringComparison.OrdinalIgnoreCase));
            }

            return Task.FromResult(query.OrderBy(e => e.Date).ThenBy(e => e.StartTime).AsEnumerable());
        }
    }

    public Task<Event?> UpdateEventAsync(string id, EventUpdateRequest request)
    {
        lock (_lock)
        {
            var eventItem = _events.FirstOrDefault(e => e.Id == id);
            if (eventItem == null)
            {
                return Task.FromResult<Event?>(null);
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                eventItem.Name = request.Name;
            }

            if (!string.IsNullOrWhiteSpace(request.Location))
            {
                eventItem.Location = request.Location;
            }

            if (request.Date.HasValue)
            {
                eventItem.Date = request.Date.Value;
            }

            if (request.StartTime.HasValue)
            {
                eventItem.StartTime = request.StartTime.Value;
            }

            eventItem.UpdatedAt = DateTime.UtcNow;

            return Task.FromResult<Event?>(eventItem);
        }
    }

    public Task<bool> DeleteEventAsync(string id)
    {
        lock (_lock)
        {
            var eventItem = _events.FirstOrDefault(e => e.Id == id);
            if (eventItem == null)
            {
                return Task.FromResult(false);
            }

            _events.Remove(eventItem);
            return Task.FromResult(true);
        }
    }
}