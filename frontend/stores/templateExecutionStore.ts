// stores/templateExecutionStore.ts

import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { TemplateOutput, ExecutionState } from '@/types/template';
import { templateService } from '@/services/template.service';

export const useTemplateExecutionStore = defineStore('templateExecution', () => {
    // State
    const isExecuting = ref<boolean>(false);
    const output = ref<TemplateOutput | null>(null);
    const error = ref<string | null>(null);
    const templateId = ref<string | null>(null);

    // Actions
    const resetState = () => {
        isExecuting.value = false;
        output.value = null;
        error.value = null;
        templateId.value = null;
    };

    const handleTemplateExecution = async (id: string, formData: Record<string, any>) => {
        isExecuting.value = true;
        error.value = null;
        templateId.value = id;

        try {
            const response = await templateService.submitConfig(id, formData);
            
            // Type assertion for the response
            const outputData = response as TemplateOutput;
            output.value = outputData;

            return outputData;
        } catch (err) {
            error.value = err instanceof Error ? err.message : 'Template execution failed';
            throw err;
        } finally {
            isExecuting.value = false;
        }
    };

    // Computed state
    const executionState = computed((): ExecutionState => ({
        isExecuting: isExecuting.value,
        output: output.value,
        error: error.value,
        templateId: templateId.value
    }));

    return {
        // State
        isExecuting,
        output,
        error,
        templateId,
        executionState,
        // Actions
        resetState,
        handleTemplateExecution
    };
});