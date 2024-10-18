// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2024-04-03',
  alias: {
    assets: "/<rootDir>/assets",
  },
  nitro: {
    devProxy: {
      "/api": {
        target: "http://localhost:5041",
      },
    },
  },
  routeRules: {
    "/api": {
      proxy: "http://localhost:5041",
    },
  },

  modules: ["@nuxt/ui"],
  css: ["@/assets/main.css"],
  devtools: { enabled: true },
  ssr: true,
  typescript: {
    typeCheck: true,
  }
})