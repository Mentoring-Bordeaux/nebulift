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
    '/api': {
      proxy: 'http://localhost:5052',
    },
  },
  compatibilityDate: '2024-04-03',
  typescript: {
    typeCheck: false,
  },
  runtimeConfig: {
    public: {
      apiBase: process.env.NODE_ENV === 'production' ? '/api' : 'http://localhost:5052'
    }
  }
  
},
);