<script setup lang="ts">
import { useTemplateConfigStore } from '@/stores/templateConfigStore';
import type { TemplateSchema, FormData } from '~/types/template';

interface Props {
    formData: FormData;
    schema: TemplateSchema;
    errors?: Record<string, string>;
}

const props = withDefaults(defineProps<Props>(), {
    errors: () => ({})
});
</script>

<template>
    <div class="space-y-6">
        <h2 class="text-xl font-semibold mb-4">Review your configuration</h2>

        <div v-for="(section, sectionKey) in props.formData" :key="sectionKey" class="mb-6">
            <h3 class="text-lg font-semibold mb-2">• {{ props.schema[sectionKey]?.title || sectionKey }}</h3>
            <div class="bg-gray-50 p-4 rounded-md">
                <div v-for="(value, fieldKey) in section" :key="fieldKey" class="mb-2">
                    <strong>- {{ props.schema[sectionKey]?.properties[fieldKey]?.title || fieldKey }} :</strong> {{ value }}
                </div>
            </div>
        </div>

        <div class="flex justify-between mt-6">
            <button 
                type="button" 
                @click="$emit('back')" 
                class="px-4 py-2 w-24 bg-gray-600 text-white rounded-xl hover:bg-gray-700 
                   focus:outline-none focus:ring-2 focus:ring-gray-500 focus:ring-offset-2"
            >
                Back
            </button>
            <button 
                type="button" 
                @click="$emit('create')" 
                class="px-4 py-2 w-32 bg-orange-600 text-white rounded-2xl hover:bg-orange-500 
                   focus:outline-none focus:ring-2 focus:ring-orange-700 focus:ring-offset-2 
                   disabled:opacity-50 disabled:cursor-not-allowed font-bold"
            >
                Create
            </button>
        </div>
    </div>
</template>