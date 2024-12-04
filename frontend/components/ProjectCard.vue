<template>
  <div class="project-card" @click="handleClick">
    <div class="project-name">
      <span class="icon">🚀</span> <!-- Example character icon -->
      {{ name }}
    </div>
    <div class="name-underline"></div>
    <ul>
      <li v-for="technology in technologies" :key="technology" class="technology-item">{{ "• " + technology }}</li>
    </ul>
  </div>
</template>

<script>
export default {
  name: "ProjectCard",
  props: {
    name: {
      type: String,
      required: true,
    },
    technologies: {
      type: Array,
      required: true,
    },
    id: {
      type: String,
      required: true,
    },
  },
  methods: {
    handleClick() {
      this.$emit('click', { id: this.id, name: this.name, technologies: this.technologies });
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
  color: black;
  /* Set text color to black */
  cursor: pointer;
  /* Change cursor to pointer on hover */
  transition: transform 0.3s, box-shadow 0.3s;
  /* Smooth transition for hover effect */
}

.project-card:hover {
  transform: translateY(-5px);
  /* Slightly lift the card */
  box-shadow: 0 8px 12px rgba(0, 0, 0, 0.2);
  /* Increase shadow for hover effect */
}

.project-name {
  font-size: 1.25rem;
  font-weight: bold;
  margin-bottom: 0.5rem;
  display: flex;
  align-items: center;
}

.icon {
  margin-right: 0.5rem;
  font-size: 1.25rem;
  /* Adjust the size as needed */
}

.name-underline {
  border-bottom: 1px solid #eaeaea;
  margin-bottom: 1rem;
}

.technology-item {
  padding-left: 1rem;
  /* Add padding to the left of the list items */
}
</style>