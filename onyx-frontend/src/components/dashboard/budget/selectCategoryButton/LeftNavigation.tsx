import { FC, useState } from "react";
import { useParams } from "@tanstack/react-router";

import { Settings, X } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";

import { type SelectCategorySectionProps } from "@/components/dashboard/budget/selectCategoryButton/SelectCategoryButton";
import { useDeleteCategoryMutation } from "@/lib/hooks/mutations/useDeleteCategoryMutation";

const LeftNavigation: FC<SelectCategorySectionProps> = ({
  category,
  isEdit,
  setIsEdit,
  isSelected,
}) => {
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const { budgetId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId/",
  });

  const onMutationError = () => {
    setIsDeleteDialogOpen(true);
  };

  const { mutate, isError } = useDeleteCategoryMutation({
    budgetId,
    onMutationError,
  });

  const onDelete = () => {
    mutate({ budgetId, categoryId: category.id });
    setIsDeleteDialogOpen(false);
  };

  if (isEdit)
    return (
      <Button onClick={() => setIsEdit(false)}>
        <X />
      </Button>
    );

  if (!isEdit && isSelected)
    return (
      <Dialog open={isDeleteDialogOpen} onOpenChange={setIsDeleteDialogOpen}>
        <DropdownMenu>
          <DropdownMenuTrigger className="outline-none">
            <Settings />
          </DropdownMenuTrigger>
          <DropdownMenuContent align="start">
            <DropdownMenuItem onClick={() => setIsEdit(true)}>
              Edit
            </DropdownMenuItem>
            <DialogTrigger asChild>
              <DropdownMenuItem>Delete</DropdownMenuItem>
            </DialogTrigger>
          </DropdownMenuContent>
        </DropdownMenu>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Are you absolutely sure?</DialogTitle>
            <DialogDescription>
              This action cannot be undone. This will permanently delete this
              category and remove all your assigments.
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

export default LeftNavigation;
