import { FC } from "react";
import { FieldValues, SubmitHandler } from "react-hook-form";
import { useParams, useSearch } from "@tanstack/react-router";

import AmountInput from "@/components/dashboard/AmountInput";
import { Form, FormField, FormItem } from "@/components/ui/form";

import { useClickOutside } from "@/lib/hooks/useClickOutside";
import { FormAssignment, assign } from "@/lib/api/subcategory";
import { getCategoriesQueryOptions } from "@/lib/api/category";
import { formatAmount, removeSpacesFromAmount } from "@/lib/utils";
import useAmountForm from "@/lib/hooks/useAmountForm";

interface AssignmentFormProps {
  defaultAmount: string | undefined;
  subcategoryId: string;
  currencyToDisplay: string;
}

const AssignmentForm: FC<AssignmentFormProps> = ({
  defaultAmount,
  subcategoryId,
  currencyToDisplay,
}) => {
  const { budgetId: selectedBudget } = useParams({
    from: "/_dashboard-layout/budget/$budgetId/",
  });
  const { month, year } = useSearch({
    from: "/_dashboard-layout/budget/$budgetId/",
  });
  const defaultInputValue = defaultAmount
    ? formatAmount(defaultAmount)
    : "0.00";
  const { control, form, handleSubmit, isDirty, mutate } = useAmountForm({
    defaultAmount: defaultInputValue,
    mutationFn: assign,
    queryKey: getCategoriesQueryOptions(selectedBudget).queryKey,
  });

  const onSubmit: SubmitHandler<FieldValues> = (data) => {
    const { amount } = data;
    const amountWithoutCommas = removeSpacesFromAmount(amount);
    if (Number(amountWithoutCommas) === Number(defaultAmount)) return;

    const assignment: FormAssignment = {
      assignedAmount: Number(amountWithoutCommas),
      assignmentMonth: {
        month: Number(month),
        year: Number(year),
      },
    };
    mutate({ budgetId: selectedBudget, subcategoryId, assignment });
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
            <FormItem className="flex items-center space-y-0">
              <AmountInput
                field={field}
                className="relative h-8 border-none bg-transparent px-1 pr-10 text-right text-base"
              />
              <p className="absolute right-12 pl-2 text-base">
                {currencyToDisplay}
              </p>
            </FormItem>
          )}
        />
      </form>
    </Form>
  );
};

export default AssignmentForm;
