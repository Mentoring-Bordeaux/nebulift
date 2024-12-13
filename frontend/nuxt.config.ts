// nuxt.config.ts
export default defineNuxtConfig({
  modules: ['@nuxt/ui'],
  ssr: false,
  devtools: { enabled: true },
  css: ['@/assets/main.css'],
  alias: {
    assets: '/<rootDir>/assets',
  },
  routeRules: {
    '/api/**': {
      proxy: process.env.NODE_ENV === 'production' 
        ? '/api/**'  // En production, utilise le chemin d'API Azure Static Web Apps
        : 'http://localhost:5052/**' // En développement, pointe vers l'API locale
    },
  },
  compatibilityDate: '2024-04-03',
  typescript: {
    typeCheck: false,
  },
  runtimeConfig: {
    public: {
      apiBase: process.env.NODE_ENV === 'production' ? '/api' : 'http://localhost:5000'
    }
  }
  
},
);