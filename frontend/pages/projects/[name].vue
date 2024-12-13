<template>
  <div>
    <Header />
    <main class="main-container p-8 bg-gray-100 overflow-auto">
      <div v-if="project">
        <h1 class="text-2xl font-bold mb-4 text-black">{{ project.name }}</h1>
        <ul>
          <li v-for="technology in project.technologies" :key="technology" class="text-black pl-4">{{ technology }}</li>
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
import { useProjectStore } from '@/stores/projectStore';
import { createError, useError } from '#app';

const route = useRoute();
const projectStore = useProjectStore();
const error = useError();
const project = ref<{ name: string; technologies: string[] } | null>(null);

onMounted(() => {
  const name = route.params.name as string;
  console.log("Route name:", name);
  console.log("Project Store:", projectStore.projects);

  const foundProject = projectStore.projects.find(p => p.name === name);
  if (foundProject) {
    project.value = foundProject;
  } else {
    error.value = createError({ statusCode: 404, statusMessage: 'Project "' + name + '" not found' });
  }
});
</script>