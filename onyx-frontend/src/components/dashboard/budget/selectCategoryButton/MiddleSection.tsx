import { FC, useEffect, useRef } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQueryClient } from "@tanstack/react-query";

import { Check } from "lucide-react";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormMessage,
} from "@/components/ui/form";
import { Button } from "@/components/ui/button";

import {
  CreateCategorySchema,
  type CreateCategory,
} from "@/lib/validation/category";
import {
  editCategoryName,
  getCategoriesQueryOptions,
} from "@/lib/api/category";
import { useClickOutside } from "@/lib/hooks/useClickOutside";
import { capitalize, cn } from "@/lib/utils";
import { type SelectCategorySectionProps } from "@/components/dashboard/budget/selectCategoryButton/SelectCategoryButton";

const MiddleSection: FC<SelectCategorySectionProps> = ({
  isEdit,
  isSelected,
  category,
  setIsEdit,
}) => {
  const { name, id } = category;
  const form = useForm<CreateCategory>({
    resolver: zodResolver(CreateCategorySchema),
    defaultValues: {
      name: name,
    },
  });
  const {
    handleSubmit,
    control,
    clearErrors,
    formState: { errors },
    setError,
  } = form;
  const inputRef = useRef<HTMLInputElement>(null);
  const queryClient = useQueryClient();

  useEffect(() => {
    if (isEdit && inputRef.current) {
      inputRef.current.focus();
    }
  }, [isEdit]);

  const { mutate, isPending, variables } = useMutation({
    mutationKey: ["editCategory", id],
    mutationFn: editCategoryName,
    onError: () => {
      setError("name", {
        type: "network",
        message: "Error occured. Try again.",
      });
    },
    onSettled: async (_newName, error) => {
      if (!error) {
        setIsEdit(false);
      }
      return await queryClient.invalidateQueries({
        queryKey: getCategoriesQueryOptions.queryKey,
      });
    },
  });

  const onSubmit: SubmitHandler<CreateCategory> = (data) => {
    mutate({
      id,
      newName: data.name,
    });
  };

  const formRef = useClickOutside<HTMLFormElement>(() => {
    if (errors) {
      clearErrors();
    }
  });

  if (isEdit)
    return (
      <Form {...form}>
        <form
          onSubmit={handleSubmit(onSubmit)}
          ref={formRef}
          className="flex w-full items-center"
        >
          <FormField
            control={control}
            name="name"
            render={({ field }) => (
              <FormItem className="w-full space-y-1">
                <FormControl>
                  <input
                    placeholder="Add category..."
                    {...field}
                    ref={inputRef}
                    className="h-8 w-full bg-transparent outline-none"
                  />
                </FormControl>
                <FormMessage className="max-w-fit rounded-md bg-destructive px-2 text-primary-foreground" />
              </FormItem>
            )}
          />
          <Button type="submit">
            <Check />
          </Button>
        </form>
      </Form>
    );

  return (
    <p
      className={cn(
        "w-full translate-x-0 truncate pr-6 transition-transform duration-300 ease-in-out",
        isSelected && "max-w-64 translate-x-6",
        isPending && "opacity-50",
      )}
    >
      {isPending ? capitalize(variables.newName) : name}
    </p>
  );
};

export default MiddleSection;
