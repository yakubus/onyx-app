import { Outlet, createFileRoute, redirect } from "@tanstack/react-router";

import { SingleBudgetPageParamsSchema } from "@/lib/validation/searchParams";

export const Route = createFileRoute("/_dashboard-layout/_budget-only-layout")({
  beforeLoad: ({ search: { selectedBudget, month, year } }) => {
    if (!selectedBudget || !month || !year) {
      throw redirect({
        to: "/budget",
      });
    }
  },
  validateSearch: SingleBudgetPageParamsSchema,
  component: () => <Outlet />,
});
