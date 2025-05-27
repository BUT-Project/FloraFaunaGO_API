from fastapi import FastAPI, HTTPException
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from transformers import CLIPProcessor, CLIPModel
from PIL import Image
import torch
import io
import base64
import subprocess
import tempfile
import os
import json

app = FastAPI(title="FloraFaunaGo API", version="1.0")

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# CLIP-based classifier
model = CLIPModel.from_pretrained("openai/clip-vit-base-patch32")
processor = CLIPProcessor.from_pretrained("openai/clip-vit-base-patch32")
model.eval()

prompts = ["a photo of an animal", "a photo of an insect", "a photo of a plant"]
labels = ["animal", "insect", "plant"]

class ImageBase64Request(BaseModel):
    base64_image: str

@app.get("/health")
def health_check():
    return {"status": "ok"}

@app.post("/FloraFaunaGo_API/identification/classify")
async def classify_image(request: ImageBase64Request):
    try:
        base64_str = request.base64_image
        if "," in base64_str:
            base64_str = base64_str.split(",")[1]
        image_bytes = base64.b64decode(base64_str)
        image = Image.open(io.BytesIO(image_bytes)).convert("RGB")
    except Exception as e:
        raise HTTPException(status_code=400, detail=f"Invalid image format: {str(e)}")

    inputs = processor(text=prompts, images=image, return_tensors="pt", padding=True)
    with torch.no_grad():
        outputs = model(**inputs)
        logits_per_image = outputs.logits_per_image
        probs = logits_per_image.softmax(dim=1)

    predicted_idx = probs.argmax().item()
    predicted_label = labels[predicted_idx]
    return {"classification": predicted_label}