using EventsAPI.Models;

namespace EventsAPI.Services;

public interface IRegistrationService
{
    Task<Registration> CreateRegistrationAsync(RegistrationCreateRequest request);
    Task<IEnumerable<Registration>> GetRegistrationsByEventAsync(string eventId);
    Task<Registration?> GetRegistrationAsync(string id);
}

public class RegistrationService : IRegistrationService
{
    private readonly List<Registration> _registrations = new();
    private readonly object _lock = new();

    public Task<Registration> CreateRegistrationAsync(RegistrationCreateRequest request)
    {
        var registration = new Registration
        {
            EventId = request.EventId,
            Name = request.Name,
            Email = request.Email,
            Pronouns = request.Pronouns,
            OptInCommunication = request.OptInCommunication
        };

        lock (_lock)
        {
            _registrations.Add(registration);
        }

        return Task.FromResult(registration);
    }

    public Task<IEnumerable<Registration>> GetRegistrationsByEventAsync(string eventId)
    {
        lock (_lock)
        {
            var registrations = _registrations.Where(r => r.EventId == eventId).OrderBy(r => r.RegisteredAt);
            return Task.FromResult(registrations.AsEnumerable());
        }
    }

    public Task<Registration?> GetRegistrationAsync(string id)
    {
        lock (_lock)
        {
            var registration = _registrations.FirstOrDefault(r => r.Id == id);
            return Task.FromResult(registration);
        }
    }
}