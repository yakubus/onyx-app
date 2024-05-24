import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_dashboard-layout/budget")({
  component: Budget,
});

function Budget() {
  return <div>budget</div>;
}
