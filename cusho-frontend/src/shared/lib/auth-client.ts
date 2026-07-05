import { createAuthClient } from "better-auth/react";
import { genericOAuthClient } from "better-auth/client/plugins";


export const authClient = createAuthClient({
  baseURL: import.meta.env.VITE_APP_URL ?? "http://localhost:5173", // client-side, ok to be VITE_-prefixed
  plugins: [genericOAuthClient()],
});

