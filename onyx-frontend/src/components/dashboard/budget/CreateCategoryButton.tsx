import { FC } from "react";
import { FieldValues, SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useParams } from "@tanstack/react-router";

import { Plus } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormMessage,
} from "@/components/ui/form";

import { useClickOutside } from "@/lib/hooks/useClickOutside";
import { createCategory, getCategoriesQueryOptions } from "@/lib/api/category";
import {
  type CreateCategory,
  CreateCategorySchema,
} from "@/lib/validation/category";
import { capitalize } from "@/lib/utils";

interface Props {
  categoriesCount: number;
}

const AddCategoryButton: FC<Props> = ({ categoriesCount }) => {
  const { budgetId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId",
  });
  const form = useForm<CreateCategory>({
    resolver: zodResolver(CreateCategorySchema),
    defaultValues: {
      name: "",
    },
  });
  const {
    handleSubmit,
    control,
    clearErrors,
    formState: { errors },
    reset,
    setError,
  } = form;
  const queryClient = useQueryClient();

  const { mutate } = useMutation({
    mutationKey: ["createCategory"],
    mutationFn: createCategory,
    onMutate: async (newCategory) => {
      await queryClient.cancelQueries({
        queryKey: getCategoriesQueryOptions(budgetId).queryKey,
      });

      const previousBudgetWithPayload = queryClient.getQueryData(
        getCategoriesQueryOptions(budgetId).queryKey,
      );

      queryClient.setQueryData(
        getCategoriesQueryOptions(budgetId).queryKey,
        (old) => {
          if (!old) return old;

          return [
            ...old,
            {
              id: "12345",
              name: capitalize(newCategory.name),
              subcategories: [],
              optimistic: true,
            },
          ];
        },
      );

      return { previousBudgetWithPayload };
    },
    onSettled: () => {
      queryClient.invalidateQueries({
        queryKey: getCategoriesQueryOptions(budgetId).queryKey,
      });
    },
    onError: () => {
      setError("name", {
        type: "network",
        message: "Something went wrong. Please try again.",
      });
    },
    onSuccess: () => {
      reset();
    },
  });

  const onSubmit: SubmitHandler<FieldValues> = (data) => {
    if (categoriesCount >= 10) return;
    const { name } = data;
    mutate({ name, budgetId: budgetId });
  };

  const formRef = useClickOutside<HTMLFormElement>(() => {
    if (errors) {
      clearErrors();
    }
  });

  return (
    <Form {...form}>
      <form onSubmit={handleSubmit(onSubmit)} ref={formRef}>
        <FormField
          control={control}
          disabled={categoriesCount >= 10}
          name="name"
          render={({ field }) => (
            <FormItem className="w-full">
              <FormControl>
                <div className="flex">
                  <input
                    placeholder="Add category..."
                    {...field}
                    className="h-14 w-full rounded-l-md border-y border-l px-8 outline-none"
                  />
                  <Button
                    type="submit"
                    variant="secondary"
                    className="h-14 rounded-l-none"
                    disabled={categoriesCount >= 10}
                  >
                    <Plus />
                  </Button>
                </div>
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
      </form>
    </Form>
  );
};

export default AddCategoryButton;
