import { Outlet, createFileRoute, redirect } from "@tanstack/react-router";

export const Route = createFileRoute("/_dashboard-layout/_budget-only-layout")({
  beforeLoad: ({ search: { selectedBudget } }) => {
    if (!selectedBudget) {
      throw redirect({
        to: "/budget",
      });
    }
  },
  component: () => <Outlet />,
});
