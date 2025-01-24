import { useFetch } from '#app';

export interface Project {
    name: string;
    technologies: string[];
    description: string;
}

const projectApi = {
    /**
     * Fetches all projects from the API.
     * 
     * @returns {Promise<Project[]>} A promise that resolves to an array of Project objects.
     * 
     * @example
     * import { api } from '@/services/api';
     *  
     * async function fetchProjects() {
     *     try {
     *         const projects = await api.project.getAll();
     *         console.log('Fetched projects:', projects);
     *     } catch (error) {
     *         console.error('Error fetching projects:', error);
     *     }
     * }
     */
    getAll: async (): Promise<Project[]> => {
        const baseUrl: string = process.env.NODE_ENV === 'production' ? '/api/templates' : 'http://localhost:5052/api/templates';
        const { data, error } = await useFetch<Project[]>(baseUrl);
        console.log(baseUrl);
        if (error.value) {
            console.error('Error fetching projects:', error.value);
            throw error.value;
        }
        return data.value || [];
    }
};

export const api = {
    project: projectApi,
    
};