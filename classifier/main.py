from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from transformers import CLIPProcessor, CLIPModel
from PIL import Image
import torch
import io
import base64

app = FastAPI()

# Charger le modèle et le processor
model = CLIPModel.from_pretrained("openai/clip-vit-base-patch32")
processor = CLIPProcessor.from_pretrained("openai/clip-vit-base-patch32")
model.eval()

# Définir les prompts
prompts = ["a photo of an animal", "a photo of an insect", "a photo of a plant"]
labels = ["animal", "insect", "plant"]

# Modèle de requête avec image en base64
class ImageBase64Request(BaseModel):
    base64_image: str

@app.post("/FloraFaunaGo_API/identification/classify")
async def classify_image(request: ImageBase64Request):
    try:
        # Supprimer le préfixe si présent
        base64_str = request.base64_image
        if "," in base64_str:
            base64_str = base64_str.split(",")[1]

        image_bytes = base64.b64decode(base64_str)
        image = Image.open(io.BytesIO(image_bytes)).convert("RGB")
    except Exception as e:
        raise HTTPException(status_code=400, detail=f"Invalid image format: {str(e)}")

    # Préparer les données pour le modèle
    inputs = processor(text=prompts, images=image, return_tensors="pt", padding=True)

    with torch.no_grad():
        outputs = model(**inputs)
        logits_per_image = outputs.logits_per_image
        probs = logits_per_image.softmax(dim=1)

    predicted_idx = probs.argmax().item()
    predicted_label = labels[predicted_idx]

    return {"classification": predicted_label}
