<template>
  <div>
    <Header />
    <main class="main-container">
      <div v-if="project">
        <h1 class="title">{{ project.name }}</h1>
        <ul>
          <li v-for="technology in project.technologies" :key="technology" class="technology-item">{{ "• " + technology }}</li>
        </ul>
      </div>
      <div v-else>
        <p>Loading...</p>
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
  const id = route.params.id as string;
  const name = route.query.name as string;
  const technologies = route.query.technologies ? JSON.parse(route.query.technologies as string) : [];

  if (name && technologies) {
    project.value = { id, name, technologies };
  } else {
    console.error('Invalid project data');
  }
});
</script>

<style scoped>
.main-container {
  padding: 2rem;
  background-color: #FAFAFA;
  width: 100%;
  box-sizing: border-box;
  margin-top: 80px;
  overflow: auto;
}

.title {
  color: black;
  font-size: 2rem;
  font-weight: bold;
  margin-bottom: 1rem;
}

.technology-item {
  color: black;
  padding-left: 1rem;
  /* Add padding to the left of the list items */
}
</style>