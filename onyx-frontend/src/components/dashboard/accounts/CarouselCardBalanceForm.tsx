import { FC } from "react";
import { FieldValues, SubmitHandler } from "react-hook-form";

import AmountInput from "../AmountInput";
import { Form, FormField, FormItem } from "@/components/ui/form";
import { Money } from "@/lib/validation/base";
import { formatAmount, removeSpacesFromAmount } from "@/lib/utils";
import { useClickOutside } from "@/lib/hooks/useClickOutside";
import { editBalance, getAccountsQueryOptions } from "@/lib/api/account";
import { useParams } from "@tanstack/react-router";
import useAmountForm from "@/lib/hooks/useAmountForm";

interface CarouselCardBalanceFormProps {
  balance: Money;
  accountId: string;
}

const CarouselCardBalanceForm: FC<CarouselCardBalanceFormProps> = ({
  balance,
  accountId,
}) => {
  const { budgetId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId/accounts",
  });
  const { amount, currency } = balance;
  const defaultAmount = formatAmount(amount.toString());
  const { mutate, isDirty, handleSubmit, control, form } = useAmountForm({
    defaultAmount,
    mutationFn: editBalance,
    queryKey: getAccountsQueryOptions(budgetId).queryKey,
  });

  const onSubmit: SubmitHandler<FieldValues> = (data) => {
    const { amount } = data;
    const amountWithoutCommas = removeSpacesFromAmount(amount);

    if (Number(amountWithoutCommas) === Number(defaultAmount)) return;

    const newBalance: Money = {
      amount: Number(amountWithoutCommas),
      currency,
    };
    mutate({ budgetId, newBalance, accountId });
  };

  const formRef = useClickOutside<HTMLFormElement>(() => {
    if (isDirty) {
      handleSubmit(onSubmit)();
    }
  });

  return (
    <Form {...form}>
      <form onSubmit={handleSubmit(onSubmit)} ref={formRef}>
        <FormField
          control={control}
          name="amount"
          render={({ field }) => (
            <FormItem className="flex items-center space-x-1 space-y-0">
              <p className="text-lg md:text-xl">{currency}</p>
              <AmountInput
                field={field}
                className="border-0 bg-transparent pl-1 text-lg focus-visible:ring-0 focus-visible:ring-primary-foreground focus-visible:ring-offset-1 md:text-xl"
              />
            </FormItem>
          )}
        />
      </form>
    </Form>
  );
};

export default CarouselCardBalanceForm;
