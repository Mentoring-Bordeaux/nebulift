<template>
  <div class="projects-grid grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 p-4">
    <ProjectCard
      v-for="(project, index) in projects"
      :key="index"
      :name="project.name"
      :description="project.description"
      :technologies="project.technologies"
      @click="handleCardClick(project)"
    />
  </div>
</template>

<script setup lang="ts">
const props = defineProps({
  projects: {
    type: Array as () => { name: string; technologies: string[]; description: string }[],
    required: true,
  },
});

const router = useRouter();

const handleCardClick = (project: { name: string; technologies: string[]; description: string }) => {
  router.push({
    path: `/projects/${project.name}`
  });
};
</script>