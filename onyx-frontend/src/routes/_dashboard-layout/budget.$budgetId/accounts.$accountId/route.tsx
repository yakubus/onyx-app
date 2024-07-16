import { createFileRoute } from "@tanstack/react-router";

import RouteLoadingError from "@/components/RouteLoadingError";
import AccountsLoadingSkeleton from "@/components/dashboard/accounts/AccountsLoadingSkeleton";

import { getAccountsQueryOptions } from "@/lib/api/account";
import { getTransactionsQueryOptions } from "@/lib/api/transaction";
import { SingleBudgetPageParamsSchema } from "@/lib/validation/searchParams";
import { getCategoriesQueryOptions } from "@/lib/api/category";

export const Route = createFileRoute(
  "/_dashboard-layout/budget/$budgetId/accounts/$accountId",
)({
  loader: async ({
    context: { queryClient },
    params: { budgetId, accountId },
  }) => {
    Promise.all([
      queryClient.ensureQueryData(
        getTransactionsQueryOptions(budgetId, accountId, {
          accountId,
        }),
      ),
      queryClient.ensureQueryData(getAccountsQueryOptions(budgetId)),
      queryClient.ensureQueryData(getCategoriesQueryOptions(budgetId)),
    ]);
  },
  pendingComponent: () => <AccountsLoadingSkeleton />,
  errorComponent: ({ reset }) => <RouteLoadingError reset={reset} />,
  validateSearch: SingleBudgetPageParamsSchema,
});
