// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  modules: ['@nuxt/ui', '@nuxt/eslint', '@pinia/nuxt'],
  ssr: false,
  devtools: { enabled: true },
  css: ['@/assets/main.css'],
  alias: {
    assets: '/<rootDir>/assets',
  },
  routeRules: {
    // '/api': {
    //   proxy: process.env.NUXT_PUBLIC_API_URL || 'http://localhost:5052',
    // },
    // '/projects/execution/:id': {
    //   ssr: true,
    // },
  },
  compatibilityDate: '2024-04-03',
  future: {
    compatibilityVersion: 4
  },
  nitro: {
    devProxy: {
      '/api': {
        target: process.env.NUXT_PUBLIC_API_URL || 'http://localhost:5052',
      },
    },
  },
  typescript: {
    typeCheck: true,
  },
  postcss: {
    plugins: {
      'tailwindcss': {},
      'autoprefixer': {},
    },
  },
  runtimeConfig: {
    public: {
      otelExporterOtlpEndpoint: '',
      otelExporterOtlpHeaders: '',
      otelResourceAttributes: '',
      otelServiceName: '',
      apiUrl: '',
    }
  }
});
