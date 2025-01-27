<script setup lang="ts">
import Header from '@/components/Header.vue';
import ProjectGrid from '@/components/ProjectGrid.vue';
import { useProjectStore } from '@/stores/projectStore';
import { storeToRefs } from 'pinia';
import { onMounted } from 'vue';

const projectStore = useProjectStore();
const { projects, error } = storeToRefs(projectStore);

onMounted(async () => {
  await projectStore.fetchProjects();
});

console.log('Initial project value:', projects.value);
</script>

<template>
  <div class="page-container flex flex-col h-screen">
    <Header />
    <main class="main-container flex-1 p-8 bg-gray-100 overflow-auto">
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

<style scoped>
html,
body {
  @apply m-0 p-0 w-full h-full bg-white;
}
</style>
