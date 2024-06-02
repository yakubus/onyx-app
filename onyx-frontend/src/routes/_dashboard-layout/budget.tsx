import { useState } from "react";
import { useSuspenseQuery } from "@tanstack/react-query";
import { createFileRoute } from "@tanstack/react-router";

import BudgetAssigmentCard from "@/components/dashboard/budget/BudgetAssigmentCard";
import CategoriesCard from "@/components/dashboard/budget/CategoriesCard";
import SubcategoriesCard from "@/components/dashboard/budget/SubcategoriesCard";
import RouteLoadingError from "@/components/RouteLoadingError";
import BudgetLoadingSkeleton from "@/components/dashboard/budget/BudgetLoadingSkeleton";

import { getCategoriesQueryOptions } from "@/lib/api/category";

export const Route = createFileRoute("/_dashboard-layout/budget")({
  component: Budget,
  loader: ({ context: { queryClient } }) =>
    queryClient.ensureQueryData(getCategoriesQueryOptions),
  pendingComponent: () => <BudgetLoadingSkeleton />,
  errorComponent: ({ reset }) => <RouteLoadingError reset={reset} />,
});

function Budget() {
  const [activeCategoryId, setActiveCategoryId] = useState("");

  const categoriesQuery = useSuspenseQuery(getCategoriesQueryOptions);
  const categories = categoriesQuery.data;
  const subcategories = categories.find(
    (category) => category.id === activeCategoryId,
  )?.subcategories;

  return (
    <div className="grid h-full grid-cols-1 gap-x-8 gap-y-4 rounded-md lg:grid-cols-5">
      <div className="flex h-full flex-col space-y-4 overflow-hidden lg:col-span-2">
        <BudgetAssigmentCard />
        <CategoriesCard
          activeCategoryId={activeCategoryId}
          setActiveCategoryId={setActiveCategoryId}
          categories={categories}
        />
      </div>
      {activeCategoryId && <SubcategoriesCard subcategories={subcategories} />}
    </div>
  );
}
