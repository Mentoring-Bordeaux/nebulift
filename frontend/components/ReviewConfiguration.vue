<script setup lang="ts">
import { useRoute, useRouter } from 'vue-router';
import { useTemplateExecutionStore } from '@/stores/templateExecutionStore';
import type { TemplateSchema, FormData } from '~/types/template';

interface Props {
   formData: FormData;
   schema: TemplateSchema;
   errors?: Record<string, string>;
}

const props = withDefaults(defineProps<Props>(), {
   errors: () => ({})
});

const emit = defineEmits(['back']);
const router = useRouter();
const route = useRoute();
const executionStore = useTemplateExecutionStore();

const handleCreate = async () => {
  try {
    const templateId = route.params.name as string;
    executionStore.isExecuting = true; 
    await executionStore.handleTemplateExecution(templateId, props.formData);
    router.push(`/projects/execution/${templateId}`);
  } catch (error) {
    console.error('Execution error:', error);
  } finally {
    executionStore.isExecuting = false; 
  }
};
</script>

<template>
  <div v-if="executionStore.isExecuting" class="flex items-center justify-center min-h-screen">
    <div class="text-center">
      <div class="animate-spin rounded-full h-16 w-16 border-t-4 border-orange-500 border-solid mb-4"></div>
      <p class="text-gray-600">Creating your project...</p>
    </div>
  </div>

   <div class="space-y-6 text-black">
       <h2 class="text-xl font-semibold mb-4">Review your configuration</h2>

       <div v-for="(section, sectionKey) in props.formData" :key="sectionKey" class="mb-6">
           <h3 class="text-lg font-semibold mb-2">• {{ props.schema[sectionKey]?.title || sectionKey }}</h3>
           <div class="bg-gray-50 p-4 rounded-md">
               <div v-for="(value, fieldKey) in section" :key="fieldKey" class="mb-2">
                   <strong>- {{ props.schema[sectionKey]?.properties[fieldKey]?.title || fieldKey }} :</strong> {{ value }}
               </div>
           </div>
       </div>

       <div class="flex justify-between mt-6">
           <button 
               type="button" 
               class="px-4 py-2 w-24 bg-gray-600 text-white rounded-xl hover:bg-gray-700 
                       focus:outline-none focus:ring-2 focus:ring-gray-500 focus:ring-offset-2"
               @click="emit('back')" 
           >
               Back
           </button>
           <button 
               type="button" 
               class="px-4 py-2 w-32 bg-orange-600 text-white rounded-2xl hover:bg-orange-500 
                       focus:outline-none focus:ring-2 focus:ring-orange-700 focus:ring-offset-2 
                       disabled:opacity-50 disabled:cursor-not-allowed font-bold"
               @click="handleCreate" 
           >
               Create
           </button>
       </div>
   </div>
</template>