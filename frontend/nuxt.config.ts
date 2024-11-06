// nuxt.config.ts
export default defineNuxtConfig({
  modules: ['@nuxt/ui', '@nuxt/eslint'],
  ssr: true,
  devtools: { enabled: true },
  css: ['@/assets/main.css'],
  alias: {
    assets: '/<rootDir>/assets',
  },
  routeRules: {
    '/api/**': {
      proxy: 'http://backend:80/**',
    },
  },
  compatibilityDate: '2024-04-03',
  typescript: {
    typeCheck: true,
  },
});
