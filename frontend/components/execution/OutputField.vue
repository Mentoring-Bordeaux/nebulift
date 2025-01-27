<script setup lang="ts">
import type { OutputField } from '@/types/template';

interface Props {
  field: OutputField;
  fieldKey: string;
}

const props = defineProps<Props>();

const isUrl = computed(() => {
  try {
    new URL(props.field.value);
    return true;
  } catch {
    return false;
  }
});
</script>
<template>
  <div class="bg-white rounded-lg p-6 shadow-sm border border-gray-200">
    <div class="flex justify-between items-start mb-2">
      <h3 class="text-lg font-semibold text-gray-900">{{ field.title }}</h3>
      <span
        class="px-2 py-1 text-xs font-medium bg-gray-100 rounded-full text-gray-600"
      >
        {{ field.type }}
      </span>
    </div>

    <p class="text-sm text-gray-600 mb-3">{{ field.description }}</p>

    <div class="mt-2">
      <template v-if="isUrl">
        <a
          :href="field.value"
          target="_blank"
          rel="noopener noreferrer"
          class="text-blue-600 hover:text-blue-800 break-all"
        >
          {{ field.value }}
        </a>
      </template>
      <template v-else>
        <code
          class="px-2 py-1 bg-gray-50 rounded text-sm font-mono break-all text-black"
        >
          {{ field.value }}
        </code>
      </template>
    </div>
  </div>
</template>
