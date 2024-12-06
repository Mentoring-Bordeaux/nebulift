<template>
  <header class="header fixed top-0 left-0 w-full z-50 bg-orange-600 text-white h-20 flex items-center">
    <div class="header-content flex items-center h-full relative z-10">
      <h1 class="logo text-4xl font-bold pl-6">Nebulift</h1>
    </div>
    <div class="header-shape absolute bottom-0 right-0 h-full w-52 bg-orange-500 clip-path-s-shape"></div>
  </header>
</template>

<script>
export default {
  name: "Header",
};
</script>

<style scoped>
.header-shape {
  clip-path: path('M 0 100 C 50 50, 100 50, 150 0');
}
</style>