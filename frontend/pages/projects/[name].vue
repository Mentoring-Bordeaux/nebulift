<template>
  <div>
    <Header />
    <main class="main-container p-8 bg-gray-100 mt-20 overflow-auto">
      <div v-if="project">
        <h1 class="title text-2xl font-bold mb-4 text-black">{{ project.name }}</h1>
        <ul>
          <li v-for="technology in project.technologies" :key="technology" class="technology-item text-black pl-4">{{ technology }}</li>
        </ul>
      </div>
      <div v-else>
        <p class="text-black">Loading...</p>
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import Header from "@/components/Header.vue";
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import type { Project } from '@/services/api';

const route = useRoute();
const project = ref<Project | null>(null);

onMounted(() => {
  const name = route.query.name as string;
  const technologies = route.query.technologies ? JSON.parse(route.query.technologies as string) : [];

  if (name && technologies.length > 0) {
    project.value = { name, technologies };
  } else {
    console.error('Invalid project data');
  }
});
</script>

<style scoped>
html, body {
  @apply m-0 p-0 w-full h-full bg-gray-100;
}

.main-container {
  @apply p-8 bg-gray-100 mt-20 overflow-auto;
}

.title {
  @apply text-2xl font-bold mb-4 text-black;
}

.technology-item {
  @apply text-black pl-4;
}
</style>