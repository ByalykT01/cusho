import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute('/resume')({
  component: Resume,
})

function Resume() {
  return <p>hi helloo</p>
}
