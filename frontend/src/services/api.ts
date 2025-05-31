import type { Event, EventCreateRequest, EventUpdateRequest, Registration, RegistrationCreateRequest, ApiError } from '@/types'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:7071/api'

class ApiService {
  private async handleResponse<T>(response: Response): Promise<T> {
    if (!response.ok) {
      let errorData: ApiError
      try {
        errorData = await response.json()
      } catch {
        errorData = { error: `HTTP ${response.status}: ${response.statusText}` }
      }
      throw new Error(errorData.error + (errorData.details ? ': ' + errorData.details.join(', ') : ''))
    }
    
    return response.json()
  }

  // Event methods
  async getEvents(date?: string, location?: string): Promise<Event[]> {
    const params = new URLSearchParams()
    if (date) params.append('date', date)
    if (location) params.append('location', location)
    
    const url = `${API_BASE_URL}/events${params.toString() ? '?' + params.toString() : ''}`
    const response = await fetch(url)
    return this.handleResponse<Event[]>(response)
  }

  async getEvent(id: string): Promise<Event> {
    const response = await fetch(`${API_BASE_URL}/events/${id}`)
    return this.handleResponse<Event>(response)
  }

  async createEvent(event: EventCreateRequest): Promise<Event> {
    const response = await fetch(`${API_BASE_URL}/events`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(event)
    })
    return this.handleResponse<Event>(response)
  }

  async updateEvent(id: string, event: EventUpdateRequest): Promise<Event> {
    const response = await fetch(`${API_BASE_URL}/events/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(event)
    })
    return this.handleResponse<Event>(response)
  }

  async deleteEvent(id: string): Promise<void> {
    const response = await fetch(`${API_BASE_URL}/events/${id}`, {
      method: 'DELETE'
    })
    if (!response.ok) {
      let errorData: ApiError
      try {
        errorData = await response.json()
      } catch {
        errorData = { error: `HTTP ${response.status}: ${response.statusText}` }
      }
      throw new Error(errorData.error + (errorData.details ? ': ' + errorData.details.join(', ') : ''))
    }
  }

  // Registration methods
  async createRegistration(registration: RegistrationCreateRequest): Promise<Registration> {
    const response = await fetch(`${API_BASE_URL}/registrations`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(registration)
    })
    return this.handleResponse<Registration>(response)
  }

  async getRegistrationsByEvent(eventId: string): Promise<Registration[]> {
    const response = await fetch(`${API_BASE_URL}/events/${eventId}/registrations`)
    return this.handleResponse<Registration[]>(response)
  }

  async getRegistration(id: string): Promise<Registration> {
    const response = await fetch(`${API_BASE_URL}/registrations/${id}`)
    return this.handleResponse<Registration>(response)
  }
}

export const apiService = new ApiService()
export default apiService