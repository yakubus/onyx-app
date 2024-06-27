import { getAccountsQueryOptions } from "@/lib/api/account";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute(
  "/_dashboard-layout/budget/$budgetId/accounts",
)({
  loader: ({ context: { queryClient }, params: { budgetId } }) =>
    queryClient.ensureQueryData(getAccountsQueryOptions(budgetId)),
});
