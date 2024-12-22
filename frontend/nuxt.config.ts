// nuxt.config.ts
export default defineNuxtConfig({
  modules: ['@nuxt/ui', '@nuxt/eslint', '@pinia/nuxt'],
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
});
