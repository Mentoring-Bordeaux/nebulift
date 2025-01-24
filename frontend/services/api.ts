// Remove useFetch import
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
        console.log(baseUrl);
        
        try {
            return await $fetch<Project[]>(baseUrl);
        } catch (error) {
            console.error('Error fetching projects:', error);
            throw error;
        }
    }
};

export const api = {
    project: projectApi,
};