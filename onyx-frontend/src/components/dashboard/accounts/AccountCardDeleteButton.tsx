import { FC, useState } from "react";
import { useParams } from "@tanstack/react-router";

import { Trash } from "lucide-react";
import { Button } from "@/components/ui/button";
import LoadingButton from "@/components/LoadingButton";
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

  const onMutationSuccess = () => {
    setIsDeleteDialogOpen(false);
  };

  const { mutate, isError, isPending } = useDeleteAccountMutation({
    budgetId,
    onMutationSuccess,
  });

  const onDelete = () => {
    mutate({ accountId, budgetId });
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
          <LoadingButton
            type="submit"
            variant="destructive"
            onClick={onDelete}
            isLoading={isPending}
          >
            Delete
          </LoadingButton>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};

export default AccountCardDeleteButton;
