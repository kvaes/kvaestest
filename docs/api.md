# API Documentation

The Events Manager API provides RESTful endpoints for managing events and registrations. All endpoints return JSON responses and include CORS headers for web client compatibility.

## Base URL

- **Local Development**: `http://localhost:7071/api`
- **Production**: `https://your-function-app.azurewebsites.net/api`

## Authentication

Currently, all endpoints use `AuthorizationLevel.Anonymous` for development purposes. In production, this should be updated to use proper authentication.

## Response Format

### Success Response
```json
{
  "data": "object or array"
}
```

### Error Response
```json
{
  "error": "Error message",
  "details": ["optional", "array", "of", "details"]
}
```

## Events Endpoints

### Get All Events

Retrieve a list of all events with optional filtering.

**Endpoint:** `GET /events`

**Query Parameters:**
- `date` (optional): Filter events by date (YYYY-MM-DD format)
- `location` (optional): Filter events by location (case-insensitive partial match)

**Example Requests:**
```bash
# Get all events
curl http://localhost:7071/api/events

# Get events for a specific date
curl "http://localhost:7071/api/events?date=2025-12-31"

# Get events by location
curl "http://localhost:7071/api/events?location=New York"

# Combine filters
curl "http://localhost:7071/api/events?date=2025-12-31&location=Conference"
```

**Response:**
```json
[
  {
    "id": "12345678-1234-1234-1234-123456789012",
    "name": "Annual Conference",
    "location": "New York Convention Center",
    "date": "2025-12-31",
    "startTime": "09:00",
    "createdAt": "2025-01-01T10:00:00Z",
    "updatedAt": "2025-01-01T10:00:00Z"
  }
]
```

### Get Event by ID

Retrieve a specific event by its ID.

**Endpoint:** `GET /events/{id}`

**Path Parameters:**
- `id`: The unique identifier of the event

**Example Request:**
```bash
curl http://localhost:7071/api/events/12345678-1234-1234-1234-123456789012
```

**Response:**
```json
{
  "id": "12345678-1234-1234-1234-123456789012",
  "name": "Annual Conference",
  "location": "New York Convention Center",
  "date": "2025-12-31",
  "startTime": "09:00",
  "createdAt": "2025-01-01T10:00:00Z",
  "updatedAt": "2025-01-01T10:00:00Z"
}
```

**Error Responses:**
- `404 Not Found`: Event with the specified ID doesn't exist

### Create Event

Create a new event.

**Endpoint:** `POST /events`

**Headers:**
- `Content-Type: application/json`

**Request Body:**
```json
{
  "name": "Annual Conference",
  "location": "New York Convention Center",
  "date": "2025-12-31",
  "startTime": "09:00"
}
```

**Field Validation:**
- `name`: Required, 1-200 characters
- `location`: Required, 1-500 characters
- `date`: Required, YYYY-MM-DD format
- `startTime`: Required, HH:MM format

**Example Request:**
```bash
curl -X POST http://localhost:7071/api/events \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Tech Meetup",
    "location": "Downtown Library",
    "date": "2025-06-15",
    "startTime": "18:30"
  }'
```

**Response:**
```json
{
  "id": "87654321-4321-4321-4321-210987654321",
  "name": "Tech Meetup",
  "location": "Downtown Library",
  "date": "2025-06-15",
  "startTime": "18:30",
  "createdAt": "2025-01-01T15:30:00Z",
  "updatedAt": "2025-01-01T15:30:00Z"
}
```

**Error Responses:**
- `400 Bad Request`: Invalid JSON or validation errors

### Update Event

Update an existing event. Only provided fields will be updated.

**Endpoint:** `PUT /events/{id}`

**Path Parameters:**
- `id`: The unique identifier of the event

**Headers:**
- `Content-Type: application/json`

**Request Body:** (all fields optional)
```json
{
  "name": "Updated Event Name",
  "location": "Updated Location",
  "date": "2025-07-01",
  "startTime": "20:00"
}
```

**Example Request:**
```bash
curl -X PUT http://localhost:7071/api/events/12345678-1234-1234-1234-123456789012 \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Updated Conference Name",
    "startTime": "10:00"
  }'
```

**Response:**
```json
{
  "id": "12345678-1234-1234-1234-123456789012",
  "name": "Updated Conference Name",
  "location": "New York Convention Center",
  "date": "2025-12-31",
  "startTime": "10:00",
  "createdAt": "2025-01-01T10:00:00Z",
  "updatedAt": "2025-01-01T16:00:00Z"
}
```

**Error Responses:**
- `400 Bad Request`: Invalid JSON or validation errors
- `404 Not Found`: Event with the specified ID doesn't exist

### Delete Event

Delete an event by its ID.

**Endpoint:** `DELETE /events/{id}`

**Path Parameters:**
- `id`: The unique identifier of the event

**Example Request:**
```bash
curl -X DELETE http://localhost:7071/api/events/12345678-1234-1234-1234-123456789012
```

**Response:**
- `204 No Content`: Event successfully deleted

**Error Responses:**
- `404 Not Found`: Event with the specified ID doesn't exist

## Registration Endpoints

### Create Registration

Register a user for an event.

**Endpoint:** `POST /registrations`

**Headers:**
- `Content-Type: application/json`

**Request Body:**
```json
{
  "eventId": "12345678-1234-1234-1234-123456789012",
  "name": "John Doe",
  "email": "john.doe@example.com",
  "pronouns": "he/him",
  "optInCommunication": true
}
```

**Field Validation:**
- `eventId`: Required, must be a valid event ID
- `name`: Required, 1-100 characters
- `email`: Required, valid email format
- `pronouns`: Required, max 50 characters
- `optInCommunication`: Required, boolean

