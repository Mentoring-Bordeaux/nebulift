<script setup lang="ts">
import { ref, computed, watch } from 'vue';
import type { TemplateSchema, SectionSchema } from '@/types/template';
import { useTemplateConfigStore } from '@/stores/templateConfigStore';
import TechnologySection from './TechnologySection.vue';
import Review from '../Review.vue';

const store = useTemplateConfigStore();

// Define error state type
type ErrorState = Record<string, Record<string, string>>;
const errors = ref<ErrorState>({});

const showReview = ref(false);

// Type-safe schema entries computation
interface SchemaEntry {
    sectionKey: string;
    schema: SectionSchema;
}

const schemaEntries = computed<SchemaEntry[]>(() => {
    if (!store.schema) return [];

    return Object.entries(store.schema).map(([sectionKey, schema]) => ({
        sectionKey: String(sectionKey),
        schema: schema
    }));
});

// Type-safe form value update handler
const handleFormUpdate = (sectionKey: string, value: Record<string, string>): void => {
    store.formData[sectionKey] = value;
};

// Form submission handler
const handleSubmit = async (): Promise<void> => {
    errors.value = {};
    let hasErrors = false;

    if (store.schema) {
        Object.entries(store.schema).forEach(([sectionKey, section]) => {
            const secKey = String(sectionKey);
            errors.value[secKey] = {};

            Object.entries(section.properties).forEach(([field, config]) => {
                const fieldKey = String(field);
                const value = store.formData[secKey]?.[fieldKey];

                if (!value || value === '') {
                    errors.value[secKey][fieldKey] = 'This field is required';
                    hasErrors = true;
                }
            });

            if (Object.keys(errors.value[secKey]).length === 0) {
                delete errors.value[secKey];
            }
        });
    }

    if (!hasErrors) {
        console.log('Form Data:', JSON.stringify(store.formData, null, 2));
        showReview.value = true;
        // try {
        //   await store.submitConfig();
        //   // Handle successful submission
        // } catch (error) {
        //   console.error('Submission error:', error);
        // }
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
            <div v-else-if="store.error" class="bg-red-50 border border-red-200 rounded-md p-4">
                <p class="text-red-700">{{ store.error }}</p>
            </div>

            <!-- Form Content -->
            <template v-else-if="!showReview && store.schema">
                <div class="space-y-8">
                    <TechnologySection 
                        v-for="{ sectionKey, schema } in schemaEntries" 
                        :key="sectionKey"
                        :section-key="sectionKey" 
                        :schema="schema" 
                        :model-value="store.formData[sectionKey] || {}"
                        :errors="errors[sectionKey]"
                        @update:model-value="(value: Record<string, string>) => handleFormUpdate(sectionKey, value)" />
                </div>

                <!-- Next button -->
                <div class="flex justify-end mt-6">
                    <button 
                    type="submit" 
                    class="px-4 py-2 w-32 bg-orange-600 text-white rounded-2xl hover:bg-orange-500 
                           focus:outline-none focus:ring-2 focus:ring-orange-700 focus:ring-offset-2
                           disabled:opacity-50 disabled:cursor-not-allowed font-bold" 
                    :disabled="store.loading"
                    >
                        Next
                    </button>
                </div>
            </template>

            <!-- Review Page -->
            <Review 
                v-else-if="showReview" 
                :form-data="store.formData"
                :schema="store.schema as TemplateSchema"
                @create="store.submitConfig" 
                @back="showReview = false" 
            />
        </form>
    </div>
</template>