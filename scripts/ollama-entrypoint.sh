#!/bin/bash

# Start Ollama service in the background
/bin/ollama serve &

# Wait for Ollama to be ready
echo "Waiting for Ollama service to start..."
until ollama list >/dev/null 2>&1; do
    sleep 1
done

echo "Ollama service is ready!"

# Pull the model if OLLAMA_MODEL is set and model doesn't exist
if [ -n "$OLLAMA_MODEL" ]; then
    echo "Checking if model '$OLLAMA_MODEL' exists..."
    if ! ollama list | grep -q "$OLLAMA_MODEL"; then
        echo "Model '$OLLAMA_MODEL' not found. Pulling..."
        ollama pull "$OLLAMA_MODEL"
        echo "Model '$OLLAMA_MODEL' pulled successfully!"
    else
        echo "Model '$OLLAMA_MODEL' already exists. Skipping pull."
    fi
else
    echo "OLLAMA_MODEL not set. Skipping model pull."
fi

# Keep the container running
wait
