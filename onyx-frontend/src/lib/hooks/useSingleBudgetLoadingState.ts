import { useQueryClient } from "@tanstack/react-query";
import { getCategoriesQueryOptions } from "@/lib/api/category";
import { getToAssignQueryKey } from "@/lib/api/budget";

export const useSingleBudgetLoadingState = (budgetId: string) => {
  const queryClient = useQueryClient();
  const categoriesState = queryClient.getQueryState(
    getCategoriesQueryOptions(budgetId).queryKey,
  );
  const toAssignState = queryClient.getQueryState(
    getToAssignQueryKey(budgetId),
  );

  const isLoading =
    categoriesState?.status === "pending" ||
    toAssignState?.status === "pending";
  const isError =
    categoriesState?.status === "error" || toAssignState?.status === "error";
  const errorMessage =
    categoriesState?.error?.message || toAssignState?.error?.message;

  return { isLoading, isError, errorMessage };
};
