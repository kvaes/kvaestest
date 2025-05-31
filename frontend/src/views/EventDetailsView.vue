<template>
  <div class="event-details">
    <div v-if="loading" class="loading">
      Loading event details...
    </div>

    <div v-if="error" class="error">
      {{ error }}
      <button @click="loadEvent" class="btn btn-secondary">Retry</button>
    </div>

    <div v-if="event && !loading" class="event-content">
      <div class="event-header">
        <h1>{{ event.name }}</h1>
        <div class="actions">
          <RouterLink :to="`/events/${event.id}/edit`" class="btn btn-secondary">Edit Event</RouterLink>
          <button @click="deleteEvent" class="btn btn-danger">Delete Event</button>
        </div>
      </div>

      <div class="event-info">
        <div class="info-item">
          <strong>Location:</strong> {{ event.location }}
        </div>
        <div class="info-item">
          <strong>Date:</strong> {{ formatDate(event.date) }}
        </div>
        <div class="info-item">
          <strong>Start Time:</strong> {{ event.startTime }}
        </div>
        <div class="info-item">
          <strong>Created:</strong> {{ formatDateTime(event.createdAt) }}
        </div>
        <div class="info-item">
          <strong>Last Updated:</strong> {{ formatDateTime(event.updatedAt) }}
        </div>
      </div>

      <div class="registration-section">
        <h2>Register for this Event</h2>
        
        <div v-if="registrationSuccess" class="success-message">
          Thank you for registering! We'll be in touch with more details.
        </div>

        <form v-if="!registrationSuccess" @submit.prevent="submitRegistration" class="registration-form">
          <div class="form-group">
            <label for="name">Full Name *</label>
            <input
              id="name"
              v-model="registrationForm.name"
              type="text"
              required
              :disabled="submitting"
            />
          </div>

          <div class="form-group">
            <label for="email">Email Address *</label>
            <input
              id="email"
              v-model="registrationForm.email"
              type="email"
              required
              :disabled="submitting"
            />
          </div>

          <div class="form-group">
            <label for="pronouns">Pronouns *</label>
            <input
              id="pronouns"
              v-model="registrationForm.pronouns"
              type="text"
              placeholder="e.g., they/them, she/her, he/him"
              required
              :disabled="submitting"
            />
          </div>

          <div class="form-group checkbox-group">
            <label for="optIn">
              <input
                id="optIn"
                v-model="registrationForm.optInCommunication"
                type="checkbox"
                :disabled="submitting"
              />
              I would like to receive further communication about events and updates
            </label>
          </div>

          <div v-if="registrationError" class="error-message">
            {{ registrationError }}
          </div>

          <div class="form-actions">
            <button type="submit" :disabled="submitting" class="btn btn-primary">
              {{ submitting ? 'Registering...' : 'Register for Event' }}
            </button>
          </div>
        </form>
      </div>

      <div class="navigation">
        <RouterLink to="/" class="btn btn-secondary">← Back to Events</RouterLink>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { RouterLink } from 'vue-router'
import apiService from '@/services/api'
import type { Event, RegistrationCreateRequest } from '@/types'

const route = useRoute()
const router = useRouter()

const event = ref<Event | null>(null)
const loading = ref(false)
const error = ref<string | null>(null)
const registrationSuccess = ref(false)
const registrationError = ref<string | null>(null)
const submitting = ref(false)

const registrationForm = ref<RegistrationCreateRequest>({
  eventId: '',
  name: '',
  email: '',
  pronouns: '',
  optInCommunication: false
})

const loadEvent = async () => {
  const eventId = route.params.id as string
  loading.value = true
  error.value = null
  
  try {
    event.value = await apiService.getEvent(eventId)
    registrationForm.value.eventId = eventId
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'Failed to load event'
  } finally {
    loading.value = false
  }
}

const submitRegistration = async () => {
  submitting.value = true
  registrationError.value = null
  
  try {
    await apiService.createRegistration(registrationForm.value)
    registrationSuccess.value = true
  } catch (err) {
    registrationError.value = err instanceof Error ? err.message : 'Failed to register for event'
  } finally {
    submitting.value = false
  }
}

const deleteEvent = async () => {
  if (!event.value || !confirm('Are you sure you want to delete this event?')) {
    return
  }
  
  try {
    await apiService.deleteEvent(event.value.id)
    router.push('/')
  } catch (err) {
    alert('Failed to delete event: ' + (err instanceof Error ? err.message : 'Unknown error'))
  }
}

const formatDate = (dateString: string) => {
  return new Date(dateString + 'T00:00:00').toLocaleDateString()
}

const formatDateTime = (dateTimeString: string) => {
  return new Date(dateTimeString).toLocaleString()
}

onMounted(() => {
  loadEvent()
})
</script>

<style scoped>
.event-details {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}

.event-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 30px;
}

.event-header h1 {
  margin: 0;
  color: #2c3e50;
  flex: 1;
}

.actions {
  display: flex;
  gap: 10px;
  flex-wrap: wrap;
}

.event-info {
  background: #f8f9fa;
  border-radius: 8px;
  padding: 20px;
  margin-bottom: 30px;
}

.info-item {
  margin-bottom: 10px;
  font-size: 16px;
}

.info-item strong {
  color: #2c3e50;
  display: inline-block;
  width: 120px;
}

.registration-section {
  background: white;
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  padding: 30px;
  margin-bottom: 30px;
}

.registration-section h2 {
  margin: 0 0 20px 0;
  color: #2c3e50;
}

.registration-form {
  max-width: 500px;
}

.form-group {
  margin-bottom: 20px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
  font-weight: 500;
  color: #2c3e50;
}

.form-group input[type="text"],
.form-group input[type="email"] {
  width: 100%;
  padding: 10px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
  box-sizing: border-box;
}

.form-group input:focus {
  outline: none;
  border-color: #007bff;
  box-shadow: 0 0 0 2px rgba(0, 123, 255, 0.25);
}

.checkbox-group label {
  display: flex;
  align-items: flex-start;
  gap: 10px;
  cursor: pointer;
}

.checkbox-group input[type="checkbox"] {
  margin-top: 2px;
}

.form-actions {
  margin-top: 25px;
}

.btn {
  padding: 10px 20px;
  border: none;
  border-radius: 4px;
  text-decoration: none;
  font-size: 14px;
  cursor: pointer;
  transition: background-color 0.2s;
  display: inline-block;
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-primary {
  background: #007bff;
  color: white;
}

.btn-primary:hover:not(:disabled) {
  background: #0056b3;
}

.btn-secondary {
  background: #6c757d;
  color: white;
}

.btn-secondary:hover {
  background: #545b62;
}

.btn-danger {
  background: #dc3545;
  color: white;
}

.btn-danger:hover {
  background: #c82333;
}

.loading, .error {
  text-align: center;
  padding: 40px;
  color: #666;
}

.error {
  color: #dc3545;
}

.success-message {
  background: #d4edda;
  color: #155724;
  padding: 15px;
  border-radius: 4px;
  margin-bottom: 20px;
}

.error-message {
  background: #f8d7da;
  color: #721c24;
  padding: 15px;
  border-radius: 4px;
  margin-bottom: 20px;
}

.navigation {
  text-align: center;
  margin-top: 30px;
}
</style>