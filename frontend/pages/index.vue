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
  <div class="page-container flex flex-col h-screen">
    <Header />
    <main class="main-container flex-1 p-8 bg-gray-100 mt-20 overflow-auto">
      <h1 class="title text-2xl font-bold mb-4 text-black">Create a project</h1>
      <div class="title-underline border-b-4 mb-4"></div>
      <div v-if="error">
        <p class="text-red-500">Error fetching projects: {{ error }}</p>
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
