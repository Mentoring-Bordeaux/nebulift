<template>
  <div class="projects-grid">
    <ProjectCard
      v-for="(project, index) in projects"
      :key="index"
      :name="project.name"
      :technologies="project.technologies"
      :id="project.id"
      @click="handleCardClick(project)"
    />
  </div>
</template>

<script>
import ProjectCard from "@/components/ProjectCard.vue";
import { useRouter } from 'vue-router';

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
  setup() {
    const router = useRouter();

    const handleCardClick = (project) => {
      console.log("Project techno:", project.technologies);
      router.push({
        path: `/projects/${project.id}`,
        query: {
          name: project.name,
          technologies: JSON.stringify(project.technologies),
        },
      });
    };

    return {
      handleCardClick,
    };
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