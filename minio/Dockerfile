FROM minio/minio:latest

# Create a startup script with logging
RUN echo '#!/bin/sh' > /custom-entrypoint.sh && \
    echo 'echo "=== CUSTOM MINIO CONTAINER STARTING ==="' >> /custom-entrypoint.sh && \
    echo 'echo "Data directory: /app"' >> /custom-entrypoint.sh && \
    echo 'echo "Checking /app directory..."' >> /custom-entrypoint.sh && \
    echo 'ls -la /app || echo "/app directory not found, creating..."' >> /custom-entrypoint.sh && \
    echo 'mkdir -p /app' >> /custom-entrypoint.sh && \
    echo 'echo "Directory permissions:"' >> /custom-entrypoint.sh && \
    echo 'ls -la /app' >> /custom-entrypoint.sh && \
    echo 'echo "Starting MinIO server with /app as data directory..."' >> /custom-entrypoint.sh && \
    echo 'exec /usr/bin/docker-entrypoint.sh minio server /app --console-address :9001' >> /custom-entrypoint.sh && \
    chmod +x /custom-entrypoint.sh

# Use our custom entrypoint with logging
ENTRYPOINT ["/custom-entrypoint.sh"]