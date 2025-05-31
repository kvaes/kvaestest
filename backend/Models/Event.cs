using System.ComponentModel.DataAnnotations;

namespace EventsAPI.Models;

public class Event
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(500, MinimumLength = 1)]
    public string Location { get; set; } = string.Empty;
    
    [Required]
    public DateOnly Date { get; set; }
    
    [Required]
    public TimeOnly StartTime { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class EventCreateRequest
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(500, MinimumLength = 1)]
    public string Location { get; set; } = string.Empty;
    
    [Required]
    public DateOnly Date { get; set; }
    
    [Required]
    public TimeOnly StartTime { get; set; }
}

public class EventUpdateRequest
{
    [StringLength(200, MinimumLength = 1)]
    public string? Name { get; set; }
    
    [StringLength(500, MinimumLength = 1)]
    public string? Location { get; set; }
    
    public DateOnly? Date { get; set; }
    
    public TimeOnly? StartTime { get; set; }
}