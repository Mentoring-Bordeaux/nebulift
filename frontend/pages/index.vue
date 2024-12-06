<script setup lang="ts">
import Header from "@/components/Header.vue";
import ProjectGrid from "@/components/ProjectGrid.vue";
import { ref, watchEffect } from 'vue';
import { api } from '@/services/api';
import type { Project } from '@/services/api';

const projects = ref<Project[]>([]);
const error = ref<string | null>(null);

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
  height: 100vh;
}

.main-container {
  flex: 1;
  padding: 2rem;
  background-color: #FAFAFA;
  width: 100%;
  box-sizing: border-box;
  margin-top: 80px;
  overflow: auto;
}

.title-underline {
  border-bottom: 3px solid #eaeaea;
  margin-bottom: 1rem;
}

.title {
  color: black;
  font-size: 2rem;
  font-weight: bold;
  margin-bottom: 1rem;
}
</style>