// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  modules: ['@nuxt/ui', '@nuxt/eslint'],
  ssr: true,
  devtools: { enabled: true },
  css: ['@/assets/main.css'],
  alias: {
    assets: '/<rootDir>/assets',
  },
  routeRules: {
    '/api': {
      proxy: 'http://localhost:5041',
    },
  },
  compatibilityDate: '2024-04-03',
  nitro: {
    devProxy: {
      '/api': {
        target: 'http://localhost:5041',
      },
    },
  },
  typescript: {
    typeCheck: true,
  },
});
