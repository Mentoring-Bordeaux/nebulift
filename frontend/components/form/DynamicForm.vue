<script setup lang="ts">
import { ref, computed, watch } from 'vue';
import type { TemplateSchema, TechnologySectionSchema, FormData } from '@/types/template';
import { useTemplateConfigStore } from '@/stores/templateConfigStore';
import TechnologySection from './TechnologySection.vue';

const store = useTemplateConfigStore();

// Define error state type
type ErrorState = Record<string, Record<string, string>>;
const errors = ref<ErrorState>({});

// Type-safe schema entries computation
interface SchemaEntry {
  technology: string;
  schema: TechnologySectionSchema;
}

const schemaEntries = computed<SchemaEntry[]>(() => {
  if (!store.schema) return [];
  
  return Object.entries(store.schema).map(([technology, schema]) => ({
    technology: String(technology),
    schema: schema
  }));
});

// Type-safe form value update handler
const handleFormUpdate = (technology: string, value: Record<string, string>): void => {
  store.formData[technology] = value;
};

// Form submission handler
const handleSubmit = async (): Promise<void> => {
  errors.value = {};
  let hasErrors = false;

  if (store.schema) {
    Object.entries(store.schema).forEach(([technology, fields]) => {
      const techKey = String(technology);
      errors.value[techKey] = {};

      Object.keys(fields).forEach((field) => {
        const fieldKey = String(field);
        const value = store.formData[techKey]?.[fieldKey];

        if (!value || value === '') {
          errors.value[techKey][fieldKey] = 'This field is required';
          hasErrors = true;
        }
      });

      if (Object.keys(errors.value[techKey]).length === 0) {
        delete errors.value[techKey];
      }
    });
  }

  if (!hasErrors) {
    try {
      await store.submitConfig();
      // Handle successful submission
    } catch (error) {
      console.error('Submission error:', error);
    }
  }
};

// Watch for schema changes
watch(() => store.schema, (newSchema) => {
  if (newSchema) {
    errors.value = {};
  }
}, { immediate: true });
</script>

<template>
  <div class="max-w-4xl mx-auto p-6">
    <form @submit.prevent="handleSubmit" class="space-y-6">
      <!-- Loading State -->
      <div v-if="store.loading" class="text-center py-8">
        <span class="text-gray-600">Loading...</span>
      </div>

      <!-- Error State -->
      <div 
        v-else-if="store.error" 
        class="bg-red-50 border border-red-200 rounded-md p-4"
      >
        <p class="text-red-700">{{ store.error }}</p>
      </div>

      <!-- Form Content -->
      <template v-else-if="store.schema">
        <div class="space-y-8">
          <TechnologySection
            v-for="{ technology, schema } in schemaEntries"
            :key="technology"
            :technology="technology"
            :schema="schema"
            :model-value="store.formData[technology] || {}"
            :errors="errors[technology]"
            @update:model-value="(value: Record<string, string>) => handleFormUpdate(technology, value)"
          />
        </div>

        <!-- Submit Button -->
        <div class="flex justify-end mt-6">
          <button
            type="submit"
            class="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 
                   focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2
                   disabled:opacity-50 disabled:cursor-not-allowed"
            :disabled="store.loading"
          >
            {{ store.loading ? 'Submitting...' : 'Submit' }}
          </button>
        </div>
      </template>
    </form>
  </div>
</template>