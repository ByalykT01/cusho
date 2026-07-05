import { betterAuth } from "better-auth";
import { genericOAuth, keycloak } from "better-auth/plugins";
import { tanstackStartCookies } from "better-auth/tanstack-start";


export const auth = betterAuth({
  baseURL: process.env.BETTER_AUTH_URL,
  secret: process.env.BETTER_AUTH_SECRET,
  plugins: [
    genericOAuth({
      config: [
        keycloak({
          clientId: process.env.KEYCLOAK_CLIENT_ID!,
          clientSecret: process.env.KEYCLOAK_CLIENT_SECRET!,
          issuer: process.env.KEYCLOAK_ISSUER!,
          pkce: true,
        })
      ]
    }),
    tanstackStartCookies(),
  ]
})

