<script setup lang="ts">
import type { TechnologySectionSchema } from '@/types/template';
import FormField from './FormField.vue';

interface Props {
  technology: string;
  schema: TechnologySectionSchema;
  modelValue: Record<string, string>;
  errors?: Record<string, string>;
}

const props = withDefaults(defineProps<Props>(), {
  errors: () => ({})
});

const emit = defineEmits<{
  (e: 'update:modelValue', value: Record<string, string>): void;
}>();

// Helper function to ensure string keys
const ensureString = (key: string | number): string => String(key);

const updateField = (field: string, value: string) => {
  emit('update:modelValue', {
    ...props.modelValue,
    [field]: value
  });
};

// Create entries array with explicit typing
const schemaEntries = Object.entries(props.schema).map(
  ([key, config]) => ({
    key: ensureString(key),
    config
  })
);
</script>

<template>
  <div class="mb-8">
    <h3 class="text-lg font-semibold mb-4 text-gray-800">
      {{ technology }}
    </h3>

    <div class="space-y-4">
      <FormField
        v-for="{ key, config } in schemaEntries"
        :key="key"
        :field-key="key"
        :field-config="config"
        :model-value="modelValue[key] ?? ''"
        :error="errors?.[key]"
        @update:model-value="updateField(key, $event)"
      />
    </div>
  </div>
</template>