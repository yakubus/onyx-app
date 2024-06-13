import { FC } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useParams, useSearch } from "@tanstack/react-router";
import { zodResolver } from "@hookform/resolvers/zod";

import { Check } from "lucide-react";
import {
  HoverCard,
  HoverCardContent,
  HoverCardTrigger,
} from "@/components/ui/hover-card";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";

import {
  CreateAssignment,
  CreateAssignmentSchema,
} from "@/lib/validation/subcategory";
import { cn } from "@/lib/utils";
import { getCategoriesQueryOptions } from "@/lib/api/category";
import { FormAssignment, assign } from "@/lib/api/subcategory";
import { Assignment } from "@/lib/validation/base";

interface AssignmentTooltipProps {
  isAssignmentTooltipOpen: boolean;
  setIsAssignmentTooltipOpen: (state: boolean) => void;
  subcategoryId: string;
  currentlyAssigned: Assignment | undefined;
  currencyToDisplay: string;
}

const AssignmentTooltip: FC<AssignmentTooltipProps> = ({
  isAssignmentTooltipOpen,
  setIsAssignmentTooltipOpen,
  subcategoryId,
  currentlyAssigned,
  currencyToDisplay,
}) => {
  const queryClient = useQueryClient();
  const { budgetId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId",
  });
  const { month, year } = useSearch({
    from: "/_dashboard-layout/budget/$budgetId",
  });

  const amountToDisplay =
    currentlyAssigned?.assignedAmount.amount.toString() || "0";

  const form = useForm<CreateAssignment>({
    defaultValues: {
      amount: amountToDisplay,
    },
    resolver: zodResolver(CreateAssignmentSchema),
  });

  const { handleSubmit, control, setError } = form;

  const { mutate, isPending, variables } = useMutation({
    mutationFn: assign,
    onSettled: async () => {
      return await Promise.all([
        queryClient.invalidateQueries(getCategoriesQueryOptions(budgetId)),
        queryClient.invalidateQueries({ queryKey: ["toAssign", budgetId] }),
      ]);
    },
    onError: () => {
      setError("amount", { message: "Error occured. Try again." });
    },
  });

  const onSubmit: SubmitHandler<CreateAssignment> = (data) => {
    const { amount } = data;
    const assignment: FormAssignment = {
      assignedAmount: Number(amount),
      assignmentMonth: {
        month: Number(month),
        year: Number(year),
      },
    };
    mutate({ budgetId, subcategoryId, assignment });
  };

  return (
    <div>
      <HoverCard
        open={isAssignmentTooltipOpen}
        onOpenChange={setIsAssignmentTooltipOpen}
      >
        <HoverCardTrigger>
          <span className={cn(isPending && "opacity-50")}>
            {isPending ? (
              <span>
                {variables.assignment.assignedAmount} {currencyToDisplay}
              </span>
            ) : (
              <span>
                {amountToDisplay} {currencyToDisplay}
              </span>
            )}
          </span>
        </HoverCardTrigger>
        <HoverCardContent>
          <Form {...form}>
            <form
              onSubmit={handleSubmit(onSubmit)}
              className="flex items-center px-1 py-2"
            >
              <FormField
                control={control}
                name="amount"
                disabled={isPending}
                render={({ field }) => (
                  <FormItem className="w-full space-y-1">
                    <div className="flex space-x-2">
                      <FormControl>
                        <Input {...field} type="number" min="0" step="1" />
                      </FormControl>
                      <Button
                        type="submit"
                        variant="ghost"
                        size="icon"
                        className="w-12 rounded-lg"
                      >
                        <Check className="opacity-70" />
                      </Button>
                    </div>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </form>
          </Form>
        </HoverCardContent>
      </HoverCard>
    </div>
  );
};

export default AssignmentTooltip;
