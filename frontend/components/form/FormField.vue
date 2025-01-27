<script setup lang="ts">
import { ref } from 'vue';

interface Props {
  fieldKey: string;
  fieldConfig: {
    type: string;
    title: string;
    description: string;
    enum?: string[];
    items?: {
      type: string;
      title: string;
      uniqueItems?: boolean;
    };
  };
  modelValue: string | string[];
  error?: string;
}

const props = withDefaults(defineProps<Props>(), {
  error: '',
});

const emit = defineEmits<{
  (e: 'update:modelValue', value: string | string[]): void;
}>();

const isSelectField = props.fieldConfig.enum !== undefined;
const isArrayField = props.fieldConfig.type === 'array';

const addArrayItem = () => {
  const arrayValues = Array.isArray(props.modelValue)
    ? [...props.modelValue]
    : [];
  arrayValues.push('');
  emit('update:modelValue', arrayValues);
};

const deleteArrayItem = (index: number) => {
  const arrayValues = Array.isArray(props.modelValue)
    ? [...props.modelValue]
    : [];
  arrayValues.splice(index, 1);
  emit('update:modelValue', arrayValues);
};

const updateArrayItem = (index: number, value: string) => {
  const arrayValues = Array.isArray(props.modelValue)
    ? [...props.modelValue]
    : [];
  arrayValues[index] = value;
  emit('update:modelValue', arrayValues);
};

const initializeArrayItems = () => {
  if (isArrayField) {
    if (!Array.isArray(props.modelValue) || props.modelValue.length === 0) {
      emit('update:modelValue', ['']);
    }
  }
};

initializeArrayItems();
</script>

<template>
  <div class="mb-4">
    <div class="flex flex-col mb-2">
      <label class="text-sm font-medium text-gray-700">{{
        fieldConfig.title
      }}</label>
      <span class="text-sm text-gray-500">{{ fieldConfig.description }}</span>
    </div>

    <div class="relative">
      <!-- Field for array type -->
      <div v-if="isArrayField">
        <div
          v-for="(value, index) in modelValue as string[]"
          :key="index"
          class="mb-2 flex flex-row gap-2"
        >
          <input
            type="text"
            :value="value"
            class="input-base"
            :placeholder="`User ${index + 1}`"
            @input="
              updateArrayItem(index, ($event.target as HTMLInputElement).value)
            "
          />
          <button
            v-if="index > 0"
            type="button"
            class="px-4 py-2 bg-red-500 text-white text-sm rounded-xl hover:bg-red-600 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-2"
            @click="deleteArrayItem(index)"
          >
            Delete
          </button>
        </div>
        <div class="flex justify-end">
          <button
            type="button"
            class="mt-2 px-4 py-2 bg-gray-600 text-white rounded-xl hover:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-gray-500 focus:ring-offset-2"
            @click="addArrayItem"
          >
            Add {{ fieldConfig.items?.title || '' }}
          </button>
        </div>
      </div>

      <!-- Select field -->
      <select
        v-else-if="isSelectField"
        :value="modelValue"
        class="input-base"
        @change="
          emit('update:modelValue', ($event.target as HTMLSelectElement).value)
        "
      >
        <option value="" disabled>Select an option</option>
        <option
          v-for="option in fieldConfig.enum"
          :key="option"
          :value="option"
          class="text-gray-900"
        >
          {{ option }}
        </option>
      </select>

      <!-- Base field -->
      <input
        v-else
        type="text"
        :value="modelValue"
        class="input-base"
        @input="
          emit('update:modelValue', ($event.target as HTMLInputElement).value)
        "
      />
    </div>

    <span v-if="error" class="text-sm text-red-500 mt-1">{{ error }}</span>
  </div>
</template>

<style scoped>
.input-base {
  @apply rounded-md p-2 w-full border border-gray-300 focus:ring-2 focus:ring-blue-500 focus:border-transparent bg-white text-gray-900;
}
</style>
