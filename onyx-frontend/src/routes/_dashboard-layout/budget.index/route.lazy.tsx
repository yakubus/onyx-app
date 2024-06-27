import { useEffect, useState } from "react";
import { useSuspenseQuery } from "@tanstack/react-query";
import { createLazyFileRoute } from "@tanstack/react-router";

import { Minus, Plus } from "lucide-react";
import BudgetTableRow from "@/components/dashboard/budget/BudgetTableRow";
import CreateBudgetForm from "@/components/dashboard/budget/CreateBudgetForm";
import { Button } from "@/components/ui/button";
import { getBudgetsQueryOptions } from "@/lib/api/budget";
import { cn } from "@/lib/utils";

export const Route = createLazyFileRoute("/_dashboard-layout/budget/")({
  component: Budget,
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
    <div className="h-full overflow-auto p-8 scrollbar-none md:pl-14 md:pr-10 md:pt-14">
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
          <li className="grid w-full grid-cols-3 gap-x-4 bg-accent p-4 text-lg font-semibold tracking-wide md:grid-cols-9">
            <p className="md:col-span-3">Name</p>
            <p className="md:col-span-2">Currency</p>
            <p className="md:col-span-4">Users</p>
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
