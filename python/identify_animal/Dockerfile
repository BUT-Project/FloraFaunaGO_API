FROM python:3.11-slim

WORKDIR /app

COPY requirements.txt .
COPY main.py .
COPY . .

RUN pip install --no-cache-dir --upgrade pip \
 && pip install --no-cache-dir speciesnet \
 && pip install --no-cache-dir -e .[dev] \
 && pip install --no-cache-dir -r requirements.txt

EXPOSE 8001

CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8001"]
