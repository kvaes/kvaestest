<script setup lang="ts">
import { RouterView } from 'vue-router'
</script>

<template>
  <div id="app">
    <header class="app-header">
      <div class="container">
        <h1 class="app-title">Events Manager</h1>
        <nav class="nav">
          <RouterLink to="/" class="nav-link">Events</RouterLink>
          <RouterLink to="/events/new" class="nav-link">Create Event</RouterLink>
        </nav>
      </div>
    </header>

    <main class="main-content">
      <RouterView />
    </main>

    <footer class="app-footer">
      <div class="container">
        <p>&copy; 2025 Events Manager. Built with Vue.js and Azure Functions.</p>
      </div>
    </footer>
  </div>
</template>

<style>
* {
  box-sizing: border-box;
}

body {
  margin: 0;
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', 'Roboto', 'Oxygen',
    'Ubuntu', 'Cantarell', 'Fira Sans', 'Droid Sans', 'Helvetica Neue',
    sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  background-color: #f5f5f5;
  color: #333;
  line-height: 1.6;
}

#app {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 20px;
}

.app-header {
  background: white;
  border-bottom: 1px solid #e0e0e0;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.app-header .container {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 20px;
}

.app-title {
  margin: 0;
  color: #2c3e50;
  font-size: 24px;
}

.nav {
  display: flex;
  gap: 20px;
}

.nav-link {
  color: #2c3e50;
  text-decoration: none;
  font-weight: 500;
  padding: 8px 16px;
  border-radius: 4px;
  transition: background-color 0.2s;
}

.nav-link:hover {
  background-color: #f8f9fa;
}

.nav-link.router-link-active {
  background-color: #007bff;
  color: white;
}

.main-content {
  flex: 1;
  padding: 20px 0;
}

.app-footer {
  background: #2c3e50;
  color: white;
  padding: 20px 0;
  text-align: center;
}

.app-footer p {
  margin: 0;
  font-size: 14px;
}

@media (max-width: 768px) {
  .app-header .container {
    flex-direction: column;
    gap: 20px;
  }

  .nav {
    flex-wrap: wrap;
    justify-content: center;
  }
}
</style>
