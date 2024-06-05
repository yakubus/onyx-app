import { FC } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQueryClient } from "@tanstack/react-query";

import { Plus } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormMessage,
} from "@/components/ui/form";

import { CreateBudget } from "@/lib/validation/budget";
import { CreateCategorySchema } from "@/lib/validation/category";
import { createBudget, getBudgetsQueryOptions } from "@/lib/api/budget";
import { useClickOutside } from "@/lib/hooks/useClickOutside";

interface CreateBudgetFormProps {
  setIsCreating: (state: boolean) => void;
}

const CreateBudgetForm: FC<CreateBudgetFormProps> = ({ setIsCreating }) => {
  const form = useForm<CreateBudget>({
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
    mutationKey: ["createBudget"],
    mutationFn: createBudget,
    onSettled: async () => {
      return await queryClient.invalidateQueries({
        queryKey: getBudgetsQueryOptions.queryKey,
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
      setIsCreating(false);
    },
  });

  const onSubmit: SubmitHandler<CreateBudget> = (data) => {
    const { name } = data;
    mutate(name);
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
          name="name"
          render={({ field }) => (
            <FormItem className="w-full">
              <FormControl>
                <div className="flex">
                  <input
                    placeholder="Add budget..."
                    {...field}
                    className="h-14 w-full rounded-l-md border-y border-l px-8 outline-none"
                  />
                  <Button
                    type="submit"
                    variant="secondary"
                    className="h-14 rounded-l-none"
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

export default CreateBudgetForm;
