<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useTemplateConfigStore } from '@/stores/templateConfigStore';
import { useProjectStore } from '@/stores/projectStore';
import DynamicForm from '@/components/form/DynamicForm.vue';
import Header from '@/components/Header.vue';

// Route and Store Management
const route = useRoute();
const router = useRouter();
const templateConfigStore = useTemplateConfigStore();
const projectStore = useProjectStore();

// Local State Management
const isSubmitting = ref(false);
const localError = ref<string | null>(null);

// Computed Properties
const templateName = computed(() => route.params.name as string);
const selectedTemplate = computed(() =>
  projectStore.projects.find((p) => p.name === templateName.value)
);

const pageTitle = computed(() => {
  const baseName = selectedTemplate.value?.name || 'Template';
  return `Configure ${baseName}`;
});

// Lifecycle Hooks
onMounted(async () => {
  try {
    // Verify template exists in project store
    if (!selectedTemplate.value) {
      await projectStore.fetchProjects();

      if (!selectedTemplate.value) {
        localError.value = 'Template not found';
        router.push('/');
        return;
      }
    }

    // Fetch template configuration
    await templateConfigStore.fetchTemplateConfig(templateName.value);
  } catch (error) {
    localError.value = 'Failed to load template configuration';
    console.error('Template loading error:', error);
  }
});

// Event Handlers
const handleSubmitSuccess = async () => {
  try {
    isSubmitting.value = true;
    await templateConfigStore.submitConfig();

    // Navigate to execution results page
    router.push({
      path: `/projects/execution/${templateName.value}`,
    });
  } catch (error) {
    localError.value = 'Failed to submit configuration';
    console.error('Submission error:', error);
  } finally {
    isSubmitting.value = false;
  }
};

const handleRetry = async () => {
  localError.value = null;
  await templateConfigStore.fetchTemplateConfig(templateName.value);
};
</script>

<template>
  <div class="min-h-screen bg-gray-50">
    <Header />

    <main class="container mx-auto px-4 py-8 max-w-7xl">
      <!-- Page Header -->
      <div class="mb-8">
        <div class="flex items-center justify-between">
          <h1 class="text-2xl font-bold text-gray-900">
            {{ pageTitle }}
          </h1>

          <button
            class="text-gray-600 hover:text-gray-900"
            @click="router.push('/')"
          >
            Back to Templates
          </button>
        </div>

        <div v-if="selectedTemplate" class="mt-2">
          <p class="text-gray-600">
            Technologies: {{ selectedTemplate.technologies.join(', ') }}
          </p>
        </div>
      </div>

      <!-- Error States -->
      <div
        v-if="localError || templateConfigStore.error"
        class="bg-red-50 border border-red-200 rounded-md p-4 mb-6"
      >
        <div class="flex items-start">
          <div class="flex-shrink-0">
            <!-- Error Icon -->
            <svg
              class="h-5 w-5 text-red-400"
              viewBox="0 0 20 20"
              fill="currentColor"
            >
              <path
                fill-rule="evenodd"
                d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z"
              />
            </svg>
          </div>

          <div class="ml-3">
            <h3 class="text-sm font-medium text-red-800">
              Configuration Error
            </h3>
            <p class="text-sm text-red-700 mt-1">
              {{ localError || templateConfigStore.error }}
            </p>
            <button
              class="mt-2 text-sm text-red-700 hover:text-red-600 font-medium underline"
              @click="handleRetry"
            >
              Retry
            </button>
          </div>
        </div>
      </div>

      <!-- Loading State -->
      <div
        v-if="templateConfigStore.loading"
        class="flex justify-center items-center py-12"
      >
        <div
          class="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"
        ></div>
        <span class="ml-3 text-gray-600"
          >Loading template configuration...</span
        >
      </div>

      <!-- Form Content -->
      <div v-else-if="templateConfigStore.schema">
        <DynamicForm
          :is-submitting="isSubmitting"
          @submit-success="handleSubmitSuccess"
        />
      </div>
    </main>
  </div>
</template>

<style scoped>
.container {
  @apply mx-auto px-4 sm:px-6 lg:px-8;
}

.animate-spin {
  animation: spin 1s linear infinite;
}

@keyframes spin {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
}
</style>
