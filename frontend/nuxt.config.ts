// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  modules: ['@nuxt/ui', '@nuxt/eslint', '@pinia/nuxt'],
  ssr: true,
  devtools: { enabled: true },
  css: ['@/assets/main.css'],
  alias: {
    assets: '/<rootDir>/assets',
  },
  routeRules: {
    '/api': {
      proxy: 'http://localhost:5052',
    },'/projects/execution/:id': {
      ssr: true
    }

  },
  compatibilityDate: '2024-04-03',
  nitro: {
    devProxy: {
      '/api': {
        target: 'http://localhost:5052',
      },
    },
  },
  typescript: {
    typeCheck: true,
  },
});