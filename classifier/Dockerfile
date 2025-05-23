# Étape 1 : image de base avec Python
FROM python:3.10-slim

# Étape 2 : définir le dossier de travail
WORKDIR /app

# Étape 3 : copier les fichiers du projet
COPY requirements.txt .
COPY main.py .

# Étape 4 : installer les dépendances
RUN pip install --no-cache-dir --upgrade pip \
 && pip install --no-cache-dir -r requirements.txt

# Étape 5 : exposer le port
EXPOSE 8000

# Étape 6 : démarrer l'application
CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8000"]
