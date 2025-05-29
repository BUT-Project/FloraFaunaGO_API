FROM python:3.11-slim

WORKDIR /app

COPY requirements.txt ./
COPY . .

RUN apt-get update && apt-get install -y --no-install-recommends \
    git \
    libgl1 \
    libglib2.0-0 \
    && rm -rf /var/lib/apt/lists/*

RUN pip install --no-cache-dir --upgrade pip \
 && pip install --no-cache-dir speciesnet \
 && pip install --no-cache-dir -r requirements.txt

RUN git clone --branch v6.2 https://github.com/ultralytics/yolov5.git /yolov5 \
 && pip install --no-cache-dir -r /yolov5/requirements.txt

ENV PYTHONPATH="${PYTHONPATH}:/yolov5"

EXPOSE 8001

CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8001"]

