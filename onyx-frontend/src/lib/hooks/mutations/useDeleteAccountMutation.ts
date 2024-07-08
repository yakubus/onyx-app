import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useNavigate } from "@tanstack/react-router";
import { deleteAccount, getAccountsQueryOptions } from "@/lib/api/account";
import { SingleBudgetPageSearchParams } from "@/lib/validation/searchParams";
import {
  DEFAULT_MONTH_STRING,
  DEFAULT_YEAR_STRING,
} from "@/lib/constants/date";
import { Account } from "@/lib/validation/account";

interface DeleteAccountMutationProps {
  budgetId: string;
  onMutationSuccess: () => void;
}

export const useDeleteAccountMutation = ({
  budgetId,
  onMutationSuccess,
}: DeleteAccountMutationProps) => {
  const queryClient = useQueryClient();
  const navigate = useNavigate();
  const queryKey = getAccountsQueryOptions(budgetId).queryKey;

  const handleNavigation = (accounts: Account[] | undefined) => {
    const defaultSearchParams = {
      accMonth: DEFAULT_MONTH_STRING,
      accYear: DEFAULT_YEAR_STRING,
    };

    if (!accounts || accounts.length === 0) {
      const url = `/budget/${budgetId}`;
      navigate({
        to: url,
        search: (prev: SingleBudgetPageSearchParams) => ({
          ...prev,
          ...defaultSearchParams,
        }),
        mask: {
          to: url,
        },
      });
    } else {
      const lastAccountId = accounts[accounts.length - 1].id;
      const url = `/budget/${budgetId}/accounts/${lastAccountId}`;
      navigate({
        to: url,
        search: (prev: SingleBudgetPageSearchParams) => ({
          ...prev,
          ...defaultSearchParams,
        }),
        mask: {
          to: url,
        },
      });
    }
  };

  return useMutation({
    mutationKey: ["deleteAccount"],
    mutationFn: deleteAccount,
    onSuccess: async () => {
      await queryClient.fetchQuery({ queryKey });
      const accounts = queryClient.getQueryData(queryKey);
      onMutationSuccess();
      handleNavigation(accounts);
    },
    onError: (error) => {
      console.error("Failed to delete account:", error);
    },
  });
};
