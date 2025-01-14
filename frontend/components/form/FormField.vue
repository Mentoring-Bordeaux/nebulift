<script setup lang="ts">
interface Props {
  fieldKey: string; 
  fieldConfig: {
    value: string | string[];
    description: string;
  };
  modelValue: string;
  error?: string;
}

const props = withDefaults(defineProps<Props>(), {
  error: ''
});

const emit = defineEmits<{
  (e: 'update:modelValue', value: string): void;
}>();

const isSelectField = Array.isArray(props.fieldConfig.value);

// Base classes for inputs
const inputBaseClasses = `
  rounded-md 
  p-2 
  w-full 
  border 
  border-gray-300 
  focus:ring-2 
  focus:ring-blue-500 
  focus:border-transparent
  bg-white
  text-gray-900
`;
</script>

<template>
  <div class="mb-4">
    <div class="flex flex-col mb-2">
      <label class="text-sm font-medium text-gray-700">{{ fieldKey }}</label>
      <span class="text-sm text-gray-500">{{ fieldConfig.description }}</span>
    </div>

    <div class="relative">
      <input 
        v-if="!isSelectField"
        :value="modelValue"
        @input="emit('update:modelValue', ($event.target as HTMLInputElement).value)"
        type="text"
        :class="inputBaseClasses"
      />

      <select
        v-else
        :value="modelValue"
        @change="emit('update:modelValue', ($event.target as HTMLSelectElement).value)"
        :class="inputBaseClasses"
      >
        <option value="" disabled>Select an option</option>
        <option 
          v-for="option in fieldConfig.value" 
          :key="option" 
          :value="option"
          class="text-gray-900"
        >
          {{ option }}
        </option>
      </select>
    </div>

    <span v-if="error" class="text-sm text-red-500 mt-1">{{ error }}</span>
  </div>
</template>