import { defineStore } from 'pinia';
import { ref } from 'vue';
import { api } from '@/services/api';
import type { Project } from '@/services/api';

export const useProjectStore = defineStore('projects', () => {
  const projects = ref<Project[]>([]);
  const error = ref<string | null>(null);

  const fetchProjects = async () => {
    try {
      const fetchedProjects = await api.project.getAll();
      projects.value = fetchedProjects.map(item => ({
        name: item.name,
        technologies: item.technologies
      }));
    } catch (err) {
      error.value = 'Error fetching projects';
      console.error(err);
    }
  };

  return { projects, error, fetchProjects };
});