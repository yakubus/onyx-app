import { useMutation, useQueryClient } from "@tanstack/react-query";
import {
  deleteMultipleTransactions,
  getTransactionsQueryKey,
  getTransactionsQueryOptions,
} from "@/lib/api/transaction";
import { getAccountsQueryOptions } from "@/lib/api/account";
import { getCategoriesQueryOptions } from "@/lib/api/category";
import { Transaction } from "@/lib/validation/transaction";

interface Props {
  accountId: string;
  budgetId: string;
  onMutationError: () => void;
}

export const useDeleteTransactionsMutation = ({
  accountId,
  budgetId,
  onMutationError,
}: Props) => {
  const queryClient = useQueryClient();
  const transactionsQueryKey = getTransactionsQueryKey(accountId);

  return useMutation({
    mutationKey: ["deleteTransactions"],
    mutationFn: deleteMultipleTransactions,
    onMutate: (args) => {
      queryClient.cancelQueries({ queryKey: transactionsQueryKey });

      const previousTransactions =
        queryClient.getQueryData<Transaction[]>(transactionsQueryKey);

      queryClient.setQueryData<Transaction[]>(transactionsQueryKey, (old) => {
        if (!old || !Array.isArray(old)) return old;

        const idsToDelete = new Set(args.rows.map((r) => r.original.id));
        return old.filter((transaction) => !idsToDelete.has(transaction.id));
      });

      return previousTransactions;
    },
    onError: (err, _args, previousTransactions) => {
      console.log(err);
      if (previousTransactions) {
        queryClient.setQueryData(transactionsQueryKey, previousTransactions);
      }
      onMutationError();
    },
    onSettled: () => {
      Promise.all([
        queryClient.invalidateQueries(
          getTransactionsQueryOptions(budgetId, accountId, {
            accountId,
          }),
        ),
        queryClient.invalidateQueries(getAccountsQueryOptions(budgetId)),
        queryClient.invalidateQueries(getCategoriesQueryOptions(budgetId)),
      ]);
    },
  });
};
