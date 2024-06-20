import { useMutation, useQueryClient } from "@tanstack/react-query";

import { getCategoriesQueryOptions } from "@/lib/api/category";
import { deleteSubcategory } from "@/lib/api/subcategory";
import { getToAssignQueryKey } from "@/lib/api/budget";

interface DeleteSubcategoryMutationProps {
  budgetId: string;
  onMutationError: () => void;
}

export const useDeleteSubcategoryMutation = ({
  budgetId,
  onMutationError,
}: DeleteSubcategoryMutationProps) => {
  const queryClient = useQueryClient();
  const categoriesQueryKey = getCategoriesQueryOptions(budgetId).queryKey;
  const toAssignQueryKey = getToAssignQueryKey(budgetId);

  return useMutation({
    mutationKey: ["deleteSubcategory"],
    mutationFn: deleteSubcategory,
    onMutate: (deletedSubcategory) => {
      queryClient.cancelQueries({ queryKey: categoriesQueryKey });
      const previousCategories = queryClient.getQueryData(categoriesQueryKey);

      queryClient.setQueryData(categoriesQueryKey, (old) => {
        if (!old || !Array.isArray(old)) return old;

        return old.map((category) => {
          const existingSub = category.subcategories.find(
            (s) => s.id === deletedSubcategory.subcategoryId,
          );

          if (existingSub) {
            return {
              ...category,
              subcategories: category.subcategories.map((sub) => {
                if (sub.id === deletedSubcategory.subcategoryId) {
                  return {
                    ...sub,
                    optimistic: true,
                  };
                }
                return sub;
              }),
            };
          }
          return category;
        });
      });

      return previousCategories;
    },
    onSettled: () => {
      Promise.all([
        queryClient.invalidateQueries({
          queryKey: categoriesQueryKey,
        }),
        queryClient.invalidateQueries({
          queryKey: toAssignQueryKey,
        }),
      ]);
    },
    onError: (_err, _newTodo, context) => {
      queryClient.setQueryData(categoriesQueryKey, context);
      onMutationError();
    },
  });
};
