<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useTemplateExecutionStore } from '@/stores/templateExecutionStore';
import { useTemplateConfigStore } from '@/stores/templateConfigStore';
import ResultsDisplay from '@/components/execution/ResultsDisplay.vue';
import ExecutionStatus from '@/components/execution/ExecutionStatus.vue';
import Header from '@/components/Header.vue';

const route = useRoute();
const router = useRouter();
const executionStore = useTemplateExecutionStore();
const configStore = useTemplateConfigStore();

const templateId = computed(() => route.params.id as string);
const isLoading = ref(false);
const localError = ref<string | null>(null);

const errorMessage = computed((): string => {
  return (
    localError.value || executionStore.error || 'An unexpected error occurred'
  );
});

onMounted(async () => {
  if (!configStore.formData || !templateId.value) {
    router.push('/');
    return;
  }

  try {
    isLoading.value = true;
    await executionStore.handleTemplateExecution(
      templateId.value,
      configStore.formData
    );
  } catch (error) {
    localError.value = 'Execution failed';
    console.error('Execution error:', error);
  } finally {
    isLoading.value = false;
  }
});

const handleRetry = async () => {
  localError.value = null;
  await executionStore.resetState();
  await executionStore.handleTemplateExecution(
    templateId.value,
    configStore.formData
  );
};
</script>

<template>
  <div class="min-h-screen bg-gray-50">
    <Header />

    <main class="container mx-auto py-8 px-4">
      <div class="max-w-4xl mx-auto">
        <ExecutionStatus v-if="isLoading" status="loading" />

        <ExecutionStatus
          v-else-if="localError || executionStore.error"
          status="error"
          :message="errorMessage"
        >
          <button
            @click="handleRetry"
            class="mt-4 px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
          >
            Retry Execution
          </button>
        </ExecutionStatus>

        <ResultsDisplay
          v-else-if="executionStore.output"
          :output="executionStore.output"
        />
      </div>
    </main>
  </div>
</template>
