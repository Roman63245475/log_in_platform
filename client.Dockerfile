FROM oven/bun:latest

WORKDIR /app

COPY client/package.json client/bun.lock* ./

RUN bun install

COPY client/src ./src
COPY client/tsconfig.json ./

EXPOSE 3000

CMD ["bun", "--hot", "src/index.ts"]