**Example Request:**
```bash
curl -X POST http://localhost:7071/api/registrations \
  -H "Content-Type: application/json" \
  -d '{
    "eventId": "12345678-1234-1234-1234-123456789012",
    "name": "Jane Smith",
    "email": "jane.smith@example.com",
    "pronouns": "she/her",
    "optInCommunication": false
  }'
```

**Response:**
```json
{
  "id": "11111111-2222-3333-4444-555555555555",
  "eventId": "12345678-1234-1234-1234-123456789012",
  "name": "Jane Smith",
  "email": "jane.smith@example.com",
  "pronouns": "she/her",
  "optInCommunication": false,
  "registeredAt": "2025-01-01T17:00:00Z"
}
```

**Error Responses:**
- `400 Bad Request`: Invalid JSON, validation errors, or event doesn't exist

### Get Registrations for Event

Retrieve all registrations for a specific event.

**Endpoint:** `GET /events/{eventId}/registrations`

**Path Parameters:**
- `eventId`: The unique identifier of the event

**Example Request:**
```bash
curl http://localhost:7071/api/events/12345678-1234-1234-1234-123456789012/registrations
```

**Response:**
```json
[
  {
    "id": "11111111-2222-3333-4444-555555555555",
    "eventId": "12345678-1234-1234-1234-123456789012",
    "name": "Jane Smith",
    "email": "jane.smith@example.com",
    "pronouns": "she/her",
    "optInCommunication": false,
    "registeredAt": "2025-01-01T17:00:00Z"
  }
]
```

**Error Responses:**
- `404 Not Found`: Event with the specified ID doesn't exist

### Get Registration by ID

Retrieve a specific registration by its ID.

**Endpoint:** `GET /registrations/{id}`

**Path Parameters:**
- `id`: The unique identifier of the registration

**Example Request:**
```bash
curl http://localhost:7071/api/registrations/11111111-2222-3333-4444-555555555555
```

**Response:**
```json
{
  "id": "11111111-2222-3333-4444-555555555555",
  "eventId": "12345678-1234-1234-1234-123456789012",
  "name": "Jane Smith",
  "email": "jane.smith@example.com",
  "pronouns": "she/her",
  "optInCommunication": false,
  "registeredAt": "2025-01-01T17:00:00Z"
}
```

**Error Responses:**
- `404 Not Found`: Registration with the specified ID doesn't exist

## CORS Headers

All endpoints include the following CORS headers:

```
Access-Control-Allow-Origin: *
Access-Control-Allow-Methods: GET, POST, PUT, DELETE, OPTIONS
Access-Control-Allow-Headers: Content-Type, Authorization
```

**OPTIONS Endpoints:**

The API provides OPTIONS endpoints for CORS preflight requests:
- `OPTIONS /events`
- `OPTIONS /events/{id}`
- `OPTIONS /registrations`
- `OPTIONS /registrations/{id}`
- `OPTIONS /events/{eventId}/registrations`

## Error Handling

The API provides consistent error responses for all endpoints:

### Client Errors (4xx)

**400 Bad Request:**
```json
{
  "error": "Validation failed",
  "details": [
    "The Name field is required.",
    "The Email field is not a valid e-mail address."
  ]
}
```

**404 Not Found:**
```json
{
  "error": "Event not found"
}
```

### Server Errors (5xx)

**500 Internal Server Error:**
```json
{
  "error": "An internal error occurred"
}
```

## Rate Limiting

Currently, no rate limiting is implemented. In production, consider implementing rate limiting to prevent abuse.

## API Testing

### Using curl

See the examples above for basic curl commands.

### Using a REST Client

You can use tools like:
- **Postman**: Import the API endpoints and test interactively
- **Insomnia**: Another popular API testing tool
- **VS Code REST Client**: Use `.http` files for testing

Example `.http` file:
```http
### Get all events
GET http://localhost:7071/api/events

### Create an event
POST http://localhost:7071/api/events
Content-Type: application/json

{
  "name": "Test Event",
  "location": "Test Location",
  "date": "2025-12-31",
  "startTime": "15:00"
}

### Register for an event
POST http://localhost:7071/api/registrations
Content-Type: application/json

{
  "eventId": "{{eventId}}",
  "name": "Test User",
  "email": "test@example.com",
  "pronouns": "they/them",
  "optInCommunication": true
}
```

## SDK Usage (Frontend)

The frontend includes a TypeScript API service that provides a convenient interface:

```typescript
import apiService from '@/services/api'

// Get events
const events = await apiService.getEvents()

// Create event
const newEvent = await apiService.createEvent({
  name: 'New Event',
  location: 'Location',
  date: '2025-12-31',
  startTime: '15:00'
})

// Register for event
const registration = await apiService.createRegistration({
  eventId: 'event-id',
  name: 'User Name',
  email: 'user@example.com',
  pronouns: 'they/them',
  optInCommunication: true
})
```

## Production Considerations

When deploying to production, consider:

1. **Authentication**: Implement proper authentication/authorization
2. **Rate Limiting**: Add rate limiting to prevent abuse
3. **Logging**: Implement comprehensive logging
4. **Monitoring**: Add health checks and monitoring
5. **Database**: Replace in-memory storage with persistent database
6. **CORS**: Restrict CORS origins to your domain only
7. **HTTPS**: Ensure all communications use HTTPS
8. **Input Validation**: Add additional server-side validation
9. **Error Handling**: Implement more specific error handling
10. **Documentation**: Keep API documentation updated with OpenAPI/Swagger