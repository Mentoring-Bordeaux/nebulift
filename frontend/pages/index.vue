<script setup lang="ts">
import Header from "@/components/Header.vue";
import ProjectGrid from "@/components/ProjectGrid.vue";
import { ref, watchEffect } from 'vue';
import { api } from '@/services/api';
import type { Project } from '@/services/api'; // Use type-only import

const projects = ref<Project[]>([]);
const error = ref<string | null>(null);

// Use watchEffect to fetch the data
watchEffect(async () => {
  try {
    const fetchedProjects = await api.project.getAll();
    projects.value = fetchedProjects.map(item => ({
      name: item.name,
      technologies: item.technologies
    }));
  } catch (err) {
    error.value = 'Error fetching projects';
    console.error(err);
  }
});

console.log("Initial project value:", projects.value);
</script>

<template>
  <div class="page-container">
    <Header />
    <main class="main-container">
      <h1 class="title">Create a project</h1>
      <div class="title-underline"></div>
      <div v-if="error">
        <p>Error fetching projects: {{ error }}</p>
      </div>
      <div v-else-if="!projects.length">
        <p>Loading...</p>
      </div>
      <div v-else>
        <ProjectGrid :projects="projects" />
      </div>
    </main>
  </div>
</template>

<style scoped>


.page-container {
  display: flex;
  flex-direction: column;
  height: 100vh; /* Full viewport height */
}

.main-container {
  flex: 1;
  padding: 2rem;
  background-color: #FAFAFA;
  width: 100%; /* Ensure it takes the full width */
  box-sizing: border-box; /* Include padding in the element's total width and height */
  margin-top: 80px; /* Add margin to account for fixed header */
  overflow: auto; /* Enable scrolling if content overflows */
}

.title {
  color: black;
  font-size: 2rem;
  font-weight: bold;
  margin-bottom: 1rem;
}
</style>