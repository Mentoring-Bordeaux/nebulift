<template>
  <div class="border border-gray-300 rounded-lg p-4 bg-white shadow mb-4 cursor-pointer transition-transform duration-300 ease-in-out hover:shadow-lg hover:scale-105" @click="handleClick">
    <div class="project-name flex items-center font-bold text-xl mb-2 text-black">
      <span class="icon mr-2 text-xl">🚀</span>
      {{ name }}
    </div>
    <div class="name-underline border-b mb-4"></div>
    <ul>
      <li v-for="technology in technologies" :key="technology" class="pl-4 text-black">{{ "• " + technology }}</li>
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