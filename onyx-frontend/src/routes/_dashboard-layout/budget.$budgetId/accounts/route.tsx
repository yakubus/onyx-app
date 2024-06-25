import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute(
  "/_dashboard-layout/budget/$budgetId/accounts",
)({
  component: () => (
    <div>Hello /_dashboard-layout/budget/$budgetId/accounts!</div>
  ),
});
