// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2024-04-03',
  alias: {
    assets: "/<rootDir>/assets",
  },
  modules: ["@nuxt/ui"],
  css: ["@/assets/main.css"],
  devtools: { enabled: true },
  ssr: false,
  typescript: {
    typeCheck: true,
  }
})