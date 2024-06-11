import { FC } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useParams } from "@tanstack/react-router";
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
  Subcategory,
} from "@/lib/validation/subcategory";
import { cn } from "@/lib/utils";
import { getCategoriesQueryOptions } from "@/lib/api/category";

interface AssignmentTooltipProps {
  isAssignmentTooltipOpen: boolean;
  setIsAssignmentTooltipOpen: (state: boolean) => void;
  subcategory: Subcategory;
}

const AssignmentTooltip: FC<AssignmentTooltipProps> = ({
  isAssignmentTooltipOpen,
  setIsAssignmentTooltipOpen,
  subcategory,
}) => {
  const queryClient = useQueryClient();
  const { budgetId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId",
  });
  const form = useForm<CreateAssignment>({
    defaultValues: {
      amount: "0",
    },
    resolver: zodResolver(CreateAssignmentSchema),
  });

  const { handleSubmit, control, setError } = form;

  const { mutate, isPending, variables } = useMutation({
    mutationFn: () => "",
    onSettled: async () => {
      return await queryClient.invalidateQueries(
        getCategoriesQueryOptions(budgetId),
      );
    },
    onError: () => {
      setError("amount", { message: "Error occured. Try again." });
    },
  });

  const onSubmit: SubmitHandler<CreateAssignment> = (data) => {
    const { amount } = data;
  };

  return (
    <HoverCard
      open={isAssignmentTooltipOpen}
      onOpenChange={setIsAssignmentTooltipOpen}
    >
      <HoverCardTrigger>
        <span className={cn(isPending && "opacity-50")}>
          {isPending ? "" : subcategory.name}
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
                      <Input {...field} type="number" />
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
  );
};

export default AssignmentTooltip;
