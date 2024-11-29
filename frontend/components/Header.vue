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
    height: 80px; /* Hauteur du header */
  }
  
  .header-content {
    display: flex;
    align-items: center; /* Center vertically */
    position: relative;
    z-index: 10; /* Assure que le contenu est devant la forme */
    height: 100%; /* Ensure it takes the full height of the header */
  }
  
  .logo {
    font-size: 2.5rem;
    font-weight: bold;
    align-content: center;
    padding-left: 1.5rem;
  }
  
  .header-shape {
    position: absolute;
    top: 0;
    right: 0;
    width: 100px; /* Adjust the width as needed */
    height: 100%;
    background-color: #FF983E; /* Example color for the shape */
    clip-path: polygon(100% 0, 0 100%, 100% 100%); /* Triangle shape */
    z-index: 0;
  }
  </style>