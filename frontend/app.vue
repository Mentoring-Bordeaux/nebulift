<script setup lang="ts">
console.log('Hello from the frontend!');
</script>

<template>
  <div>
    <ClientOnly>
      <NuxtLayout>
        <NuxtPage />
      </NuxtLayout>
    </ClientOnly>
  </div>
</template>
