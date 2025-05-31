<template>
  <div class="event-form">
    <h1>{{ isEdit ? 'Edit Event' : 'Create New Event' }}</h1>

    <div v-if="error" class="error-message">
      {{ error }}
    </div>

    <form @submit.prevent="submitForm" class="form">
      <div class="form-group">
        <label for="name">Event Name *</label>
        <input
          id="name"
          v-model="form.name"
          type="text"
          required
          :disabled="submitting"
          maxlength="200"
        />
      </div>

      <div class="form-group">
        <label for="location">Location *</label>
        <input
          id="location"
          v-model="form.location"
          type="text"
          required
          :disabled="submitting"
          maxlength="500"
        />
      </div>

      <div class="form-row">
        <div class="form-group">
          <label for="date">Date *</label>
          <input
            id="date"
            v-model="form.date"
            type="date"
            required
            :disabled="submitting"
          />
        </div>

        <div class="form-group">
          <label for="startTime">Start Time *</label>
          <input
            id="startTime"
            v-model="form.startTime"
            type="time"
            required
            :disabled="submitting"
          />
        </div>
      </div>

      <div class="form-actions">
        <button type="submit" :disabled="submitting" class="btn btn-primary">
          {{ submitting ? 'Saving...' : (isEdit ? 'Update Event' : 'Create Event') }}
        </button>
        <RouterLink to="/" class="btn btn-secondary">Cancel</RouterLink>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { RouterLink } from 'vue-router'
import apiService from '@/services/api'
import type { EventCreateRequest, EventUpdateRequest } from '@/types'

const route = useRoute()
const router = useRouter()

const isEdit = computed(() => route.name === 'edit-event')
const eventId = computed(() => route.params.id as string)

const form = ref<EventCreateRequest>({
  name: '',
  location: '',
  date: '',
  startTime: ''
})

const submitting = ref(false)
const error = ref<string | null>(null)

const loadEvent = async () => {
  if (!isEdit.value) return
  
  try {
    const event = await apiService.getEvent(eventId.value)
    form.value = {
      name: event.name,
      location: event.location,
      date: event.date,
      startTime: event.startTime
    }
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'Failed to load event'
  }
}

const submitForm = async () => {
  submitting.value = true
  error.value = null
  
  try {
    if (isEdit.value) {
      // Only send changed fields for update
      const updateData: EventUpdateRequest = {}
      const originalEvent = await apiService.getEvent(eventId.value)
      
      if (form.value.name !== originalEvent.name) updateData.name = form.value.name
      if (form.value.location !== originalEvent.location) updateData.location = form.value.location
      if (form.value.date !== originalEvent.date) updateData.date = form.value.date
      if (form.value.startTime !== originalEvent.startTime) updateData.startTime = form.value.startTime
      
      await apiService.updateEvent(eventId.value, updateData)
      router.push(`/events/${eventId.value}`)
    } else {
      const event = await apiService.createEvent(form.value)
      router.push(`/events/${event.id}`)
    }
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'Failed to save event'
  } finally {
    submitting.value = false
  }
}

onMounted(() => {
  loadEvent()
})
</script>

<style scoped>
.event-form {
  max-width: 600px;
  margin: 0 auto;
  padding: 20px;
}

.event-form h1 {
  margin-bottom: 30px;
  color: #2c3e50;
}

.form {
  background: white;
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  padding: 30px;
}

.form-group {
  margin-bottom: 20px;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
}

@media (max-width: 768px) {
  .form-row {
    grid-template-columns: 1fr;
    gap: 20px;
  }
}

.form-group label {
  display: block;
  margin-bottom: 5px;
  font-weight: 500;
  color: #2c3e50;
}

.form-group input {
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

.form-group input:disabled {
  background-color: #f8f9fa;
  opacity: 0.6;
}

.form-actions {
  display: flex;
  gap: 15px;
  margin-top: 30px;
}

.btn {
  padding: 12px 24px;
  border: none;
  border-radius: 4px;
  text-decoration: none;
  font-size: 14px;
  cursor: pointer;
  transition: background-color 0.2s;
  display: inline-block;
  text-align: center;
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

.error-message {
  background: #f8d7da;
  color: #721c24;
  padding: 15px;
  border-radius: 4px;
  margin-bottom: 20px;
}
</style>