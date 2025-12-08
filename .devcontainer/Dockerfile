# Use Node.js 20 as base image
FROM node:20-bullseye

# Set working directory
WORKDIR /app

# Install git and other essential tools
RUN apt-get update && apt-get install -y \
    git \
    curl \
    build-essential \
    && rm -rf /var/lib/apt/lists/*

# Create a non-root user for development
RUN useradd -m -s /bin/bash developer && \
    chown -R developer:developer /app

# Switch to the developer user
USER developer

# Set environment variables
ENV NODE_ENV=development
ENV PATH=/home/developer/.npm-global/bin:$PATH

# Default command
CMD ["/bin/bash"]
