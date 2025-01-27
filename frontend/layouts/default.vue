<template>
  <div class="default-layout">
    <slot />
  </div>
</template>

<style scoped>
.default-layout {
  width: 100%;
  height: 100%; /* Full height of the container */
  display: flex;
  flex-direction: column; /* Arrange children in a column */
}
</style>
