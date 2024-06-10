import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute(
  "/_dashboard-layout/_budget-only-layout/goals",
)({
  component: () => <div>Hello /_dashboard-layout/goals!</div>,
});
