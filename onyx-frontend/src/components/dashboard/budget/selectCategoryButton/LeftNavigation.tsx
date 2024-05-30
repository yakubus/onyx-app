import { FC, useState } from "react";
import { useMutation, useQueryClient } from "@tanstack/react-query";

import { deleteCategory, getCategoriesQueryOptions } from "@/lib/api/category";

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

import { type SelectCategorySectionProps } from "./SelectCategoryButton";

const LeftNavigation: FC<SelectCategorySectionProps> = ({
  category,
  isEdit,
  setIsEdit,
  isSelected,
}) => {
  const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState(false);
  const queryClient = useQueryClient();
  const { id } = category;

  const { mutate } = useMutation({
    mutationKey: ["deleteCategory", id],
    mutationFn: deleteCategory,
    onSettled: async () => {
      return await queryClient.invalidateQueries({
        queryKey: getCategoriesQueryOptions.queryKey,
      });
    },
  });

  const onDelete = () => {
    mutate(id);
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
          <DropdownMenuTrigger>
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
          <DialogFooter>
            <Button type="submit" variant="destructive" onClick={onDelete}>
              Delete
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    );
};

export default LeftNavigation;
