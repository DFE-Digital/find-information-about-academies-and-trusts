FROM mcr.microsoft.com/playwright:v1.34.0-jammy

COPY . .

RUN npm install
# Also install Edge as it doesn't come by default
RUN npx playwright install msedge

ENTRYPOINT ["npx", "playwright", "test", "deployment-tests/*"]
