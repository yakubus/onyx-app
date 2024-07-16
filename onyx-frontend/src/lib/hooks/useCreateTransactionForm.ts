import { useCallback, useEffect, useMemo } from "react";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useParams, useSearch } from "@tanstack/react-router";
import { SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";

import {
  CreateTransactionSchema,
  TCreateTransactionSchema,
} from "@/lib/validation/transaction";
import {
  createTransaction,
  CreateTransactionPayload,
  getTransactionsQueryOptions,
} from "@/lib/api/transaction";
import { getAccountsQueryOptions } from "@/lib/api/account";
import { getCategoriesQueryOptions } from "@/lib/api/category";
import { formatToDotDecimal } from "@/lib/utils";
import { type Account } from "@/lib/validation/account";

interface Props {
  account: Account;
}

export const useCreateTransactionForm = ({ account }: Props) => {
  const { accMonth, accYear } = useSearch({
    from: "/_dashboard-layout/budget/$budgetId/accounts/$accountId",
  });
  const { budgetId, accountId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId/accounts/$accountId",
  });
  const queryClient = useQueryClient();

  const isCurrentMonthSelected =
    Number(accMonth) === new Date().getMonth() + 1 &&
    Number(accYear) === new Date().getFullYear();
  const dafaultTransactedAt = useMemo(
    () =>
      isCurrentMonthSelected
        ? new Date()
        : new Date(`${accYear}-${accMonth}-01`),
    [accMonth, accYear, isCurrentMonthSelected],
  );

  const form = useForm<TCreateTransactionSchema>({
    defaultValues: {
      currency: account.balance.currency,
      amount: "0.00",
      counterpartyName: "",
      subcategoryId: "",
      subcategoryName: "",
      transactedAt: dafaultTransactedAt,
      transactionSign: "-",
    },
    resolver: zodResolver(CreateTransactionSchema),
  });
  const {
    control,
    setFocus,
    handleSubmit,
    watch,
    reset,
    setValue,
    clearErrors,
  } = form;
  const selectedCurrency = watch("currency");
  const transactionSign = watch("transactionSign");
  const selectedSubcategoryName = watch("subcategoryName");

  useEffect(() => {
    reset((defaultValues) => ({
      ...defaultValues,
      currency: account.balance.currency,
      transactedAt: dafaultTransactedAt,
    }));
  }, [dafaultTransactedAt, reset, account.balance.currency]);

  const [
    transtactionsQueryOptions,
    accountsQueryOptions,
    categoriesQueryOptions,
  ] = [
    getTransactionsQueryOptions(budgetId, accountId, {
      accountId,
    }),
    getAccountsQueryOptions(budgetId),
    getCategoriesQueryOptions(budgetId),
  ];
  const transactionsQueryKey = transtactionsQueryOptions.queryKey;

  const { mutate, isPending } = useMutation({
    mutationFn: createTransaction,
    onMutate: (payload) => {
      queryClient.cancelQueries(transtactionsQueryOptions);

      const previousTransactions =
        queryClient.getQueryData(transactionsQueryKey);

      queryClient.setQueryData(transactionsQueryKey, (old) => {
        if (!old || !Array.isArray(old)) return old;

        const {
          payload: { amount, counterpartyName, transactedAt },
        } = payload;

        return [
          {
            transactedAt: transactedAt.toString(),
            id: "12345",
            amount: {
              amount: amount.amount,
              currency: amount.currency,
            },
            counterparty: {
              id: "123456",
              name: counterpartyName,
              type: "Payee" as const,
            },
            subcategory: {
              id: "1234556",
              name: selectedSubcategoryName || "N/A",
              assignments: null,
              description: null,
              target: null,
            },
            account,
          },
          ...old,
        ];
      });

      return previousTransactions;
    },
    onError: (err, _newTodo, previousTransactions) => {
      queryClient.setQueryData(transactionsQueryKey, previousTransactions);
      console.error(err);
    },
    onSettled: () => {
      Promise.all([
        queryClient.invalidateQueries(transtactionsQueryOptions),
        queryClient.invalidateQueries(accountsQueryOptions),
        queryClient.invalidateQueries(categoriesQueryOptions),
      ]);
    },
  });

  const onSubmit: SubmitHandler<TCreateTransactionSchema> = (data) => {
    const {
      amount,
      counterpartyName,
      subcategoryId,
      transactedAt,
      currency,
      transactionSign,
    } = data;
    const formattedAmount =
      transactionSign === "-"
        ? Number(transactionSign + formatToDotDecimal(amount))
        : Number(formatToDotDecimal(amount));

    const payload: CreateTransactionPayload = {
      accountId,
      amount: {
        amount: formattedAmount,
        currency,
      },
      counterpartyName,
      subcategoryId: subcategoryId === "" ? null : subcategoryId,
      transactedAt,
    };

    mutate({ budgetId, payload });
    reset();
  };

  const handlePlusMinusBtn = useCallback(
    (state: "+" | "-") => {
      setValue("transactionSign", state);
    },
    [setValue],
  );

  return {
    onSubmit,
    isPending,
    control,
    setFocus,
    handleSubmit,
    selectedCurrency,
    transactionSign,
    isCurrentMonthSelected,
    form,
    handlePlusMinusBtn,
    selectedSubcategoryName,
    setValue,
    clearErrors,
  };
};
