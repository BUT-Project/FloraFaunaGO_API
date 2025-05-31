FROM python:3.11-slim

WORKDIR /app

COPY requirements.txt ./
COPY . .

# Installer dépendances système
RUN apt-get update && apt-get install -y --no-install-recommends \
    git \
    libgl1 \
    libglib2.0-0 \
    && rm -rf /var/lib/apt/lists/*

# Installer pip deps
RUN pip install --no-cache-dir --upgrade pip \
 && pip install --no-cache-dir speciesnet \
 && pip install --no-cache-dir -r requirements.txt

# Supprimer toute mauvaise installation de yolov5
RUN pip uninstall -y yolov5 || true

# Cloner exactement YOLOv5 v4.0
RUN git clone --branch v4.0 https://github.com/ultralytics/yolov5.git /yolov5 \
 && pip install --no-cache-dir -r /yolov5/requirements.txt

# Fixe PYTHONPATH pour importer le bon yolov5
ENV PYTHONPATH=/yolov5:$PYTHONPATH

EXPOSE 8001

CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8001"]

