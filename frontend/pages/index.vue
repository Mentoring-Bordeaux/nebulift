<template>
  <div class="page-container">
    <Header />
    <main class="main-container">
      <h1 class="title">Create a project</h1>
      <ProjectGrid :projects="projects" />
    </main>
  </div>
</template>

<script>
import Header from "@/components/Header.vue";
import ProjectGrid from "@/components/ProjectGrid.vue";

export default {
  components: {
    Header,
    ProjectGrid,
  },
  data() {
    return {
      projects: [
        {
          title: "SaaS project",
          features: ["GitHub", ".NET Backend", "React Frontend", "Stateless"],
        },
        {
          title: "Big Data project",
          features: ["GitLab", "Python Backend", "Redis DB"],
        },
        {
          title: "REST API",
          features: ["Gitea", "Spring backend", "Jenkins"],
        },
        {
          title: "Browser videogame",
          features: ["GitHub", "C++ Backend", "Vue Frontend"],
        },
      ],
    };
  },
};
</script>

<style scoped>
html, body {
  margin: 0;
  padding: 0;
  width: 100%;
  height: 100%;
  background-color: white; /* Set background to white */
}

.page-container {
  display: flex;
  flex-direction: column;
  height: 100vh; /* Full viewport height */
}

.main-container {
  flex: 1;
  padding: 2rem;
  background-color: #FAFAFA;
  width: 100%; /* Ensure it takes the full width */
  box-sizing: border-box; /* Include padding in the element's total width and height */
  margin-top: 80px; /* Add margin to account for fixed header */
  overflow: auto; /* Enable scrolling if content overflows */
}

.title {
  color: black;
  font-size: 2rem;
  font-weight: bold;
  margin-bottom: 1rem;
}
</style>