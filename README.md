# Bella-Frisoer
Second Semester Exam, 2025

Docker startup commands - Kører migrations først og starter webui container efter hvis migrations er succesful.:
# Run everything; stop all containers if migrations exits; return migrations exit code
docker compose up --build --abort-on-container-exit --exit-code-from migrations
# If migrations succeeded, start webui detached
docker compose up -d webui