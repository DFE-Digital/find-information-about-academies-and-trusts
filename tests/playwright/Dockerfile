FROM mcr.microsoft.com/playwright:v1.34.0-jammy

COPY . .

RUN npm install

ENTRYPOINT ["npx", "playwright", "test", "--project=zap-tests"]
