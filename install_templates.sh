#!/bin/bash
TEMPLATE_DIR="./Templates"

if ! command -v dotnet &> /dev/null; then
    echo "Error: .NET SDK is not installed or not in PATH."
    exit 1
fi

if [ ! -d "$TEMPLATE_DIR" ]; then
    echo "Error: Directory '$TEMPLATE_DIR' does not exist."
    exit 1
fi

echo "Reinstalling Templates from: $TEMPLATE_DIR"

dotnet new uninstall "$TEMPLATE_DIR" > /dev/null 2>&1

# 2. Install the directory
dotnet new install "$TEMPLATE_DIR"

echo "Installation Complete. Checking list..."

# 3. Verify they show up (grep for the folder name to filter output)
dotnet new list | grep "$(basename "$TEMPLATE_DIR")"

echo "Done."