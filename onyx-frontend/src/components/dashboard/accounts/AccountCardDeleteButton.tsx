import { FC, useState } from "react";
import { useParams } from "@tanstack/react-router";

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

import { useDeleteAccountMutation } from "@/lib/hooks/mutations/useDeleteAccountMutation";

interface AccountCardDeleteButtonProps {
  accountId: string;
}

const AccountCardDeleteButton: FC<AccountCardDeleteButtonProps> = ({
  accountId,
}) => {
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const { budgetId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId/accounts/$accountId",
  });

  const onMutationError = () => {
    setIsDeleteDialogOpen(true);
  };

  const { mutate, isError } = useDeleteAccountMutation({
    budgetId,
    onMutationError,
  });

  const onDelete = () => {
    mutate({ accountId, budgetId });
    setIsDeleteDialogOpen(false);
  };

  return (
    <Dialog open={isDeleteDialogOpen} onOpenChange={setIsDeleteDialogOpen}>
      <DialogTrigger asChild>
        <Button
          variant="ghost"
          size="icon"
          className="opacity-50 hover:bg-transparent hover:text-primary-foreground hover:opacity-100"
        >
          <Trash />
        </Button>
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Are you absolutely sure?</DialogTitle>
          <DialogDescription>
            This action cannot be undone. This will permanently delete this
            account and remove all data connected.
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

export default AccountCardDeleteButton;
