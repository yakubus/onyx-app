import { FC } from "react";
import { type SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useParams } from "@tanstack/react-router";

import { ChevronRight, Plus } from "lucide-react";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Button } from "@/components/ui/button";

import {
  type CreateSubcategory,
  CreateSubcategorySchema,
} from "@/lib/validation/subcategory";
import { useClickOutside } from "@/lib/hooks/useClickOutside";
import { createSubcategory } from "@/lib/api/subcategory";
import { getCategoriesQueryOptions } from "@/lib/api/category";
import { capitalize } from "@/lib/utils";

interface CreateSubcategoryButtonProps {
  parentCategoryId: string;
}

const CreateSubcategoryButton: FC<CreateSubcategoryButtonProps> = ({
  parentCategoryId,
}) => {
  const queryClient = useQueryClient();
  const { budgetId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId",
  });
  const form = useForm<CreateSubcategory>({
    defaultValues: {
      name: "",
    },
    resolver: zodResolver(CreateSubcategorySchema),
  });
  const { handleSubmit, control, clearErrors, reset } = form;

  const { mutate } = useMutation({
    mutationFn: createSubcategory,
    onMutate: async (newSubcategory) => {
      const queryKey = getCategoriesQueryOptions(budgetId).queryKey;

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
    },
    onSettled: () => {
      queryClient.invalidateQueries({
        queryKey: getCategoriesQueryOptions(budgetId).queryKey,
      });
    },
    onSuccess: () => {
      reset();
    },
  });

  const onSubmit: SubmitHandler<CreateSubcategory> = (data) => {
    const { name: subcategoryName } = data;
    mutate({ budgetId, parentCategoryId, subcategoryName });
  };

  const formRef = useClickOutside<HTMLFormElement>(() => {
    clearErrors();
  });

  return (
    <Form {...form}>
      <form
        onSubmit={handleSubmit(onSubmit)}
        className="flex items-center border-t px-1 py-2"
        ref={formRef}
      >
        <FormField
          control={control}
          name="name"
          render={({ field }) => (
            <FormItem className="w-full space-y-1">
              <div className="flex">
                <FormLabel className="flex h-10 w-10 items-center justify-center opacity-60">
                  <ChevronRight />
                </FormLabel>
                <FormControl>
                  <input
                    placeholder="Add subcategory..."
                    {...field}
                    className="mr-4 h-10 flex-1 rounded-md px-4 outline-none ring-secondary ring-offset-0 focus:ring-1"
                  />
                </FormControl>
                <Button type="submit" variant="ghost" size="icon">
                  <Plus />
                </Button>
              </div>
              <FormMessage className="ml-14" />
            </FormItem>
          )}
        />
      </form>
    </Form>
  );
};

export default CreateSubcategoryButton;
