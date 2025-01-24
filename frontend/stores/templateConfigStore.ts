import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { TemplateSchema, FormData } from '@/types/template';
import { templateService } from '@/services/template.service';

export const useTemplateConfigStore = defineStore('templateConfig', () => {
    // State
    const selectedTemplate = ref<string | null>(null);
    const schema = ref<TemplateSchema | null>(null);
    const formData = ref<FormData>({});
    const loading = ref(false);
    const error = ref<string | null>(null);

    // Actions
    const fetchTemplateConfig = async (templateId: string) => {
        loading.value = true;
        error.value = null;

        try {
            selectedTemplate.value = templateId;
            schema.value = await templateService.getTemplateConfig(templateId);
            // Initialize form data structure based on schema
            initializeFormData();
        } catch (err) {
            error.value = 'Failed to fetch template configuration';
            console.error(err);
        } finally {
            loading.value = false;
        }
    };

    const initializeFormData = () => {
        if (!schema.value) return;

        formData.value = Object.keys(schema.value).reduce((acc, sectionKey) => {
            acc[sectionKey] = Object.keys(schema.value![sectionKey].properties).reduce((fields, fieldName) => {
                fields[fieldName] = '';
                return fields;
            }, {} as Record<string, string>);
            return acc;
        }, {} as FormData);
    };

    const submitConfig = async () => {
        if (!selectedTemplate.value) return;

        loading.value = true;
        error.value = null;

        try {
            await templateService.submitConfig(selectedTemplate.value, formData.value);
        } catch (err) {
            error.value = 'Failed to submit configuration';
            console.error(err);
        } finally {
            loading.value = false;
        }
    };

    return {
        // State
        selectedTemplate,
        schema,
        formData,
        loading,
        error,
        // Actions
        fetchTemplateConfig,
        submitConfig
    };
});