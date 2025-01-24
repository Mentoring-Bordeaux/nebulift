import { useFetch } from '#app';
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
        const { data, error } = await useFetch<TemplateSchema>(`${this.baseUrl}/${id}`);
        console.log(this.baseUrl);

        if (error.value) {
            console.error('Error fetching template config:', error.value);
            throw error.value;
        }

        return data.value as TemplateSchema;
    }

    /**
     * Submits template configuration data
     */
    async submitConfig(id: string, formData: FormData): Promise<void> {
        console.log('Submitting form data:', JSON.stringify(formData, null, 2));
        const { error } = await useFetch(`${this.baseUrl}/${id}`, {
            method: 'POST',
            body: JSON.stringify(formData)
        });

        if (error.value) {
            console.error('Error submitting template config:', error.value);
            throw error.value;
        }
    }
}

export const templateService = new TemplateService();