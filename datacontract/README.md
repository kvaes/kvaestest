# Data Contract

This directory contains the data models and contracts that ensure consistency between the backend API and frontend consumer.

## Models

### Event
- `id`: Unique identifier for the event
- `name`: Name of the event
- `location`: Location where the event takes place
- `date`: Date of the event (ISO 8601 format)
- `startTime`: Start time of the event (ISO 8601 format)
- `createdAt`: Timestamp when the event was created
- `updatedAt`: Timestamp when the event was last updated

### Registration
- `id`: Unique identifier for the registration
- `eventId`: ID of the event being registered for
- `name`: Full name of the registrant
- `email`: Email address of the registrant
- `pronouns`: Preferred pronouns of the registrant
- `optInCommunication`: Boolean indicating if registrant opted in for further communication
- `registeredAt`: Timestamp when the registration was created

## Usage

These models should be implemented consistently in both the C# backend (as classes) and the frontend (as TypeScript interfaces or Vue props).