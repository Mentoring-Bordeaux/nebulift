FROM node:20-alpine AS build

WORKDIR /app

# Installer wget
RUN apk add --no-cache wget

# Définir la variable d'environnement SHELL
ENV SHELL=/bin/sh

# Installer pnpm en utilisant sh et en définissant ENV et SHELL
RUN wget -qO- https://get.pnpm.io/install.sh | ENV="$HOME/.shrc" sh -

# Ajouter pnpm au PATH
ENV PATH="/root/.local/share/pnpm:$PATH"

# Copier uniquement les fichiers de configuration d'abord
COPY package.json pnpm-lock.yaml ./
COPY nuxt.config.ts ./
RUN ls -la
# Installer les dépendances
RUN pnpm install

# Copier le reste des fichiers
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
