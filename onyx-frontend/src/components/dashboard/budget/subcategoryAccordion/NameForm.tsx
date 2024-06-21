import { FC, useEffect } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useParams } from "@tanstack/react-router";

import { Form, FormControl, FormField, FormItem } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { useToast } from "@/components/ui/use-toast";

import {
  CreateSubcategory,
  CreateSubcategorySchema,
  Subcategory,
} from "@/lib/validation/subcategory";
import { editSubcategoryName } from "@/lib/api/subcategory";
import { getCategoriesQueryOptions } from "@/lib/api/category";
import { useClickOutside } from "@/lib/hooks/useClickOutside";

interface NameFormProps {
  subcategory: Subcategory;
  setIsNameEditActive: (state: boolean) => void;
  isNameEditActive: boolean;
}

const NameForm: FC<NameFormProps> = ({
  subcategory,
  isNameEditActive,
  setIsNameEditActive,
}) => {
  const queryClient = useQueryClient();
  const { budgetId } = useParams({
    from: "/_dashboard-layout/_budget-only-layout/budget/$budgetId",
  });
  const { toast } = useToast();
  const form = useForm<CreateSubcategory>({
    defaultValues: {
      name: subcategory.name,
    },
  });

  const {
    handleSubmit,
    control,
    reset,
    formState: { isDirty },
    setFocus,
  } = form;

  const { mutate, isError, isPending, variables } = useMutation({
    mutationFn: editSubcategoryName,
    onSettled: async () => {
      return await queryClient.invalidateQueries(
        getCategoriesQueryOptions(budgetId),
      );
    },
    onError: (error) => {
      console.error("Mutation error:", error);
      toast({
        variant: "destructive",
        title: "Error",
        description: "Oops... Something went wrong. Please try again later.",
      });
      setTimeout(() => setFocus("name"), 0);
    },
  });

  const onSubmit: SubmitHandler<CreateSubcategory> = (data) => {
    const validatedData = CreateSubcategorySchema.safeParse(data);

    if (validatedData.error) {
      toast({
        variant: "destructive",
        title: "Error",
        description: "Invalid subcategory name.",
      });

      setTimeout(() => setFocus("name"), 0);
      return;
    }

    const { name: subcategoryName } = validatedData.data;

    if (subcategoryName === subcategory.name) return;

    mutate({ budgetId, subcategoryId: subcategory.id, subcategoryName });
  };

  useEffect(() => {
    if (isNameEditActive) {
      setFocus("name");
    }
  }, [isNameEditActive, setFocus]);

  useEffect(() => {
    if (isError) {
      reset({
        name: subcategory.name,
      });
    }
  }, [isError, reset]);

  const formRef = useClickOutside<HTMLFormElement>(() => {
    if (isDirty) {
      handleSubmit(onSubmit)();
    }
    setIsNameEditActive(false);
  });

  return isNameEditActive ? (
    <Form {...form}>
      <form onSubmit={handleSubmit(onSubmit)} ref={formRef}>
        <FormField
          control={control}
          name="name"
          render={({ field }) => (
            <FormItem>
              <FormControl>
                <Input
                  {...field}
                  autoComplete="off"
                  className="h-8 cursor-pointer border-none bg-transparent text-base"
                />
              </FormControl>
            </FormItem>
          )}
        />
      </form>
    </Form>
  ) : (
    <p className="px-3">
      {isPending ? variables?.subcategoryName : subcategory.name}
    </p>
  );
};

export default NameForm;
