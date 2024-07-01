import { createFileRoute } from "@tanstack/react-router";

import { getAccountsQueryOptions } from "@/lib/api/account";
import { SingleBudgetPageParamsSchema } from "@/lib/validation/searchParams";
import { getTransactionsQueryOptions } from "@/lib/api/transaction";

export const Route = createFileRoute(
  "/_dashboard-layout/budget/$budgetId/accounts",
)({
  loaderDeps: ({ search: { selectedAcc } }) => ({
    selectedAcc,
  }),
  loader: async ({
    context: { queryClient },
    params: { budgetId },
    deps: { selectedAcc },
  }) => {
    const accounts = await queryClient.ensureQueryData(
      getAccountsQueryOptions(budgetId),
    );

    if (selectedAcc) {
      queryClient.ensureQueryData(
        getTransactionsQueryOptions(budgetId, selectedAcc, {
          accountId: selectedAcc,
        }),
      );
    } else if (accounts.length > 0 && !selectedAcc) {
      const accId = accounts[0].id;

      queryClient.ensureQueryData(
        getTransactionsQueryOptions(budgetId, accId, {
          accountId: accId,
        }),
      );
    }
  },
  pendingComponent: () => <div>loading...</div>,
  validateSearch: SingleBudgetPageParamsSchema,
});
