FROM minio/minio:latest

# Copier le client MinIO (mc)
COPY --from=minio/mc:latest /usr/bin/mc /usr/bin/mc

# Créer un script d'entrée personnalisé
RUN echo '#!/bin/sh' > /custom-entrypoint.sh && \
    echo 'echo "=== CUSTOM MINIO CONTAINER STARTING ==="' >> /custom-entrypoint.sh && \
    echo 'mkdir -p /app' >> /custom-entrypoint.sh && \
    echo 'echo "Starting MinIO server..."' >> /custom-entrypoint.sh && \
    echo '/usr/bin/docker-entrypoint.sh minio server /app --console-address :9001 &' >> /custom-entrypoint.sh && \
    echo 'server_pid=$!' >> /custom-entrypoint.sh && \
    echo 'echo "Waiting for MinIO to be ready..."' >> /custom-entrypoint.sh && \
    echo 'until mc alias set local http://localhost:9000 $MINIO_ROOT_USER $MINIO_ROOT_PASSWORD; do sleep 1; done' >> /custom-entrypoint.sh && \
    echo 'echo "Creating buckets..."' >> /custom-entrypoint.sh && \
    echo 'mc mb -p local/test-bucket || true' >> /custom-entrypoint.sh && \
    echo 'mc mb -p local/uploads || true' >> /custom-entrypoint.sh && \
    echo 'wait $server_pid' >> /custom-entrypoint.sh && \
    chmod +x /custom-entrypoint.sh

# Définir les variables d’environnement (identifiants)
ENV MINIO_ROOT_USER=minioadmin
ENV MINIO_ROOT_PASSWORD=minioadmin123

# Exposer les ports pour l'API et la console
EXPOSE 9000 9001

# Lancer notre script personnalisé
ENTRYPOINT ["/custom-entrypoint.sh"]