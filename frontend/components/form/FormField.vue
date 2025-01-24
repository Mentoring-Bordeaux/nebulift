<script setup lang="ts">
interface Props {
    fieldKey: string;
    fieldConfig: {
        type: string;
        title: string;
        description: string;
        enum?: string[];
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

const isSelectField = props.fieldConfig.enum !== undefined;

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
            <label class="text-sm font-medium text-gray-700">{{ fieldConfig.title }}</label>
            <span class="text-sm text-gray-500">{{ fieldConfig.description }}</span>
        </div>

        <div class="relative">
            <input 
                v-if="!isSelectField" 
                type="text"
                :value="modelValue"
                :class="inputBaseClasses" 
                @input="emit('update:modelValue', ($event.target as HTMLInputElement).value)" 
            >

            <select 
                v-else :value="modelValue"
                :class="inputBaseClasses"
                @change="emit('update:modelValue', ($event.target as HTMLSelectElement).value)"
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
        </div>

        <span v-if="error" class="text-sm text-red-500 mt-1">{{ error }}</span>
    </div>
</template>