import { createRouter, createWebHistory } from 'vue-router'
import EventsListView from '../views/EventsListView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'events-list',
      component: EventsListView,
    },
    {
      path: '/events/new',
      name: 'create-event',
      component: () => import('../views/EventFormView.vue'),
    },
    {
      path: '/events/:id',
      name: 'event-details',
      component: () => import('../views/EventDetailsView.vue'),
    },
    {
      path: '/events/:id/edit',
      name: 'edit-event',
      component: () => import('../views/EventFormView.vue'),
    },
  ],
})

export default router
