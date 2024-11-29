<template>
    <div class="projects-grid">
      <ProjectCard
        v-for="(project, index) in projects"
        :key="index"
        :title="project.title"
        :features="project.features"
      />
    </div>
  </template>
  
  <script>
  import ProjectCard from "@/components/ProjectCard.vue";
  
  export default {
    name: "ProjectGrid",
    components: {
      ProjectCard,
    },
    props: {
      projects: {
        type: Array,
        required: true,
      },
    },
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