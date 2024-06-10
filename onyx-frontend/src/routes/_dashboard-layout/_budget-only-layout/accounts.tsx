import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute(
  "/_dashboard-layout/_budget-only-layout/accounts",
)({
  component: () => <div>Hello /_dashboard-layout/accounts!</div>,
});
