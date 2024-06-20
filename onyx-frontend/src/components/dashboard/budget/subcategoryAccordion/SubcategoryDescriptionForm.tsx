import { FC } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { useParams } from "@tanstack/react-router";
import { useMutation, useQueryClient } from "@tanstack/react-query";

import { Form, FormControl, FormField, FormItem } from "@/components/ui/form";
import { Textarea } from "@/components/ui/textarea";
import { useToast } from "@/components/ui/use-toast";

import {
  Subcategory,
  CreateDescriptionSchema,
  CreateDescription,
} from "@/lib/validation/subcategory";
import { useClickOutside } from "@/lib/hooks/useClickOutside";
import { createSubcategoryDescription } from "@/lib/api/subcategory";
import { getCategoriesQueryOptions } from "@/lib/api/category";

interface SubcategoryDescriptionFormProps {
  subcategory: Subcategory;
}

const SubcategoryDescriptionForm: FC<SubcategoryDescriptionFormProps> = ({
  subcategory,
}) => {
  const queryClient = useQueryClient();
  const { budgetId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId",
  });
  const { toast } = useToast();
  const form = useForm<CreateDescription>({
    defaultValues: {
      description: subcategory.description || "",
    },
  });

  const {
    handleSubmit,
    control,
    formState: { isDirty },
    setFocus,
  } = form;

  const { mutate } = useMutation({
    mutationFn: createSubcategoryDescription,
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
      setTimeout(() => setFocus("description"), 0);
    },
  });

  const onSubmit: SubmitHandler<CreateDescription> = (data) => {
    const validatedData = CreateDescriptionSchema.safeParse(data);
    if (validatedData.error) {
      toast({
        variant: "destructive",
        title: "Error",
        description: "Maximum length of description is 255 characters.",
      });
      setTimeout(() => setFocus("description"), 0);
      return;
    }

    const { description } = data;
    if (description === subcategory.description) return;

    mutate({
      budgetId,
      subcategoryId: subcategory.id,
      newDescription: description,
    });
  };

  const formRef = useClickOutside<HTMLFormElement>(() => {
    if (isDirty) {
      handleSubmit(onSubmit)();
    }
  });

  return (
    <Form {...form}>
      <form
        onSubmit={handleSubmit(onSubmit)}
        ref={formRef}
        className="flex h-full w-full flex-col"
      >
        <FormField
          control={control}
          name="description"
          render={({ field }) => (
            <FormItem className="flex-grow">
              <FormControl>
                <Textarea
                  {...field}
                  placeholder="Description..."
                  className="h-full resize-none bg-card shadow-sm scrollbar-thin"
                />
              </FormControl>
            </FormItem>
          )}
        />
      </form>
    </Form>
  );
};

export default SubcategoryDescriptionForm;
