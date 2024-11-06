#!/bin/bash

echo "🧹 Nettoyage de l'environnement Docker..."
docker compose down
docker system prune -f

echo "🏗️  Construction des images Docker..."
docker compose build --no-cache

echo "🚀 Démarrage des conteneurs..."
docker compose up -d

echo "⏳ Attente du démarrage des services..."
sleep 10

echo "🔍 Test du backend..."
curl http://localhost:5052/weatherforecast
echo -e "\n"

echo "🔍 Test du frontend..."
open -a "Google Chrome" http://localhost:3000

echo -e "\n"

echo "📝 Affichage des logs..."
docker compose logs

echo "💡 Pour arrêter les conteneurs, utilisez: docker compose down"