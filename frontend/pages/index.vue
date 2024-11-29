<script setup lang="ts">
import Header from "@/components/Header.vue";
import ProjectGrid from "@/components/ProjectGrid.vue";
import { ref, watchEffect } from 'vue';

interface Project {
  title: string;
  features: string[];
}

const { data, error } = await useFetch<{ name: string; technologies: string[] }[]>('http://localhost:5052/api/templates');
const projects = ref<Project[]>([]);

// Use watchEffect to reactively watch the data
watchEffect(() => {
  console.log("Fetched data:", data.value);
  if (data.value && data.value.length) {
    projects.value = data.value.map(item => ({
      title: item.name,
      features: item.technologies
    }));
  }
});

console.log("Initial project value:", projects.value);
</script>

<template>
  <div class="page-container">
    <Header />
    <main class="main-container">
      <h1 class="title">Create a project</h1>
      <div v-if="error">
        <p>Error fetching projects: {{ error.message }}</p>
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
html, body {
  margin: 0;
  padding: 0;
  width: 100%;
  height: 100%;
  background-color: white; /* Set background to white */
}

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