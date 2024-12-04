import axios from 'axios';

const apiClient = axios.create({
    baseURL: 'http://localhost:5052/api',
    headers: {
        'Content-Type': 'application/json',
    },
});

export interface Project {
    id: string;
    name: string;
    technologies: string[];
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
        try {
            const response = await apiClient.get<Project[]>('/templates');
            return response.data;
        } catch (error) {
            console.error('Error fetching projects:', error);
            throw error;
        }
    },
    
    /**
     * Fetches a project by its ID from the API.
     * 
     * @param {string} id - The ID of the project to fetch.
     * @returns {Promise<Project>} A promise that resolves to a Project object.
     * 
     * @example
     * import { api } from '@/services/api';
     * 
     * async function fetchProjectById(id: string) {
     *     try {
     *         const project = await api.project.getById(id);
     *         console.log('Fetched project:', project);
     *     } catch (error) {
     *         console.error(`Error fetching project with id ${id}:`, error);
     *     }
     * }
     */
    getById: async (id: string): Promise<Project> => {
        try {
            const response = await apiClient.get<Project>(`/templates/${id}`);
            return response.data;
        } catch (error) {
            console.error(`Error fetching project with id ${id}:`, error);
            throw error;
        }
    },
};

export const api = {
    project: projectApi,
};