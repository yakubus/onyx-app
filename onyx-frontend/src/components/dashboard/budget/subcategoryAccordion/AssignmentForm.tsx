import { FC, useEffect } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useSearch } from "@tanstack/react-router";
import { useMutation, useQueryClient } from "@tanstack/react-query";

import { Input } from "@/components/ui/input";
import { Form, FormControl, FormField, FormItem } from "@/components/ui/form";

import {
  CreateAssignment,
  CreateAssignmentSchema,
} from "@/lib/validation/subcategory";
import { useClickOutside } from "@/lib/hooks/useClickOutside";
import { FormAssignment, assign } from "@/lib/api/subcategory";
import { getCategoriesQueryOptions } from "@/lib/api/category";
import { useToast } from "@/components/ui/use-toast";

interface AssignmentFormProps {
  defaultAmount: string | undefined;
  subcategoryId: string;
}

const AssignmentForm: FC<AssignmentFormProps> = ({
  defaultAmount,
  subcategoryId,
}) => {
  const { toast } = useToast();
  const queryClient = useQueryClient();
  const { month, year, selectedBudget } = useSearch({
    from: "/_dashboard-layout/budget/$budgetId",
  });
  const form = useForm<CreateAssignment>({
    defaultValues: {
      amount: defaultAmount || "0",
    },
    resolver: zodResolver(CreateAssignmentSchema),
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
    if (isError) {
      reset({
        amount: defaultAmount,
      });
    }
  }, [isError, reset]);

  const onSubmit: SubmitHandler<CreateAssignment> = (data) => {
    const { amount } = data;
    if (Number(amount) === Number(defaultAmount)) return;

    const assignment: FormAssignment = {
      assignedAmount: Number(amount),
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
            <FormItem className="space-y-0">
              <FormControl>
                <Input
                  type="number"
                  min="0"
                  step="any"
                  {...field}
                  onChange={(e) => {
                    const { value } = e.target;
                    let transformedValue = value.replace(/^-/, "");
                    if (
                      transformedValue.startsWith("0") &&
                      transformedValue !== "0"
                    ) {
                      transformedValue = transformedValue.replace(/^0+/, "");
                    }
                    field.onChange(transformedValue);
                  }}
                  className="h-8 border-none bg-transparent pb-2 pr-1 pt-1 text-right text-base font-semibold"
                />
              </FormControl>
            </FormItem>
          )}
        />
      </form>
    </Form>
  );
};

export default AssignmentForm;
