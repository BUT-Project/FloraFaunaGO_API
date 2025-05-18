import sys
from PIL import Image
from transformers import CLIPProcessor, CLIPModel
import torch
import json

model = CLIPModel.from_pretrained("openai/clip-vit-base-patch32")
processor = CLIPProcessor.from_pretrained("openai/clip-vit-base-patch32")
model.eval()

prompts = ["a photo of an animal", "a photo of an insect", "a photo of a plant"]
labels = ["animal", "insect", "plant"]

def classify(image_path):
    image = Image.open(image_path).convert("RGB")
    inputs = processor(text=prompts, images=image, return_tensors="pt", padding=True)
    with torch.no_grad():
        outputs = model(**inputs)
        logits_per_image = outputs.logits_per_image
        probs = logits_per_image.softmax(dim=1)
    predicted_idx = probs.argmax().item()
    return labels[predicted_idx]

if __name__ == "__main__":
    img_path = sys.argv[1]
    label = classify(img_path)
    print(json.dumps({"classification": label}))
