import { useMutation, useQueryClient } from "@tanstack/react-query";
import { deleteAccount, getAccountsQueryOptions } from "@/lib/api/account";

interface DeleteAccountMutationProps {
  budgetId: string;
  onMutationError: () => void;
}

export const useDeleteAccountMutation = ({
  budgetId,
  onMutationError,
}: DeleteAccountMutationProps) => {
  const queryClient = useQueryClient();
  const queryKey = getAccountsQueryOptions(budgetId).queryKey;

  return useMutation({
    mutationKey: ["deleteAccount"],
    mutationFn: deleteAccount,
    onMutate: (deletedAccount) => {
      queryClient.cancelQueries({
        queryKey,
      });
      const previousAccounts = queryClient.getQueryData(queryKey);

      queryClient.setQueryData(queryKey, (old) => {
        if (!old || !Array.isArray(old)) return old;

        return old.map((account) => {
          if (account.id === deletedAccount.accountId) {
            return {
              ...account,
              optimistic: true,
            };
          }
          return account;
        });
      });

      return previousAccounts;
    },
    onSettled: () => {
      queryClient.invalidateQueries({
        queryKey,
      });
    },
    onError: (_err, _newTodo, context) => {
      queryClient.setQueryData(queryKey, context);
      onMutationError();
    },
  });
};
