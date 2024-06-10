import { useMutation, useQueryClient } from "@tanstack/react-query";

import { deleteCategory, getCategoriesQueryOptions } from "@/lib/api/category";

interface CategoryMutationProps {
  budgetId: string;
  onMutationError: () => void;
}

export const useDeleteCategoryMutation = ({
  budgetId,
  onMutationError,
}: CategoryMutationProps) => {
  const queryClient = useQueryClient();
  const queryKey = getCategoriesQueryOptions(budgetId).queryKey;

  return useMutation({
    mutationKey: ["deleteCategory"],
    mutationFn: deleteCategory,
    onMutate: (deletedCategory) => {
      queryClient.cancelQueries({
        queryKey,
      });
      const previousCategories = queryClient.getQueryData(queryKey);

      queryClient.setQueryData(queryKey, (old) => {
        if (!old || !Array.isArray(old)) return old;

        return old.map((category) => {
          if (category.id === deletedCategory.categoryId) {
            return {
              ...category,
              optimistic: true,
            };
          }
          return category;
        });
      });

      return previousCategories;
    },
    onSettled: () => {
      queryClient.invalidateQueries({
        queryKey,
      });
    },
    onError: (_err, _newTodo, context) => {
      queryClient.setQueryData(queryKey, context);
      onMutationError();
    },
  });
};
