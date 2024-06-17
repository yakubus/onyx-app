import { useEffect, useState } from "react";
import { createFileRoute } from "@tanstack/react-router";
import { useSuspenseQuery } from "@tanstack/react-query";

import { Minus, Plus } from "lucide-react";
import RouteLoadingError from "@/components/RouteLoadingError";
import { Button } from "@/components/ui/button";
import CreateBudgetForm from "@/components/dashboard/budget/CreateBudgetForm";
import BudgetTableRow from "@/components/dashboard/budget/BudgetTableRow";
import BudgetsLoadingSkeleton from "@/components/dashboard/budget/BudgetsLoadingSkeleton";

import { getBudgetsQueryOptions } from "@/lib/api/budget";
import { cn } from "@/lib/utils";

export const Route = createFileRoute("/_dashboard-layout/budget/")({
  component: Budget,
  loader: ({ context: { queryClient } }) =>
    queryClient.ensureQueryData(getBudgetsQueryOptions),
  pendingComponent: () => <BudgetsLoadingSkeleton />,
  errorComponent: ({ reset }) => <RouteLoadingError reset={reset} />,
});

function Budget() {
  const {
    auth: { user },
  } = Route.useRouteContext();
  const [isCreating, setIsCreating] = useState(false);

  const budgetsQuery = useSuspenseQuery(getBudgetsQueryOptions);
  const { data: budgets } = budgetsQuery;

  const noBudgets = budgets.length === 0 || !budgets;

  useEffect(() => {
    if (noBudgets) {
      setIsCreating(true);
    }
  }, [budgets, noBudgets]);

  const handleCreateBudgetButtonClick = () => {
    if (noBudgets) return;
    setIsCreating(!isCreating);
  };

  return (
    <div className="h-full overflow-auto py-8 scrollbar-none md:pl-14 md:pr-10 md:pt-14">
      <header className="space-y-1 lg:space-y-2">
        <h1 className="text-3xl font-bold md:text-4xl lg:text-5xl">
          All your budgets in one place.
        </h1>
        <p className="text-sm text-muted-foreground">
          Select and manage your existing budgets as you wish or create new
          ones!
        </p>
      </header>
      <section className="space-y-4 py-20">
        <ul className="overflow-hidden rounded-lg border">
          <li className="grid w-full grid-cols-9 gap-x-4 bg-accent p-4 text-lg font-semibold tracking-wide">
            <p className="col-span-3">Name</p>
            <p className="col-span-2">Currency</p>
            <p className="col-span-4">Users</p>
          </li>
          {budgets.map((budget) => (
            <BudgetTableRow budget={budget} key={budget.id} />
          ))}

          <li
            className={cn(
              "grid grid-rows-[0fr] overflow-hidden transition-all duration-300 ease-in-out",
              isCreating && user && "grid-rows-[1fr] border-t",
            )}
          >
            <div className="overflow-hidden">
              <CreateBudgetForm setIsCreating={setIsCreating} user={user!} />
            </div>
          </li>
        </ul>
        <div className="flex justify-center">
          <Button
            variant="outline"
            className={cn("rounded-full", isCreating && "bg-secondary")}
            size="icon"
            onClick={handleCreateBudgetButtonClick}
            disabled={noBudgets}
          >
            {isCreating ? <Minus /> : <Plus />}
          </Button>
        </div>
      </section>
    </div>
  );
}
