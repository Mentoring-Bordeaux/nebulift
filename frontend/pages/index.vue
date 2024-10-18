<script setup lang="ts">
const { data, status, error, refresh, clear } = await useFetch(
  '/api/weatherforecast'
);

const clearAndRefresh = () => {
  clear();
  refresh();
};
</script>

<template>
  <div>
    <p v-if="status === 'pending'">Loading...</p>
    <p v-else-if="status === 'success'">
      {{ data }}
    </p>
    <p v-else-if="status === 'error'">
      {{ error }}
    </p>
    <UButton @click="clearAndRefresh"> Refresh </UButton>
  </div>
</template>

<style scoped>
p {
  text-align: center;
  font-size: 2em;
  padding-bottom: 1em;
}

div {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 100vh;
}
</style>
