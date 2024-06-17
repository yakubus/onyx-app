import { useMutation, useQueryClient } from "@tanstack/react-query";

import { getCategoriesQueryOptions } from "@/lib/api/category";
import { createSubcategory } from "@/lib/api/subcategory";
import { capitalize } from "@/lib/utils";

interface SubcategoryMutationProps {
  budgetId: string;
  parentCategoryId: string;
  onMutationError: () => void;
  onMutationSuccess: () => void;
}

export const useCreateSubcategoryMutation = ({
  budgetId,
  parentCategoryId,
  onMutationError,
  onMutationSuccess,
}: SubcategoryMutationProps) => {
  const queryClient = useQueryClient();
  const queryKey = getCategoriesQueryOptions(budgetId).queryKey;

  return useMutation({
    mutationFn: createSubcategory,
    onMutate: async (newSubcategory) => {
      await queryClient.cancelQueries({ queryKey });

      const previousCategories = queryClient.getQueryData(queryKey);

      queryClient.setQueryData(queryKey, (old) => {
        if (!old) return old;

        return old.map((oldCategory) => {
          if (oldCategory.id !== parentCategoryId) {
            return oldCategory;
          }

          const subcategories = oldCategory.subcategories || [];

          return {
            ...oldCategory,
            subcategories: [
              ...subcategories,
              {
                id: "123456",
                name: capitalize(newSubcategory.subcategoryName),
                optimistic: true,
                assignments: null,
                target: null,
                description: null,
              },
            ],
          };
        });
      });

      return { previousCategories };
    },
    onError: (_err, _newTodo, context) => {
      if (!context) return context;
      queryClient.setQueryData(
        getCategoriesQueryOptions(budgetId).queryKey,
        context.previousCategories,
      );
      onMutationError();
    },
    onSettled: () => {
      queryClient.invalidateQueries({
        queryKey: getCategoriesQueryOptions(budgetId).queryKey,
      });
    },
    onSuccess: () => {
      onMutationSuccess();
    },
  });
};
