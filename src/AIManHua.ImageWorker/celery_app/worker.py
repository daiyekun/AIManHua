import os
from celery import Celery

broker_url = os.getenv("CELERY_BROKER_URL", "amqp://guest:guest@localhost:5672")
result_backend = os.getenv("CELERY_RESULT_BACKEND", "redis://localhost:6379/1")

app = Celery(
    "aimanhua_image_worker",
    broker=broker_url,
    backend=result_backend,
    include=["celery_app.tasks"],
)

app.conf.update(
    task_serializer="json",
    result_serializer="json",
    accept_content=["json"],
    timezone="Asia/Shanghai",
    enable_utc=True,
    task_track_started=True,
    task_acks_late=True,
    worker_prefetch_multiplier=1,
)
