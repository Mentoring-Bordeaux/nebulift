<template>
  <header
    class="header sticky w-full z-50 bg-orange-600 text-white h-20 flex items-center"
  >
    <div class="header-content flex items-center h-full relative z-10">
      <h1 class="logo text-4xl font-bold pl-6">Nebulift</h1>
    </div>
    <div class="header-shape absolute bottom-0 right-0 h-full w-52 bg-orange-500 clip-path-s-shape"></div>
  </header>
</template>

<script>
export default {
  name: 'Header',
};
</script>
