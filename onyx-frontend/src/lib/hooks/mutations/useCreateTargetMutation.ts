import { useMutation, useQueryClient } from "@tanstack/react-query";

import { getCategoriesQueryOptions } from "@/lib/api/category";
import { createTarget } from "@/lib/api/subcategory";
import { DEFAULT_MONTH_NUMBER } from "@/lib/constants/date";

interface TargetMutationProps {
  budgetId: string;
  currency: string;
  onMutationError: () => void;
}

export const useCreateTargetMutation = ({
  budgetId,
  currency,
  onMutationError,
}: TargetMutationProps) => {
  const queryClient = useQueryClient();
  const queryKey = getCategoriesQueryOptions(budgetId).queryKey;

  return useMutation({
    mutationKey: ["createTarget"],
    mutationFn: createTarget,
    onMutate: (newTarget) => {
      queryClient.cancelQueries({ queryKey });
      const previousCategories = queryClient.getQueryData(queryKey);

      queryClient.setQueryData(queryKey, (old) => {
        if (!old || !Array.isArray(old)) return old;

        return old.map((category) => {
          const { subcategoryId, formTarget } = newTarget;
          const selectedSubcategory = category.subcategories.find(
            (sub) => sub.id === subcategoryId,
          );

          if (selectedSubcategory) {
            const {
              targetAmount,
              startedAt,
              targetUpToMonth: upToMonth,
            } = formTarget;
            return {
              ...category,
              subcategories: category.subcategories.map((sub) => {
                if (sub.id === selectedSubcategory.id) {
                  return {
                    ...sub,
                    target: {
                      targetAmount: { amount: Number(targetAmount), currency },
                      startedAt,
                      upToMonth,
                      collectedAmount: { amount: 0, currency },
                      amountAssignedEveryMonth: {
                        amount:
                          targetAmount /
                          (upToMonth.month - DEFAULT_MONTH_NUMBER),
                        currency,
                      },
                      optimistic: true,
                    },
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
      queryClient.invalidateQueries({ queryKey });
    },
    onError: (err, _newTodo, context) => {
      console.log(err);
      queryClient.setQueryData(queryKey, context);
      onMutationError();
    },
  });
};
