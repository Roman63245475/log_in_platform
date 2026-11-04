import { serve } from "bun";
import index from "./index.html";

const server = serve({
  port: 8088,
  routes: {
    // Proxy browser API calls through the UI origin so deployments do not
    // depend on cross-origin access or expose the API's internal Docker host.
    "/api/*": async (request) => {
      const target = new URL(request.url);
      target.protocol = "http:";
      target.hostname = "api";
      target.port = "5056";
      target.pathname = target.pathname.slice("/api".length) || "/";

      return fetch(new Request(target, request));
    },
    // Serve index.html for all unmatched routes.
    "/*": index,
  },

  development: process.env.NODE_ENV !== "production" && {
    // Enable browser hot reloading in development
    hmr: true,

    // Echo console logs from the browser to the server
    console: true,
  },
});

console.log(`🚀 Server running at ${server.url}`);
