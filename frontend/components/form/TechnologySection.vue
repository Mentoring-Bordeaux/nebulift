<script setup lang="ts">
import type { SectionSchema } from '@/types/template';
import FormField from './FormField.vue';

interface Props {
    sectionKey: string;
    schema: SectionSchema;
    modelValue: Record<string, string>;
    errors?: Record<string, string>;
}

const props = withDefaults(defineProps<Props>(), {
    errors: () => ({})
});

const emit = defineEmits<{
    (e: 'update:modelValue', value: Record<string, string>): void;
}>();

const updateField = (field: string, value: string) => {
    emit('update:modelValue', {
        ...props.modelValue,
        [field]: value
    });
};

</script>

<template>
    <div class="mb-8">
        <div class="flex flex-col mb-4">
            <h3 class="text-lg font-semibold text-gray-800">
                {{ schema.title }}
            </h3>
            <span v-if="schema.description && schema.description.trim() !== ''" class="text-sm text-gray-500">
                {{ schema.description }}
            </span>
        </div>
        

        <div class="space-y-4">
            <FormField 
                v-for="(config, field) in schema.properties" 
                :key="field" 
                :field-key="String(field)"
                :field-config="config" 
                :model-value="modelValue[field] ?? ''" 
                :error="errors?.[field]"
                @update:model-value="updateField(String(field), $event)" />
        </div>
    </div>
</template>