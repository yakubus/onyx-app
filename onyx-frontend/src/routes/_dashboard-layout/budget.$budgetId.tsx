import { useState } from "react";
import { createFileRoute } from "@tanstack/react-router";
import { useSuspenseQuery } from "@tanstack/react-query";

import BudgetAssigmentCard from "@/components/dashboard/budget/BudgetAssigmentCard";
import CategoriesCard from "@/components/dashboard/budget/CategoriesCard";
import SubcategoriesCard from "@/components/dashboard/budget/SubcategoriesCard";
import BudgetLoadingSkeleton from "@/components/dashboard/budget/BudgetLoadingSkeleton";

import { getCategoriesQueryOptions } from "@/lib/api/category";
import RouteLoadingError from "@/components/RouteLoadingError";

export const Route = createFileRoute("/_dashboard-layout/budget/$budgetId")({
  loader: ({ context: { queryClient }, params: { budgetId } }) =>
    queryClient.ensureQueryData(getCategoriesQueryOptions(budgetId)),
  component: SingleBudget,
  pendingComponent: () => <BudgetLoadingSkeleton />,
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
    <div className="grid h-full grid-cols-1 gap-x-8 gap-y-4 rounded-md lg:grid-cols-5">
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
