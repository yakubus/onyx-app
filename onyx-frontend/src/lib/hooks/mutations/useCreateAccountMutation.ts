import { createAccount, getAccountsQueryOptions } from "@/lib/api/account";
import {
  DEFAULT_MONTH_STRING,
  DEFAULT_YEAR_STRING,
} from "@/lib/constants/date";
import { SingleBudgetPageSearchParams } from "@/lib/validation/searchParams";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useNavigate } from "@tanstack/react-router";

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
  const navigate = useNavigate();

  return useMutation({
    mutationKey: ["createAccount"],
    mutationFn: createAccount,
    onError: (err) => {
      console.error("Mutation error:", err);
      onMutationError();
    },
    onSuccess: async ({ accountId }) => {
      const queryKey = getAccountsQueryOptions(budgetId).queryKey;
      await queryClient.fetchQuery({ queryKey });
      onMutationSuccess();

      navigate({
        to: `/budget/${budgetId}/accounts/${accountId}`,
        params: { accountId, budgetId },
        search: (prev: SingleBudgetPageSearchParams) => ({
          ...prev,
          accMonth: DEFAULT_MONTH_STRING,
          accYear: DEFAULT_YEAR_STRING,
        }),
        mask: {
          to: `/budget/${budgetId}/accounts/${accountId}`,
        },
      });
    },
  });
};
