#!/bin/sh
set -e

echo "=== CUSTOM MINIO CONTAINER STARTING ==="

# Fonction de nettoyage : tue MinIO et la boucle de health-check
cleanup() {
  echo "=== Signal reçu, nettoyage des processus ==="
  # Si la boucle health-check tourne, la tuer
  if [ -n "$health_loop_pid" ]; then
    echo "Terminaison de la boucle health-check (PID $health_loop_pid)..."
    kill "$health_loop_pid" 2>/dev/null || true
    # on peut attendre un peu pour s'assurer qu'il s'arrête
    wait "$health_loop_pid" 2>/dev/null || true
  fi
  # Tuer MinIO si toujours vivant
  if [ -n "$server_pid" ]; then
    echo "Terminaison du serveur MinIO (PID $server_pid)..."
    kill "$server_pid" 2>/dev/null || true
    wait "$server_pid" 2>/dev/null || true
  fi
  echo "=== Nettoyage terminé, sortie du script ==="
  exit 0
}
trap cleanup INT TERM

# Préparation du dossier de données
mkdir -p /app
# Si besoin : ajuster ownership si MinIO tourne sous un autre utilisateur
# chown -R minio-user:minio-user /app

echo "Starting MinIO server..."
# Lancement du serveur MinIO en arrière-plan
/usr/bin/docker-entrypoint.sh minio server /app --console-address :9001 &
server_pid=$!

# Attente de la readiness de MinIO
echo "Waiting for MinIO to be ready..."
# On attend l’endpoint health ; adaptez l’URL selon la version de MinIO
# Exemple : /minio/health/ready. Si absent, tester la racine.
until curl --silent --fail http://127.0.0.1:9000/minio/health/ready; do
  echo "MinIO pas encore prêt, nouvelle vérification dans 1s..."
  sleep 1
done

echo "MinIO prêt, configuration mc alias..."
# Configuration de l’alias mc (on peut tester 127.0.0.1 au lieu de localhost)
if mc alias set local http://127.0.0.1:9000 "$MINIO_ROOT_USER" "$MINIO_ROOT_PASSWORD"; then
  echo "Alias mc configuré avec succès."
else
  echo "Warning: impossible de configurer alias mc, tentative ignorée."
fi

# Création des buckets (ignore si déjà existants)
echo "Creating buckets..."
mc mb -p local/test-bucket || echo "Bucket test-bucket existe déjà ou création échouée."
mc mb -p local/uploads || echo "Bucket uploads existe déjà ou création échouée."

# Démarrage de la boucle health-check en arrière-plan
echo "Démarrage de la boucle de health-check (deep-listing) en arrière-plan..."
health_check_loop() {
  while true; do
    ts=$(date --iso-8601=seconds)
    echo "[${ts}] Deep-listing all objects in all buckets:"
    # On liste récursivement, capture d’erreur pour ne pas stopper la boucle si erreur temporaire
    if ! mc ls --recursive local; then
      echo "[${ts}] Warning: mc ls a échoué."
    fi
    sleep 10
  done
}
health_check_loop &
health_loop_pid=$!

# Attente du serveur MinIO. Quand il s’arrête, on sort de wait et on nettoie.
echo "Entrée dans l’attente du processus MinIO (PID $server_pid)..."
wait "$server_pid"

# Si on arrive ici, serveur MinIO s'est terminé : on doit arrêter la boucle health-check
echo "Le serveur MinIO (PID $server_pid) est terminé. Arrêt de la boucle health-check..."
if [ -n "$health_loop_pid" ]; then
  kill "$health_loop_pid" 2>/dev/null || true
  wait "$health_loop_pid" 2>/dev/null || true
fi

echo "Sortie du script custom-entrypoint.sh."
exit 0
