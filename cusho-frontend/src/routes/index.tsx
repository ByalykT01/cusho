import * as fs from "node:fs";
import { createFileRoute, Link, useRouter } from "@tanstack/react-router";
import { createServerFn } from "@tanstack/react-start";
import { authClient } from "@/shared/lib/auth-client";

const filePath = "count.txt";

async function readCount() {
  return parseInt(
    await fs.promises.readFile(filePath, "utf-8").catch(() => "0"),
  );
}

const getCount = createServerFn({
  method: "GET",
}).handler(() => {
  return readCount();
});

const updateCount = createServerFn({ method: "POST" })
  .inputValidator((d: number) => d)
  .handler(async ({ data }) => {
    const count = await readCount();
    await fs.promises.writeFile(filePath, `${count + data}`);
  });

export const Route = createFileRoute("/")({
  component: Home,
  loader: async () => await getCount(),
});

async function Home() {
  const router = useRouter();
  const state = Route.useLoaderData();

  const { data, error } = await authClient.getAccessToken({
    providerId: "keycloak" // Match the ID used in your genericOAuth config
  });

  console.log(data?.accessToken);

  const handleKeycloakSignIn = async () => {
    try {
      await authClient.signIn.oauth2({
        providerId: "keycloak",
        callbackURL: "/",
      });
    } catch (error) {
      console.error("Keycloak sign-in encountered an error:", error);
    }
  };

  return (
    <div style={{ display: "flex", flexDirection: "column", gap: "10px", padding: "20px" }}>
      <button
        type="button"
        onClick={() => {
          updateCount({ data: 1 }).then(() => {
            router.invalidate();
          });
        }}
      >
        Add 1 to {state}?
      </button>

      {/* Standard functional button */}
      <button type="button" onClick={handleKeycloakSignIn}>
        Sign in with Keycloak
      </button>

      <hr style={{ width: "100%", margin: "10px 0" }} />

      <Link to="/about">About</Link>
      <Link to="/resume">Resume</Link>
      <Link to="/projects">Projects</Link>
      <Link to="/press">Portfolio</Link>
    </div>
  );
}
