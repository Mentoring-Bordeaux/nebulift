// nuxt.config.ts
export default defineNuxtConfig({
  // Modules de base
  modules: ['@nuxt/ui'],
  
  devtools: { enabled: true },
  css: ['@/assets/main.css'],
  alias: {
    assets: '/<rootDir>/assets',
  },
  routeRules: {
    '/api/**': process.env.NODE_ENV === 'production'
      ? { proxy: '/api/**' }
      : { proxy: 'http://localhost:5000/**' },
  },
  
  // Configuration TypeScript conditionnelle
  typescript: {
    // Désactive la vérification des types en production
    typeCheck: process.env.NODE_ENV !== 'production',
    strict: true
  }
});