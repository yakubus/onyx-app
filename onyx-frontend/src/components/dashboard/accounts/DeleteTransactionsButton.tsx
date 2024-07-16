import { FC, useState } from "react";
import { useParams } from "@tanstack/react-router";
import { Row } from "@tanstack/react-table";

import { Trash } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";

import { Transaction } from "@/lib/validation/transaction";
import { useDeleteTransactionsMutation } from "@/lib/hooks/mutations/useDeleteTransactionsMutation";

interface DeleteTransactionsButtonProps {
  rows: Row<Transaction>[];
}

const DeleteTransactionsButton: FC<DeleteTransactionsButtonProps> = ({
  rows,
}) => {
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const { budgetId, accountId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId/accounts/$accountId",
  });

  const onMutationError = () => {
    setIsDeleteDialogOpen(true);
  };

  const { mutate, isError } = useDeleteTransactionsMutation({
    budgetId,
    accountId,
    onMutationError,
  });

  const onDelete = () => {
    mutate({ budgetId, rows });
    setIsDeleteDialogOpen(false);
  };

  return (
    <Dialog open={isDeleteDialogOpen} onOpenChange={setIsDeleteDialogOpen}>
      <DialogTrigger asChild>
        <Button variant="outline" className="space-x-2">
          <Trash className="inline-flex size-5 flex-shrink-0 opacity-90" />
          <span className="inline-flex">Delete ({rows.length.toString()})</span>
        </Button>
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Are you absolutely sure?</DialogTitle>
          <DialogDescription>
            This action cannot be undone. This will permanently delete all
            selected transactions.
          </DialogDescription>
        </DialogHeader>
        <DialogFooter className="items-center">
          {isError && (
            <p className="text-end text-sm text-destructive">
              Something went wrong. Please try again.
            </p>
          )}
          <Button type="submit" variant="destructive" onClick={onDelete}>
            Delete
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};

export default DeleteTransactionsButton;
