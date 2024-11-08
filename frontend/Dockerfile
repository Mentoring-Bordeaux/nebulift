FROM node:20-alpine AS build

WORKDIR /app

# Installation de pnpm
RUN wget -qO- https://get.pnpm.io/install.sh | ENV="$HOME/.bashrc" SHELL="$(which bash)" bash -


# Copie des fichiers de configuration uniquement d'abord
COPY package.json pnpm-lock.yaml ./
COPY nuxt.config.ts ./

# Installation des dépendances avec des versions spécifiques
RUN pnpm install

# Copie du reste des fichiers
COPY . .


# Build
RUN pnpm run build

FROM node:20-alpine

WORKDIR /app

COPY --from=build /app/.output .

ENV HOST=0.0.0.0
ENV PORT=3000

EXPOSE 3000

CMD ["node", "./server/index.mjs"]