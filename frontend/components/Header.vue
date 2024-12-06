<template>
  <header class="header">
    <div class="header-content">
      <h1 class="logo">Nebulift</h1>
    </div>
    <div class="header-shape"></div>
  </header>
</template>

<script>
export default {
  name: "Header",
};
</script>

<style scoped>
.header {
  background-color: #FB6028;
  padding: 1rem;
  color: white;
  width: 100%;
  position: fixed;
  top: 0;
  left: 0;
  z-index: 1000;
  overflow: hidden;
  height: 80px;
}

.header-content {
  display: flex;
  align-items: center;
  position: relative;
  z-index: 10;
  height: 100%;
}

.logo {
  font-size: 2.5rem;
  font-weight: bold;
  align-content: center;
  padding-left: 1.5rem;
}

.header-shape {
  position: absolute;
  bottom: 0;
  right: 0;
  height: 100%;
  width: 200px;
  background: #ff7a00;
  clip-path: ellipse(100% 100% at 100% 100%);
}
</style>