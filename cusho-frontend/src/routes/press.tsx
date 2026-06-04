import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/press")({
  component: Press,
});

function Press() {
  return <p>hi helloo</p>;
}
