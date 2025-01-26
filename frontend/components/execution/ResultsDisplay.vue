<script setup lang="ts">
import type { TemplateOutput } from '@/types/template';
import OutputField from './OutputField.vue';

interface Props {
  output: TemplateOutput;
}

const props = defineProps<Props>();

const outputEntries = computed(() => 
  Object.entries(props.output).map(([key, field]) => ({
    key,
    field
  }))
);
</script>

<template>
  <div class="space-y-6">
    <h2 class="text-2xl font-bold text-gray-900 mb-6">
      Execution Results
    </h2>

    <div class="grid gap-6 md:grid-cols-2">
      <OutputField
        v-for="{ key, field } in outputEntries"
        :key="key"
        :field-key="key"
        :field="field"
      />
    </div>

    <div class="flex justify-end mt-8 pt-4 border-t border-gray-200">
      <button
        @click="$router.push('/')"
        class="px-4 py-2 bg-gray-100 text-gray-700 rounded-md hover:bg-gray-200 mr-4"
      >
        Back to Templates
      </button>
      
      <button
        @click="$router.push(`/projects/${$route.params.id}`)"
        class="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
      >
        Configure New Template
      </button>
    </div>
  </div>
</template>