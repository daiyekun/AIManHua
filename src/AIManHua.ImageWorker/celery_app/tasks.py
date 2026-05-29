from .worker import app
from processors import bubble, layout, stitch, effects


@app.task(bind=True, name="generate_comic_page")
def generate_comic_page(self, task_id: str, panels: list, config: dict):
    """Generate a comic page from a list of panels with layout and dialogue bubbles."""
    # Business logic will be implemented in the next phase
    return {"task_id": task_id, "status": "completed"}


@app.task(bind=True, name="generate_single_image")
def generate_single_image(self, task_id: str, prompt: str, params: dict):
    """Generate a single AI image from a text prompt."""
    # Business logic will be implemented in the next phase
    return {"task_id": task_id, "status": "completed"}
