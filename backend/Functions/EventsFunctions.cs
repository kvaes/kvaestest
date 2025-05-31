using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using EventsAPI.Models;
using EventsAPI.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace EventsAPI;

public class EventsFunctions
{
    private readonly ILogger _logger;
    private readonly IEventService _eventService;

    public EventsFunctions(ILoggerFactory loggerFactory, IEventService eventService)
    {
        _logger = loggerFactory.CreateLogger<EventsFunctions>();
        _eventService = eventService;
    }

    [Function("CreateEvent")]
    public async Task<HttpResponseData> CreateEvent([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "events")] HttpRequestData req)
    {
        _logger.LogInformation("Creating new event");

        try
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonSerializer.Deserialize<EventCreateRequest>(requestBody, new JsonSerializerOptions
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

            var eventItem = await _eventService.CreateEventAsync(request);
            return await CreateSuccessResponse(req, eventItem);
        }
        catch (JsonException)
        {
            return await CreateErrorResponse(req, HttpStatusCode.BadRequest, "Invalid JSON format");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating event");
            return await CreateErrorResponse(req, HttpStatusCode.InternalServerError, "An internal error occurred");
        }
    }

    [Function("GetEvent")]
    public async Task<HttpResponseData> GetEvent([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "events/{id}")] HttpRequestData req, string id)
    {
        _logger.LogInformation($"Getting event with ID: {id}");

        try
        {
            var eventItem = await _eventService.GetEventAsync(id);
            if (eventItem == null)
            {
                return await CreateErrorResponse(req, HttpStatusCode.NotFound, "Event not found");
            }

            return await CreateSuccessResponse(req, eventItem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting event");
            return await CreateErrorResponse(req, HttpStatusCode.InternalServerError, "An internal error occurred");
        }
    }

    [Function("GetEvents")]
    public async Task<HttpResponseData> GetEvents([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "events")] HttpRequestData req)
    {
        _logger.LogInformation("Getting events");

        try
        {
            var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            
            DateOnly? date = null;
            if (query["date"] != null && DateOnly.TryParse(query["date"], out var parsedDate))
            {
                date = parsedDate;
            }

            var location = query["location"];

            var events = await _eventService.GetEventsAsync(date, location);
            return await CreateSuccessResponse(req, events);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting events");
            return await CreateErrorResponse(req, HttpStatusCode.InternalServerError, "An internal error occurred");
        }
    }

    [Function("UpdateEvent")]
    public async Task<HttpResponseData> UpdateEvent([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "events/{id}")] HttpRequestData req, string id)
    {
        _logger.LogInformation($"Updating event with ID: {id}");

        try
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonSerializer.Deserialize<EventUpdateRequest>(requestBody, new JsonSerializerOptions
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

            var eventItem = await _eventService.UpdateEventAsync(id, request);
            if (eventItem == null)
            {
                return await CreateErrorResponse(req, HttpStatusCode.NotFound, "Event not found");
            }

            return await CreateSuccessResponse(req, eventItem);
        }
        catch (JsonException)
        {
            return await CreateErrorResponse(req, HttpStatusCode.BadRequest, "Invalid JSON format");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating event");
            return await CreateErrorResponse(req, HttpStatusCode.InternalServerError, "An internal error occurred");
        }
    }

    [Function("DeleteEvent")]
    public async Task<HttpResponseData> DeleteEvent([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "events/{id}")] HttpRequestData req, string id)
    {
        _logger.LogInformation($"Deleting event with ID: {id}");

        try
        {
            var success = await _eventService.DeleteEventAsync(id);
            if (!success)
            {
                return await CreateErrorResponse(req, HttpStatusCode.NotFound, "Event not found");
            }

            var response = req.CreateResponse(HttpStatusCode.NoContent);
            AddCorsHeaders(response);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting event");
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

    [Function("EventsOptions")]
    public HttpResponseData EventsOptions([HttpTrigger(AuthorizationLevel.Anonymous, "options", Route = "events")] HttpRequestData req)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        AddCorsHeaders(response);
        return response;
    }

    [Function("EventOptions")]
    public HttpResponseData EventOptions([HttpTrigger(AuthorizationLevel.Anonymous, "options", Route = "events/{id}")] HttpRequestData req, string id)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        AddCorsHeaders(response);
        return response;
    }
}