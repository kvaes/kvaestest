export interface Event {
  id: string
  name: string
  location: string
  date: string // ISO date format (YYYY-MM-DD)
  startTime: string // Time format (HH:MM)
  createdAt: string // ISO datetime format
  updatedAt: string // ISO datetime format
}

export interface EventCreateRequest {
  name: string
  location: string
  date: string // ISO date format (YYYY-MM-DD)
  startTime: string // Time format (HH:MM)
}

export interface EventUpdateRequest {
  name?: string
  location?: string
  date?: string // ISO date format (YYYY-MM-DD)
  startTime?: string // Time format (HH:MM)
}

export interface Registration {
  id: string
  eventId: string
  name: string
  email: string
  pronouns: string
  optInCommunication: boolean
  registeredAt: string // ISO datetime format
}

export interface RegistrationCreateRequest {
  eventId: string
  name: string
  email: string
  pronouns: string
  optInCommunication: boolean
}

export interface ApiError {
  error: string
  details?: string[]
}