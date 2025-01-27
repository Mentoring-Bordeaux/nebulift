<script setup lang="ts">
interface Props {
  status: 'loading' | 'error' | 'success';
  message?: string | null;
}

const props = withDefaults(defineProps<Props>(), {
  message: null,
});

// Create a computed property for safe message handling
const displayMessage = computed(() => {
  if (!props.message) {
    return {
      loading: 'Processing execution...',
      error: 'An error occurred during execution',
      success: 'Execution completed successfully',
    }[props.status];
  }
  return props.message;
});

const statusConfig = computed(
  () =>
    ({
      loading: {
        icon: 'animate-spin',
        class: 'text-blue-600',
        title: 'Processing',
      },
      error: {
        icon: 'x-circle',
        class: 'text-red-600',
        title: 'Error',
      },
      success: {
        icon: 'check-circle',
        class: 'text-green-600',
        title: 'Success',
      },
    })[props.status]
);
</script>

<template>
  <div
    class="rounded-lg p-6"
    :class="{
      'bg-blue-50 border-blue-200': status === 'loading',
      'bg-red-50 border-red-200': status === 'error',
      'bg-green-50 border-green-200': status === 'success',
    }"
  >
    <div class="flex items-center">
      <div :class="[statusConfig.class, 'mr-3']">
        <i :class="statusConfig.icon" />
      </div>
      <div>
        <h3 class="text-lg font-medium" :class="statusConfig.class">
          {{ statusConfig.title }}
        </h3>
        <p class="mt-1 text-sm text-gray-600">{{ displayMessage }}</p>
      </div>
    </div>

    <slot></slot>
  </div>
</template>
