#!/bin/bash

# Définition des couleurs
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color
BLUE='\033[0;34m'

# Fonction pour afficher les messages
log() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Vérifier si les ports sont disponibles
check_ports() {
    if lsof -i:5052 > /dev/null || lsof -i:3000 > /dev/null; then
        error "Les ports 5052 ou 3000 sont déjà utilisés!"
        error "Vérifiez qu'aucune autre application n'utilise ces ports"
        exit 1
    fi
}

# Démarrer le backend
start_backend() {
    log "Démarrage du backend..."
    cd backend || exit
    dotnet restore
    dotnet build
    dotnet run --project src/Nebulift.Api/Nebulift.Api.csproj &
    BACKEND_PID=$!
    cd ..
    success "Backend démarré sur http://localhost:5052"
}

# Démarrer le frontend
start_frontend() {
    log "Démarrage du frontend..."
    cd frontend || exit
    pnpm install
    pnpm dev &
    FRONTEND_PID=$!
    cd ..
    success "Frontend démarré sur http://localhost:3000"
}

# Arrêter les processus au CTRL+C
cleanup() {
    log "Arrêt des services..."
    kill $BACKEND_PID 2>/dev/null
    kill $FRONTEND_PID 2>/dev/null
    exit 0
}

# Menu principal
show_menu() {
    echo -e "\n${BLUE}=== Menu de développement local ===${NC}"
    echo "1. Démarrer les services"
    echo "2. Ouvrir dans Chrome"
    echo "3. Tester l'API"
    echo "4. Arrêter les services"
    echo "5. Quitter"
    echo -n "Sélectionnez une option: "
}

# Ouvrir dans Chrome
open_chrome() {
    log "Ouverture des URLs dans Chrome..."
    open -a "Google Chrome" http://localhost:3000
    open -a "Google Chrome" http://localhost:5052/swagger
}

# Tester l'API
test_api() {
    log "Test de l'API..."
    curl -s http://localhost:5052/weatherforecast | json_pp
}

# Programme principal
main() {
    check_ports
    
    # Mettre en place le trap pour le nettoyage
    trap cleanup SIGINT SIGTERM

    while true; do
        show_menu
        read -r choice

        case $choice in
            1)
                start_backend
                sleep 5  # Attendre que le backend démarre
                start_frontend
                success "Services démarrés! Utilisez CTRL+C pour arrêter"
                ;;
            2)
                open_chrome
                ;;
            3)
                test_api
                ;;
            4)
                cleanup
                ;;
            5)
                cleanup
                ;;
            *)
                error "Option invalide"
                ;;
        esac
    done
}

# Démarrer le programme
main