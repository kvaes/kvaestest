<template>
  <div class="events-list">
    <div class="header">
      <h1>Events</h1>
      <RouterLink to="/events/new" class="btn btn-primary">Create Event</RouterLink>
    </div>

    <!-- Filters -->
    <div class="filters">
      <div class="filter-group">
        <label for="date-filter">Date:</label>
        <input
          id="date-filter"
          v-model="filters.date"
          type="date"
          @change="loadEvents"
        />
      </div>
      <div class="filter-group">
        <label for="location-filter">Location:</label>
        <input
          id="location-filter"
          v-model="filters.location"
          type="text"
          placeholder="Filter by location"
          @input="debounceLoadEvents"
        />
      </div>
      <button @click="clearFilters" class="btn btn-secondary">Clear Filters</button>
    </div>

    <!-- Loading state -->
    <div v-if="loading" class="loading">
      Loading events...
    </div>

    <!-- Error state -->
    <div v-if="error" class="error">
      {{ error }}
      <button @click="loadEvents" class="btn btn-secondary">Retry</button>
    </div>

    <!-- Events list -->
    <div v-if="!loading && !error" class="events-grid">
      <div v-for="event in events" :key="event.id" class="event-card">
        <h3>{{ event.name }}</h3>
        <p class="location">📍 {{ event.location }}</p>
        <p class="date-time">🗓️ {{ formatDate(event.date) }} at {{ event.startTime }}</p>
        <div class="actions">
          <RouterLink :to="`/events/${event.id}`" class="btn btn-primary">View Details</RouterLink>
          <RouterLink :to="`/events/${event.id}/edit`" class="btn btn-secondary">Edit</RouterLink>
          <button @click="deleteEvent(event.id)" class="btn btn-danger">Delete</button>
        </div>
      </div>
    </div>

    <div v-if="!loading && !error && events.length === 0" class="empty-state">
      <p>No events found.</p>
      <RouterLink to="/events/new" class="btn btn-primary">Create your first event</RouterLink>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { RouterLink } from 'vue-router'
import apiService from '@/services/api'
import type { Event } from '@/types'

const events = ref<Event[]>([])
const loading = ref(false)
const error = ref<string | null>(null)
const filters = ref({
  date: '',
  location: ''
})

let debounceTimer: number | null = null

const loadEvents = async () => {
  loading.value = true
  error.value = null
  
  try {
    events.value = await apiService.getEvents(
      filters.value.date || undefined,
      filters.value.location || undefined
    )
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'Failed to load events'
  } finally {
    loading.value = false
  }
}

const debounceLoadEvents = () => {
  if (debounceTimer) {
    clearTimeout(debounceTimer)
  }
  debounceTimer = setTimeout(loadEvents, 300)
}

const clearFilters = () => {
  filters.value.date = ''
  filters.value.location = ''
  loadEvents()
}

const deleteEvent = async (id: string) => {
  if (!confirm('Are you sure you want to delete this event?')) {
    return
  }
  
  try {
    await apiService.deleteEvent(id)
    await loadEvents()
  } catch (err) {
    alert('Failed to delete event: ' + (err instanceof Error ? err.message : 'Unknown error'))
  }
}

const formatDate = (dateString: string) => {
  return new Date(dateString + 'T00:00:00').toLocaleDateString()
}

onMounted(() => {
  loadEvents()
})
</script>

<style scoped>
.events-list {
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 30px;
}

.header h1 {
  margin: 0;
  color: #2c3e50;
}

.filters {
  display: flex;
  gap: 20px;
  align-items: flex-end;
  margin-bottom: 30px;
  padding: 20px;
  background: #f8f9fa;
  border-radius: 8px;
}

.filter-group {
  display: flex;
  flex-direction: column;
  gap: 5px;
}

.filter-group label {
  font-weight: 500;
  color: #2c3e50;
}

.filter-group input {
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
}

.events-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 20px;
}

.event-card {
  background: white;
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  padding: 20px;
  transition: transform 0.2s, box-shadow 0.2s;
}

.event-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.event-card h3 {
  margin: 0 0 10px 0;
  color: #2c3e50;
}

.location, .date-time {
  margin: 8px 0;
  color: #666;
  font-size: 14px;
}

.actions {
  display: flex;
  gap: 10px;
  margin-top: 15px;
  flex-wrap: wrap;
}

.btn {
  padding: 8px 16px;
  border: none;
  border-radius: 4px;
  text-decoration: none;
  font-size: 14px;
  cursor: pointer;
  transition: background-color 0.2s;
}

.btn-primary {
  background: #007bff;
  color: white;
}

.btn-primary:hover {
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

.loading, .error, .empty-state {
  text-align: center;
  padding: 40px;
  color: #666;
}

.error {
  color: #dc3545;
}

.empty-state {
  background: #f8f9fa;
  border-radius: 8px;
}
</style>