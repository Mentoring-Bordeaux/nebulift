<template>
    <div class="project-card">
      <div class="project-title">
        <span class="icon">🚀</span> <!-- Example character icon -->
        {{ title }}
      </div>
      <div class="title-underline"></div>
      <ul>
        <li v-for="feature in features" :key="feature">{{ "• " + feature }}</li>
      </ul>
    </div>
  </template>
  
  <script>
  export default {
    name: "ProjectCard",
    props: {
      title: {
        type: String,
        required: true,
      },
      features: {
        type: Array,
        required: true,
      },
    },
  };
  </script>
  
  <style scoped>
  .project-card {
    border: 1px solid #eaeaea;
    border-radius: 8px;
    padding: 1rem;
    background: #ffffff;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    margin-bottom: 1rem;
    color: black; /* Set text color to black */
  }
  
  .project-title {
    font-size: 1.25rem;
    font-weight: bold;
    margin-bottom: 0.5rem;
    display: flex;
    align-items: center;
  }
  
  .icon {
    margin-right: 0.5rem;
    font-size: 1.25rem; /* Adjust the size as needed */
  }
  .title-underline {
    border-bottom: 1px solid #eaeaea;
    margin-bottom: 1rem;
  }
  </style>