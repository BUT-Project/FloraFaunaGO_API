FROM minio/minio:latest

# Copier le client mc depuis l’image officielle
COPY --from=minio/mc:latest /usr/bin/mc /usr/bin/mc

# Copier le script d’entrée personnalisé dans l’image
COPY custom-entrypoint.sh /custom-entrypoint.sh
RUN chmod +x /custom-entrypoint.sh

# Variables d’environnement par défaut (peuvent être surchargées au runtime)
ENV MINIO_ROOT_USER=minioadmin
ENV MINIO_ROOT_PASSWORD=minioadmin123

# Exposer les ports MinIO (API) et Console
EXPOSE 9000 9001

# Lancer le script d’entrée
ENTRYPOINT ["/custom-entrypoint.sh"]
