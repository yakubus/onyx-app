import { FC, useState } from "react";
import { useParams } from "@tanstack/react-router";

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

import { useDeleteSubcategoryMutation } from "@/lib/hooks/mutations/useDeleteSubcategoryMutation";

interface DeleteSubcategoryButtonProps {
  subcategoryId: string;
}

const DeleteSubcategoryButton: FC<DeleteSubcategoryButtonProps> = ({
  subcategoryId,
}) => {
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const { budgetId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId",
  });

  const onMutationError = () => {
    setIsDeleteDialogOpen(true);
  };

  const { mutate, isError } = useDeleteSubcategoryMutation({
    budgetId,
    onMutationError,
  });

  const onDelete = () => {
    mutate({ budgetId, subcategoryId });
    setIsDeleteDialogOpen(false);
  };

  return (
    <Dialog open={isDeleteDialogOpen} onOpenChange={setIsDeleteDialogOpen}>
      <DialogTrigger asChild>
        <Button className="w-full" variant="outline">
          Delete
        </Button>
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Are you absolutely sure?</DialogTitle>
          <DialogDescription>
            This action cannot be undone. This will permanently delete this
            subcategory and remove all your assignments.
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

export default DeleteSubcategoryButton;
