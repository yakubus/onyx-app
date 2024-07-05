import { createFileRoute } from "@tanstack/react-router";

import SingleBudgetLoadingSkeleton from "@/components/dashboard/budget/SingleBudgetLoadingSkeleton";
import RouteLoadingError from "@/components/RouteLoadingError";

import { SingleBudgetPageParamsSchema } from "@/lib/validation/searchParams";
import { getCategoriesQueryOptions } from "@/lib/api/category";
import { getToAssignQueryOptions } from "@/lib/api/budget";
import { getAccountsQueryOptions } from "@/lib/api/account";

export const Route = createFileRoute("/_dashboard-layout/budget/$budgetId/")({
  loaderDeps: ({ search: { month, year } }) => ({
    month,
    year,
  }),
  loader: ({
    context: { queryClient },
    params: { budgetId },
    deps: { month, year },
  }) => {
    Promise.all([
      queryClient.ensureQueryData(getCategoriesQueryOptions(budgetId)),
      queryClient.ensureQueryData(
        getToAssignQueryOptions({ budgetId, month, year }),
      ),
      queryClient.ensureQueryData(getAccountsQueryOptions(budgetId)),
    ]);
  },
  pendingComponent: () => <SingleBudgetLoadingSkeleton />,
  errorComponent: ({ reset }) => <RouteLoadingError reset={reset} />,
  validateSearch: SingleBudgetPageParamsSchema,
});
