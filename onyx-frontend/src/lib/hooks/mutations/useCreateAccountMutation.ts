import { createAccount, getAccountsQueryOptions } from "@/lib/api/account";
import { capitalize } from "@/lib/utils";
import { useMutation, useQueryClient } from "@tanstack/react-query";

interface CreateAccountMutationProps {
  budgetId: string;
  onMutationSuccess: () => void;
  onMutationError: () => void;
}

export const useCreateAccountMutation = ({
  budgetId,
  onMutationSuccess,
  onMutationError,
}: CreateAccountMutationProps) => {
  const queryClient = useQueryClient();
  const queryKey = getAccountsQueryOptions(budgetId).queryKey;

  return useMutation({
    mutationKey: ["createAccount"],
    mutationFn: createAccount,
    onMutate: async (newAccount) => {
      await queryClient.cancelQueries({
        queryKey,
      });

      const previousCategories = queryClient.getQueryData(queryKey);

      queryClient.setQueryData(queryKey, (old) => {
        if (!old) return old;
        const { name, balance, accountType } = newAccount.payload;
        return [
          ...old,
          {
            id: "12345",
            name: capitalize(name),
            balance,
            type: accountType,
            optimistic: true,
          },
        ];
      });

      return previousCategories;
    },
    onSettled: () => {
      queryClient.invalidateQueries({
        queryKey,
      });
    },
    onError: (err, _newTodo, context) => {
      console.error("Mutation error:", err);
      queryClient.setQueryData(queryKey, context);
      onMutationError();
    },
    onSuccess: () => {
      onMutationSuccess();
    },
  });
};
