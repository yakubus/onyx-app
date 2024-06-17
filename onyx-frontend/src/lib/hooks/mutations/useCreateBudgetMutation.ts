import { useMutation, useQueryClient } from "@tanstack/react-query";

import { createBudget, getBudgetsQueryOptions } from "@/lib/api/budget";

interface CreateBudgetMutationProps {
  onMutationError: () => void;
  onMutationSuccess: () => void;
}

export const useCreateBudgetMutation = ({
  onMutationError,
  onMutationSuccess,
}: CreateBudgetMutationProps) => {
  const queryClient = useQueryClient();
  const queryKey = getBudgetsQueryOptions.queryKey;

  return useMutation({
    mutationKey: ["createBudget"],
    mutationFn: createBudget,
    onMutate: async (newBudget) => {
      const { budgetName, budgetCurrency, userId } = newBudget;

      await queryClient.cancelQueries({
        queryKey,
      });

      const previousBudgets = queryClient.getQueryData(queryKey);

      queryClient.setQueryData(queryKey, (old) => {
        if (!old) return old;

        return [
          ...old,
          {
            id: "12345",
            name: budgetName,
            currency: budgetCurrency,
            userIds: [userId],
            optimistic: true,
          },
        ];
      });

      return previousBudgets;
    },
    onSettled: () => {
      queryClient.invalidateQueries({
        queryKey,
      });
    },
    onError: (err, _newTodo, context) => {
      console.log(err);
      queryClient.setQueryData(queryKey, context);
      onMutationError();
    },
    onSuccess: () => {
      onMutationSuccess();
    },
  });
};
