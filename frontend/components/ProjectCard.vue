<template>
  <div class="project-card" @click="handleClick">
    <div class="project-name">
      <span class="icon">🚀</span>
      {{ name }}
    </div>
    <div class="name-underline"></div>
    <ul>
      <li v-for="technology in technologies" :key="technology" class="technology-item">{{ "• " + technology }}</li>
    </ul>
  </div>
</template>

<script setup lang="ts">
import { defineProps, defineEmits } from 'vue';

const props = defineProps({
  name: {
    type: String,
    required: true,
  },
  technologies: {
    type: Array as () => string[],
    required: true,
  },
});

const emit = defineEmits(['click']);

const handleClick = () => {
  emit('click', { name: props.name, technologies: props.technologies });
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
  cursor: pointer;
  transition: transform 0.3s, box-shadow 0.3s;
}

.project-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 8px 12px rgba(0, 0, 0, 0.2);
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
}

.name-underline {
  border-bottom: 1px solid #eaeaea;
  margin-bottom: 1rem;
}

.technology-item {
  padding-left: 1rem;
}
</style>