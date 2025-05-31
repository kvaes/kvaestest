using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using EventsAPI.Models;
using EventsAPI.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace EventsAPI;

public class RegistrationsFunctions
{
    private readonly ILogger _logger;
    private readonly IRegistrationService _registrationService;
    private readonly IEventService _eventService;

    public RegistrationsFunctions(ILoggerFactory loggerFactory, IRegistrationService registrationService, IEventService eventService)
    {
        _logger = loggerFactory.CreateLogger<RegistrationsFunctions>();
        _registrationService = registrationService;
        _eventService = eventService;
    }

    [Function("CreateRegistration")]
    public async Task<HttpResponseData> CreateRegistration([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "registrations")] HttpRequestData req)
    {
        _logger.LogInformation("Creating new registration");

        try
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonSerializer.Deserialize<RegistrationCreateRequest>(requestBody, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (request == null)
            {
                return await CreateErrorResponse(req, HttpStatusCode.BadRequest, "Invalid request body");
            }

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(request, new ValidationContext(request), validationResults, true);

            if (!isValid)
            {
                var errors = validationResults.Select(v => v.ErrorMessage ?? "Validation error").ToArray();
                return await CreateErrorResponse(req, HttpStatusCode.BadRequest, "Validation failed", errors);
            }

            // Check if event exists
            var eventItem = await _eventService.GetEventAsync(request.EventId);
            if (eventItem == null)
            {
                return await CreateErrorResponse(req, HttpStatusCode.BadRequest, "Event not found");
            }

            var registration = await _registrationService.CreateRegistrationAsync(request);
            return await CreateSuccessResponse(req, registration);
        }
        catch (JsonException)
        {
            return await CreateErrorResponse(req, HttpStatusCode.BadRequest, "Invalid JSON format");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating registration");
            return await CreateErrorResponse(req, HttpStatusCode.InternalServerError, "An internal error occurred");
        }
    }

    [Function("GetRegistrationsByEvent")]
    public async Task<HttpResponseData> GetRegistrationsByEvent([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "events/{eventId}/registrations")] HttpRequestData req, string eventId)
    {
        _logger.LogInformation($"Getting registrations for event: {eventId}");

        try
        {
            // Check if event exists
            var eventItem = await _eventService.GetEventAsync(eventId);
            if (eventItem == null)
            {
                return await CreateErrorResponse(req, HttpStatusCode.NotFound, "Event not found");
            }

            var registrations = await _registrationService.GetRegistrationsByEventAsync(eventId);
            return await CreateSuccessResponse(req, registrations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting registrations");
            return await CreateErrorResponse(req, HttpStatusCode.InternalServerError, "An internal error occurred");
        }
    }

    [Function("GetRegistration")]
    public async Task<HttpResponseData> GetRegistration([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "registrations/{id}")] HttpRequestData req, string id)
    {
        _logger.LogInformation($"Getting registration with ID: {id}");

        try
        {
            var registration = await _registrationService.GetRegistrationAsync(id);
            if (registration == null)
            {
                return await CreateErrorResponse(req, HttpStatusCode.NotFound, "Registration not found");
            }

            return await CreateSuccessResponse(req, registration);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting registration");
            return await CreateErrorResponse(req, HttpStatusCode.InternalServerError, "An internal error occurred");
        }
    }

    private async Task<HttpResponseData> CreateSuccessResponse(HttpRequestData req, object data)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        AddCorsHeaders(response);
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");
        
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        
        await response.WriteStringAsync(json);
        return response;
    }

    private async Task<HttpResponseData> CreateErrorResponse(HttpRequestData req, HttpStatusCode statusCode, string message, string[]? details = null)
    {
        var response = req.CreateResponse(statusCode);
        AddCorsHeaders(response);
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");

        var errorResponse = new
        {
            error = message,
            details = details
        };

        var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await response.WriteStringAsync(json);
        return response;
    }

    private void AddCorsHeaders(HttpResponseData response)
    {
        response.Headers.Add("Access-Control-Allow-Origin", "*");
        response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
        response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
    }

    [Function("RegistrationsOptions")]
    public HttpResponseData RegistrationsOptions([HttpTrigger(AuthorizationLevel.Anonymous, "options", Route = "registrations")] HttpRequestData req)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        AddCorsHeaders(response);
        return response;
    }

    [Function("RegistrationOptions")]
    public HttpResponseData RegistrationOptions([HttpTrigger(AuthorizationLevel.Anonymous, "options", Route = "registrations/{id}")] HttpRequestData req, string id)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        AddCorsHeaders(response);
        return response;
    }

    [Function("EventRegistrationsOptions")]
    public HttpResponseData EventRegistrationsOptions([HttpTrigger(AuthorizationLevel.Anonymous, "options", Route = "events/{eventId}/registrations")] HttpRequestData req, string eventId)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        AddCorsHeaders(response);
        return response;
    }
}