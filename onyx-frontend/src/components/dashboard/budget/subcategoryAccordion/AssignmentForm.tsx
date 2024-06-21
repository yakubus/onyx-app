import { FC, useEffect } from "react";
import { FieldValues, SubmitHandler, useForm } from "react-hook-form";
import { useSearch } from "@tanstack/react-router";
import { useMutation, useQueryClient } from "@tanstack/react-query";

import { Input } from "@/components/ui/input";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
} from "@/components/ui/form";

import { assignmentLiveValidation } from "@/lib/validation/subcategory";
import { useClickOutside } from "@/lib/hooks/useClickOutside";
import { FormAssignment, assign } from "@/lib/api/subcategory";
import { getCategoriesQueryOptions } from "@/lib/api/category";
import { useToast } from "@/components/ui/use-toast";
import {
  addCommasToAmount,
  formatAmount,
  formatDecimals,
  removeCommasFromAmount,
} from "@/lib/utils";

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
  const { toast } = useToast();
  const queryClient = useQueryClient();
  const { month, year, selectedBudget } = useSearch({
    from: "/_dashboard-layout/_budget-only-layout/budget/$budgetId",
  });
  const defaultInputValue = defaultAmount
    ? formatAmount(defaultAmount)
    : "0.00";
  const form = useForm({
    defaultValues: {
      amount: defaultInputValue,
    },
  });

  const {
    handleSubmit,
    control,
    formState: { isDirty },
    reset,
  } = form;

  const { mutate, isError } = useMutation({
    mutationFn: assign,
    onSettled: async () => {
      return await Promise.all([
        queryClient.invalidateQueries(
          getCategoriesQueryOptions(selectedBudget),
        ),
        queryClient.invalidateQueries({
          queryKey: ["toAssign", selectedBudget],
        }),
      ]);
    },
    onError: (error) => {
      console.error("Mutation error:", error);
      toast({
        title: "Error",
        description: "Oops... Something went wrong. Please try again later.",
      });
    },
  });

  useEffect(() => {
    reset({
      amount: defaultInputValue,
    });
  }, [defaultInputValue, reset]);

  useEffect(() => {
    if (isError) {
      reset({
        amount: defaultInputValue,
      });
    }
  }, [isError, reset, defaultInputValue]);

  const onSubmit: SubmitHandler<FieldValues> = (data) => {
    const { amount } = data;
    const amountWithoutCommas = removeCommasFromAmount(amount);
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
              <FormControl>
                <Input
                  type="text"
                  step="any"
                  autoComplete="off"
                  {...field}
                  onChange={(e) => {
                    let { value } = e.target;
                    value = assignmentLiveValidation(value);
                    value = addCommasToAmount(value);
                    field.onChange(value);
                  }}
                  onBlur={(e) => {
                    const { value } = e.target;
                    const formattedValue = formatDecimals(value);
                    field.onChange(formattedValue);
                  }}
                  className="h-8 border-none bg-transparent px-1 pb-2 pt-1 text-right text-base"
                />
              </FormControl>
              <FormLabel className="pl-2 text-base">
                {currencyToDisplay}
              </FormLabel>
            </FormItem>
          )}
        />
      </form>
    </Form>
  );
};

export default AssignmentForm;
