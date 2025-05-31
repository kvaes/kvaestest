using System.ComponentModel.DataAnnotations;

namespace EventsAPI.Models;

public class Registration
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    public string EventId { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string Pronouns { get; set; } = string.Empty;
    
    [Required]
    public bool OptInCommunication { get; set; }
    
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
}

public class RegistrationCreateRequest
{
    [Required]
    public string EventId { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string Pronouns { get; set; } = string.Empty;
    
    [Required]
    public bool OptInCommunication { get; set; }
}