import { useQueryClient } from "@tanstack/react-query";
import { getCategoriesQueryOptions } from "@/lib/api/category";
import { getToAssignQueryKey } from "@/lib/api/budget";
import { getAccountsQueryOptions } from "@/lib/api/account";

export const useSingleBudgetLoadingState = (budgetId?: string) => {
  const queryClient = useQueryClient();

  if (!budgetId) {
    return { isLoading: false, isError: true, errorMessage: "No budget ID" };
  }

  const queryStates = [
    queryClient.getQueryState(getCategoriesQueryOptions(budgetId).queryKey),
    queryClient.getQueryState(getToAssignQueryKey(budgetId)),
    queryClient.getQueryState(getAccountsQueryOptions(budgetId).queryKey),
  ];

  const isLoading = queryStates.some((state) => state?.status === "pending");
  const isError = queryStates.some((state) => state?.status === "error");
  const errorMessage = queryStates.find((state) => state?.status === "error")
    ?.error?.message;

  return { isLoading, isError, errorMessage };
};
