// Remove useFetch import, no need for it
import type { TemplateSchema, FormData } from '@/types/template';

/**
 * Service for handling template-related API operations
 */
class TemplateService {
    private readonly baseUrl: string = process.env.NODE_ENV === 'production' ? '/api/templates' : 'http://localhost:5052/api/templates';

    /**
     * Fetches template configuration schema by template ID
     */
    async getTemplateConfig(id: string): Promise<TemplateSchema> {
        try {
            return await $fetch<TemplateSchema>(`${this.baseUrl}/${id}`);
        } catch (error) {
            console.error('Error fetching template config:', error);
            throw error;
        }
    }

    /**
     * Submits template configuration data
     */
    async submitConfig(id: string, formData: FormData): Promise<void> {
        try {
            await $fetch(`${this.baseUrl}/${id}`, {
                method: 'POST',
                body: formData  // Keep as FormData, no JSON.stringify
            });
        } catch (error) {
            console.error('Error submitting template config:', error);
            throw error;
        }
    }
}

export const templateService = new TemplateService();