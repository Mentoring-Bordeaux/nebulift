FROM node:20-alpine AS build

WORKDIR /app

# Installation de pnpm
RUN npm install -g pnpm

# Copie des fichiers de configuration uniquement d'abord
COPY package.json pnpm-lock.yaml ./
COPY  postcss.config.js ./
COPY nuxt.config.ts ./

# Installation des dépendances avec des versions spécifiques
RUN pnpm install
RUN pnpm add -D tailwindcss@3.3.5 postcss@8.4.31 autoprefixer@10.4.16 @nuxtjs/tailwindcss@6.10.0

# Copie du reste des fichiers
COPY . .

# Vérification de la structure
RUN ls -la
RUN cat tailwind.config.js
RUN cat nuxt.config.ts

# Build
RUN pnpm run build

FROM node:20-alpine

WORKDIR /app

COPY --from=build /app/.output .

ENV HOST=0.0.0.0
ENV PORT=3000

EXPOSE 3000

CMD ["node", "./server/index.mjs"]
