import { defineStore } from 'pinia';
import { api } from '@/services/api';

export const useProjectStore = defineStore('projectStore', {
  state: () => ({
    projects: [] as Array<{ name: string; technologies: string[] }>,
  }),
  actions: {
    async fetchProjects() {
      if (this.projects.length === 0) {
        try {
          this.projects = await api.project.getAll();
        } catch (error) {
          console.error('Failed to fetch projects:', error);
        }
      }
    },
  },
});