FROM node:20 AS build
WORKDIR /app
COPY package.json ./
COPY pnpm-lock.yaml ./
RUN npm install -g pnpm
RUN pnpm install
COPY . ./
RUN pnpm run build

# Utiliser une image allégée pour servir les fichiers statiques
FROM node:20-alpine AS production
WORKDIR /app
COPY --from=build /app/.output ./
EXPOSE 3000
CMD ["node", "./server/index.mjs"]
