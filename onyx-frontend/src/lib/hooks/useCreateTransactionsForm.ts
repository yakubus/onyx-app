import { useEffect, useMemo } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";

import {
  CreateTransactionSchema,
  TCreateTransactionSchema,
} from "@/lib/validation/transaction";

interface Props {
  accMonth: string;
  accYear: string;
  accountCurrency: string;
}

export const useCreateTransactionsForm = ({
  accMonth,
  accYear,
  accountCurrency,
}: Props) => {
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
      currency: accountCurrency,
      amount: "0.00",
      counterpartyName: "",
      subcategoryId: "",
      transactedAt: dafaultTransactedAt,
      categoryId: "",
      transactionSign: "-",
    },
    resolver: zodResolver(CreateTransactionSchema),
  });

  const { control, setFocus, handleSubmit, watch, reset, setValue } = form;
  const selectedCategoryId = watch("categoryId");
  const selectedCurrency = watch("currency");
  const transactionSign = watch("transactionSign");

  useEffect(() => {
    reset((defaultValues) => ({
      ...defaultValues,
      transactedAt: dafaultTransactedAt,
    }));
  }, [dafaultTransactedAt, reset]);

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
        ? Number(transactionSign + removeSpacesFromAmount(amount))
        : Number(removeSpacesFromAmount(amount));

    const payload: CreateTransactionPayload = {
      accountId: account.id,
      amount: {
        amount: formattedAmount,
        currency,
      },
      counterpartyName,
      subcategoryId: subcategoryId === "" ? null : subcategoryId,
      transactedAt,
    };

    mutate({ budgetId, payload });
  };

  return form;
};
