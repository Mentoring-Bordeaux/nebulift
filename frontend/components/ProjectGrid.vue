<template>
  <div class="projects-grid grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 p-4">
    <ProjectCard
      v-for="(project, index) in projects"
      :key="index"
      :name="project.name"
      :technologies="project.technologies"
      @click="handleCardClick(project)"
    />
  </div>
</template>

<script setup lang="ts">
import { defineProps } from 'vue';
import { useRouter } from 'vue-router';
import ProjectCard from "@/components/ProjectCard.vue";

const props = defineProps({
  projects: {
    type: Array as () => { name: string; technologies: string[] }[],
    required: true,
  },
});

const router = useRouter();

const handleCardClick = (project: { name: string; technologies: string[] }) => {
  console.log("Project techno:", project.technologies);
  router.push({
    path: `/projects/${project.name}`
  });
};
</script>