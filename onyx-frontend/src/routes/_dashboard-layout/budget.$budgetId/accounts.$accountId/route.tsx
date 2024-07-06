import { createFileRoute } from "@tanstack/react-router";
import { getAccountsQueryOptions } from "@/lib/api/account";
import { getTransactionsQueryOptions } from "@/lib/api/transaction";
import { SingleBudgetPageParamsSchema } from "@/lib/validation/searchParams";

export const Route = createFileRoute(
  "/_dashboard-layout/budget/$budgetId/accounts/$accountId",
)({
  loader: ({ context: { queryClient }, params: { budgetId, accountId } }) => {
    Promise.all([
      queryClient.ensureQueryData(
        getTransactionsQueryOptions(budgetId, accountId, {
          accountId,
        }),
      ),
      queryClient.ensureQueryData(getAccountsQueryOptions(budgetId)),
    ]);
  },
  pendingComponent: () => <div>loading..</div>,
  validateSearch: SingleBudgetPageParamsSchema,
});
