import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute(
  "/_dashboard-layout/budget/$budgetId/accounts/$accountId",
)({
  component: () => (
    <div>
      Hello /_dashboard-layout/budget/$budgetId/accounts/accounts/$accountId!
    </div>
  ),
});
