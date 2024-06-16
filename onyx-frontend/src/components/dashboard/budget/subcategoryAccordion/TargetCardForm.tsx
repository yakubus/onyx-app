import { FC } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useSearch } from "@tanstack/react-router";
import { useMutation, useQueryClient } from "@tanstack/react-query";

import { X } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";

import { Target } from "@/lib/validation/base";
import { CreateTarget, CreateTargetSchema } from "@/lib/validation/subcategory";
import { useClickOutside } from "@/lib/hooks/useClickOutside";
import TargetCardFormDatePicker from "./TargetCardFormDatePicker";
import { FormTarget, createTarget } from "@/lib/api/subcategory";
import { getCategoriesQueryOptions } from "@/lib/api/category";

interface TargetCardFormProps {
  currentTarget: Target | undefined | null;
  setIsCreating: (state: boolean) => void;
  subcategoryId: string;
}

const TargetCardForm: FC<TargetCardFormProps> = ({
  currentTarget,
  setIsCreating,
  subcategoryId,
}) => {
  const queryClient = useQueryClient();
  const {
    month: searchMonth,
    selectedBudget,
    year: searchYear,
  } = useSearch({
    from: "/_dashboard-layout/budget/$budgetId",
  });

  const isEditing = !!currentTarget;
  const currentTargetAmount = currentTarget?.targetAmount.amount;
  const currentTargetUpToDate = currentTarget?.upToMonth;

  const form = useForm<CreateTarget>({
    resolver: zodResolver(CreateTargetSchema),
    defaultValues: {
      amount: currentTargetAmount?.toString() || "",
      month:
        currentTargetUpToDate?.month.toString() ||
        Number(searchMonth).toString(),
      year: currentTargetUpToDate?.year.toString() || searchYear,
    },
  });
  const {
    handleSubmit,
    control,
    clearErrors,
    formState: { errors },
  } = form;

  const { mutate } = useMutation({
    mutationKey: ["createTarget"],
    mutationFn: createTarget,
    onSettled: async () => {
      setIsCreating(false);
      return await queryClient.invalidateQueries({
        queryKey: getCategoriesQueryOptions(selectedBudget).queryKey,
      });
    },
  });

  const onSubmit: SubmitHandler<CreateTarget> = (data) => {
    const { month, year, amount } = data;

    const target: FormTarget = {
      targetAmount: Number(amount),
      startedAt: {
        month: new Date().getMonth(),
        year: new Date().getFullYear(),
      },
      targetUpToMonth: {
        month: Number(month),
        year: Number(year),
      },
    };

    mutate({
      budgetId: selectedBudget,
      subcategoryId,
      formTarget: target,
    });
  };

  const formRef = useClickOutside<HTMLFormElement>(() => {
    if (errors) {
      clearErrors();
    }
  });

  return (
    <div>
      <Form {...form}>
        <form
          onSubmit={handleSubmit(onSubmit)}
          ref={formRef}
          className="my-4 space-y-2"
        >
          <FormField
            control={control}
            name="amount"
            render={({ field }) => (
              <FormItem className="grid grid-cols-3 items-center gap-x-1 space-y-0">
                <FormLabel className="col-span-1">Amount:</FormLabel>
                <FormControl>
                  <Input
                    placeholder="Set amount..."
                    type="number"
                    {...field}
                    className="col-span-2 bg-card focus-visible:ring-0 focus-visible:ring-offset-0"
                  />
                </FormControl>
                <FormMessage className="col-span-3 pt-1" />
              </FormItem>
            )}
          />
          <FormItem className="grid grid-cols-3 place-items-center gap-x-1 space-y-0">
            <FormLabel className="col-span-1">Up to month:</FormLabel>
            <FormControl>
              <div className="col-span-2 h-10 w-full rounded-md border border-input px-3">
                <TargetCardFormDatePicker
                  searchMonth={searchMonth}
                  searchYear={searchYear}
                />
              </div>
            </FormControl>
            <FormMessage />
          </FormItem>
          <div className="pt-6">
            <Button type="submit" className="w-full">
              Set target
            </Button>
          </div>
        </form>
      </Form>
      {isEditing && (
        <Button
          className="w-full"
          variant="outline"
          onClick={() => setIsCreating(false)}
        >
          <X />
        </Button>
      )}
    </div>
  );
};

export default TargetCardForm;
