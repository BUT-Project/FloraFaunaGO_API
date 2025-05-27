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

@app.post("/FloraFaunaGo_API/identification/animal")
async def speciesnet_identification(request: ImageBase64Request):
    """
    This endpoint expects a base64-encoded image and uses the cameratrapai (SpeciesNet) model.
    The subprocess will be run with the Python interpreter from the specified virtual environment.
    """
    try:
        base64_str = request.base64_image
        if "," in base64_str:
            base64_str = base64_str.split(",")[1]
        image_bytes = base64.b64decode(base64_str)
        image = Image.open(io.BytesIO(image_bytes)).convert("RGB")
    except Exception as e:
        raise HTTPException(status_code=400, detail=f"Invalid image format: {str(e)}")
    
    with tempfile.TemporaryDirectory() as tmpdir:
        image_path = os.path.join(tmpdir, "input.jpg")
        output_path = os.path.join(tmpdir, "output.json")
        image.save(image_path)

        # Path to the Python executable in your env
        env_python = r"env\Scripts\python.exe"
        #env_python = "python"
        # If you want to use the current running python instead (if you know you're in the right env):
        # env_python = sys.executable

        try:
            cmd = [
                env_python, "-m", "speciesnet.scripts.run_model",
                "--folders", tmpdir,
                "--predictions_json", output_path
            ]
            subprocess.run(cmd, check=True, capture_output=True)
        except subprocess.CalledProcessError as e:
            raise HTTPException(status_code=500, detail=f"Error running cameratrapai: {e.stderr.decode()}")

        try:
            with open(output_path, "r") as f:
                predictions = json.load(f)
            return predictions
        except Exception as e:
            raise HTTPException(status_code=500, detail=f"Error reading cameratrapai output: {str(e)}")