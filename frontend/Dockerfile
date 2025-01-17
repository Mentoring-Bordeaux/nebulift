# Use the Node.js 20 Alpine image as the base image for the build stage
FROM node:20-alpine AS build

# Set the working directory inside the container
WORKDIR /app

# Install wget for downloading pnpm installation script
RUN apk add --no-cache wget

# Set the SHELL environment variable to use /bin/sh
ENV SHELL=/bin/sh

# Install pnpm using the wget command and the install script
RUN wget -qO- https://get.pnpm.io/install.sh | ENV="$HOME/.shrc" sh -

# Add pnpm to the PATH environment variable
ENV PATH="/root/.local/share/pnpm:$PATH"

# Copy only configuration files first to leverage Docker layer caching
COPY package.json pnpm-lock.yaml ./
COPY nuxt.config.ts ./

# Copy the remaining application files
COPY . .

# Install project dependencies using pnpm
RUN pnpm install

# Build the project
RUN pnpm run build

# Use the Node.js 20 Alpine image as the base image for the runtime stage
FROM node:20-alpine

# Set the working directory inside the container
WORKDIR /app

# Copy the build output from the build stage to the runtime stage
COPY --from=build /app/.output .

# Set environment variables for the application
ENV HOST=0.0.0.0
ENV PORT=3000

# Expose the application port
EXPOSE 3000

# Start the application using the Node.js server
CMD ["node", "./server/index.mjs"]
