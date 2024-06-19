import { useState } from "react";
import { useSuspenseQueries } from "@tanstack/react-query";
import { Link, createLazyFileRoute } from "@tanstack/react-router";

import { Undo2 } from "lucide-react";
import BudgetAssignmentCard from "@/components/dashboard/budget/BudgetAssigmentCard";
import CategoriesCard from "@/components/dashboard/budget/CategoriesCard";
import SubcategoriesCard from "@/components/dashboard/budget/SubcategoriesCard";
import { Button } from "@/components/ui/button";

import { getToAssignQueryOptions } from "@/lib/api/budget";
import { getCategoriesQueryOptions } from "@/lib/api/category";

export const Route = createLazyFileRoute("/_dashboard-layout/budget/$budgetId")(
  {
    component: SingleBudget,
  },
);

function SingleBudget() {
  const [activeCategoryId, setActiveCategoryId] = useState("");
  const { budgetId } = Route.useParams();
  const { month, year } = Route.useSearch();
  const [{ data: categories }, { data: toAssign }] = useSuspenseQueries({
    queries: [
      getCategoriesQueryOptions(budgetId),
      getToAssignQueryOptions({ budgetId, month, year }),
    ],
  });
  const activeCategoryData = categories.find(
    (category) => category.id === activeCategoryId,
  );

  return (
    <div className="relative grid h-full grid-cols-1 gap-x-8 gap-y-4 rounded-md lg:grid-cols-5">
      <Button
        className="-top-16 lg:absolute"
        asChild
        variant="outline"
        size="sm"
      >
        <Link to="/budget" className="inline-flex  items-center justify-center">
          <Undo2 className="size-4" />
          <span className="ml-2">Budgets</span>
        </Link>
      </Button>
      <div className="flex h-full flex-col space-y-4 overflow-hidden lg:col-span-2">
        <BudgetAssignmentCard toAssign={toAssign} />
        <CategoriesCard
          activeCategoryId={activeCategoryId}
          setActiveCategoryId={setActiveCategoryId}
          categories={categories}
        />
      </div>
      {activeCategoryData && (
        <SubcategoriesCard activeCategory={activeCategoryData} />
      )}
    </div>
  );
}
