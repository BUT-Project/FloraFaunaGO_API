FROM python:3.11-slim

WORKDIR /app

COPY requirements.txt ./
COPY . .

# Install system dependencies
RUN apt-get update && apt-get install -y --no-install-recommends \
    git \
    libgl1 \
    libglib2.0-0 \
    && rm -rf /var/lib/apt/lists/*

# Install pip packages (speciesnet and requirements)
RUN pip install --no-cache-dir --upgrade pip \
 && pip install --no-cache-dir speciesnet \
 && pip install --no-cache-dir -r requirements.txt

# REMOVE BAD yolov5
RUN pip uninstall -y yolov5 || true

# Clone yolov5 at v6.2
RUN git clone --branch v6.2 https://github.com/ultralytics/yolov5.git /yolov5 \
 && pip install --no-cache-dir -r /yolov5/requirements.txt

# Force Python to use the local yolov5
ENV PYTHONPATH=/yolov5:$PYTHONPATH

EXPOSE 8001

CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8001"]


