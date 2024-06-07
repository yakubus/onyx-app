import { useEffect, useState } from "react";
import { createFileRoute } from "@tanstack/react-router";
import { useMutationState, useSuspenseQuery } from "@tanstack/react-query";

import { Plus } from "lucide-react";
import RouteLoadingError from "@/components/RouteLoadingError";
import { Button } from "@/components/ui/button";
import CreateBudgetForm from "@/components/dashboard/budget/CreateBudgetForm";
import BudgetTableRow from "@/components/dashboard/budget/BudgetTableRow";
import BudgetsLoadingSkeleton from "@/components/dashboard/budget/BudgetsLoadingSkeleton";

import { getBudgetsQueryOptions } from "@/lib/api/budget";
import { useClickOutside } from "@/lib/hooks/useClickOutside";
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

  const optimisticalyAddedBudget = useMutationState({
    filters: { mutationKey: ["createBudget"], status: "pending" },
    select: (mutation) => mutation.state.variables,
  });

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

  const createBudgetFormRef = useClickOutside<HTMLLIElement>(() => {
    if (noBudgets) return;
    setIsCreating(false);
  });

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
          <li className="grid w-full grid-cols-4 bg-primary/20 p-4 text-lg font-semibold tracking-wide">
            <p>Budget name</p>
            <p>Budget Currency</p>
            <p>Users</p>
          </li>
          {budgets.map((budget) => (
            <BudgetTableRow budget={budget} key={budget.id} />
          ))}
          {optimisticalyAddedBudget.length > 0 && (
            <li className="border-t opacity-50 first-of-type:border-none">
              <div className="grid w-full grid-cols-4 px-4 py-8">
                <p>{optimisticalyAddedBudget[0] as string}</p>
                <p>{user?.currency}</p>
                <p className="col-span-2 flex flex-col">{user?.id}</p>
              </div>
            </li>
          )}
          {isCreating && (
            <li className="border-t px-4 py-8" ref={createBudgetFormRef}>
              <CreateBudgetForm setIsCreating={setIsCreating} />
            </li>
          )}
        </ul>
        <div className="flex justify-center">
          <Button
            variant="outline"
            className={cn("rounded-full", isCreating && "bg-secondary")}
            size="icon"
            onClick={handleCreateBudgetButtonClick}
            disabled={noBudgets}
          >
            <Plus />
          </Button>
        </div>
      </section>
    </div>
  );
}
