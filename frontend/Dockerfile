FROM node:20-alpine AS build

WORKDIR /app

# Installation de pnpm
RUN npm install -g pnpm

# Copie des fichiers de dépendances
COPY package.json pnpm-lock.yaml ./

# Installation des dépendances explicites de Tailwind
RUN pnpm add -D tailwindcss@latest postcss@latest autoprefixer@latest

# Installation normale des dépendances
RUN pnpm install

# Copie du reste des fichiers
COPY . .

# Génération du fichier de configuration Tailwind si nécessaire
RUN npx tailwindcss init -p

# Build
RUN pnpm run build

FROM node:20-alpine

WORKDIR /app

# Copie des fichiers de build
COPY --from=build /app/.output .

# Configuration d'environnement
ENV HOST=0.0.0.0
ENV PORT=3000

EXPOSE 3000

# Démarrage de l'application
CMD ["node", "./server/index.mjs"]