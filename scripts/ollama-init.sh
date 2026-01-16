#!/bin/bash

echo "=========================================="
echo "OLLAMA MODEL INITIALIZATION"
echo "=========================================="

# Start Ollama service in the background
echo "Starting Ollama service..."
/bin/ollama serve &
OLLAMA_PID=$!

# Wait for Ollama to be ready
echo "Waiting for Ollama service to start..."
until ollama list >/dev/null 2>&1; do
    sleep 1
done
echo "✓ Ollama service started"

# Pull the model if OLLAMA_MODEL is set
if [ -n "$OLLAMA_MODEL" ]; then
    echo "Checking if model '$OLLAMA_MODEL' exists..."
    if ! ollama list | grep -q "$OLLAMA_MODEL"; then
        echo "=========================================="
        echo "Model '$OLLAMA_MODEL' not found."
        echo "Pulling model (this may take several minutes)..."
        echo "=========================================="
        ollama pull "$OLLAMA_MODEL"
        if [ $? -eq 0 ]; then
            echo "=========================================="
            echo "✓ Model '$OLLAMA_MODEL' pulled successfully!"
            echo "=========================================="
        else
            echo "=========================================="
            echo "✗ Failed to pull model '$OLLAMA_MODEL'"
            echo "=========================================="
            kill $OLLAMA_PID
            exit 1
        fi
    else
        echo "✓ Model '$OLLAMA_MODEL' already exists"
    fi
else
    echo "Warning: OLLAMA_MODEL not set. No model will be pulled."
fi

# Cleanup
echo "Stopping initialization service..."
kill $OLLAMA_PID
wait $OLLAMA_PID 2>/dev/null

echo "=========================================="
echo "✓ OLLAMA INITIALIZATION COMPLETE"
echo "=========================================="
