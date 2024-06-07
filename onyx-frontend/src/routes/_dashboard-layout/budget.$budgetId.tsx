import { useState } from "react";
import { Link, createFileRoute } from "@tanstack/react-router";
import { useSuspenseQuery } from "@tanstack/react-query";

import { Undo2 } from "lucide-react";
import BudgetAssigmentCard from "@/components/dashboard/budget/BudgetAssigmentCard";
import CategoriesCard from "@/components/dashboard/budget/CategoriesCard";
import SubcategoriesCard from "@/components/dashboard/budget/SubcategoriesCard";
import SingleBudgetLoadingSkeleton from "@/components/dashboard/budget/SingleBudgetLoadingSkeleton";
import RouteLoadingError from "@/components/RouteLoadingError";
import { Button } from "@/components/ui/button";

import { getCategoriesQueryOptions } from "@/lib/api/category";

export const Route = createFileRoute("/_dashboard-layout/budget/$budgetId")({
  loader: ({ context: { queryClient }, params: { budgetId } }) =>
    queryClient.ensureQueryData(getCategoriesQueryOptions(budgetId)),
  component: SingleBudget,
  pendingComponent: () => <SingleBudgetLoadingSkeleton />,
  errorComponent: ({ reset }) => <RouteLoadingError reset={reset} />,
});

function SingleBudget() {
  const [activeCategoryId, setActiveCategoryId] = useState("");
  const { budgetId } = Route.useParams();
  const { data } = useSuspenseQuery(getCategoriesQueryOptions(budgetId));
  const subcategories = data.find(
    (category) => category.id === activeCategoryId,
  )?.subcategories;

  return (
    <div className="relative grid h-full grid-cols-1 gap-x-8 gap-y-4 rounded-md lg:grid-cols-5">
      <Button
        className="-top-16 lg:absolute"
        asChild
        variant="outline"
        size="sm"
      >
        <Link to="/budget" className="inline-flex items-center justify-center">
          <Undo2 className="size-4" />
          <span className="ml-2">Budgets</span>
        </Link>
      </Button>
      <div className="flex h-full flex-col space-y-4 overflow-hidden lg:col-span-2">
        <BudgetAssigmentCard />
        <CategoriesCard
          activeCategoryId={activeCategoryId}
          setActiveCategoryId={setActiveCategoryId}
          categories={data}
        />
      </div>
      {activeCategoryId && <SubcategoriesCard subcategories={subcategories} />}
    </div>
  );
}
