<template>
  <div class="projects-grid">
    <ProjectCard
      v-for="(project, index) in projects"
      :key="project.name"
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
    path: `/projects/${project.name}`,
    query: {
      name: project.name,
      technologies: JSON.stringify(project.technologies),
    },
  });
};
</script>

<style scoped>
.projects-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 1rem;
  width: 100%;
  box-sizing: border-box;
  padding: 2rem;
}
</style>