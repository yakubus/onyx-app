import { FC, useState } from "react";
import { Link } from "@tanstack/react-router";
import { useMutation, useQueryClient } from "@tanstack/react-query";

import { Edit, Trash } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  Tooltip,
  TooltipContent,
  TooltipProvider,
  TooltipTrigger,
} from "@/components/ui/tooltip";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";

import type { Budget } from "@/lib/validation/budget";
import { deleteBudget, getBudgetsQueryOptions } from "@/lib/api/budget";
import { cn } from "@/lib/utils";

interface BudgetTableRowProps {
  budget: Budget;
}

const BudgetTableRow: FC<BudgetTableRowProps> = ({ budget }) => {
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const queryClient = useQueryClient();
  const { id } = budget;

  const { mutate, isError, isPending } = useMutation({
    mutationKey: ["deleteBudget", id],
    mutationFn: deleteBudget,
    onSettled: async () => {
      return await queryClient.invalidateQueries({
        queryKey: getBudgetsQueryOptions.queryKey,
      });
    },
    onError: () => {
      setIsDeleteDialogOpen(true);
    },
  });

  const onDelete = () => {
    mutate(id);
    setIsDeleteDialogOpen(false);
  };

  return (
    <Dialog open={isDeleteDialogOpen} onOpenChange={setIsDeleteDialogOpen}>
      <TooltipProvider>
        <Tooltip delayDuration={300}>
          <TooltipTrigger asChild>
            <li
              className={cn("border-t", {
                "opacity-50": isPending || budget.optimistic,
              })}
            >
              <Link
                to="/budget/$budgetId"
                params={{ budgetId: budget.id }}
                search={{
                  selectedBudget: budget.id,
                  month: String(new Date().getMonth() + 1),
                  year: String(new Date().getFullYear()),
                }}
                mask={{ to: `/budget/${budget.id}` }}
                preload="intent"
                className="grid w-full grid-cols-9 gap-x-4 px-4 py-8 hover:bg-accent"
              >
                <p className="col-span-3">{budget.name}</p>
                <p className="col-span-2">{budget.currency}</p>
                <p className="col-span-4 flex flex-col">
                  {budget.userIds.map((id) => (
                    <span key={id}>{id}</span>
                  ))}
                </p>
              </Link>
            </li>
          </TooltipTrigger>
          <TooltipContent side="left">
            <div className="flex flex-col space-y-1">
              <Button variant="outline" className="rounded-lg" size="icon">
                <Edit className="size-5" />
              </Button>
              <DialogTrigger asChild>
                <Button variant="outline" className="rounded-lg" size="icon">
                  <Trash className="size-5" />
                </Button>
              </DialogTrigger>
            </div>
          </TooltipContent>
        </Tooltip>
      </TooltipProvider>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Are you absolutely sure?</DialogTitle>
          <DialogDescription>
            This action cannot be undone. This will permanently delete your
            account and remove your data from our servers.
          </DialogDescription>
        </DialogHeader>
        <DialogFooter className="items-center">
          {isError && (
            <p className="text-end text-sm text-destructive">
              Something went wrong. Please try again.
            </p>
          )}
          <Button
            type="submit"
            size="sm"
            variant="destructive"
            onClick={onDelete}
          >
            Delete
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};

export default BudgetTableRow;
