import { FC, useEffect } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { useMutation, useQueryClient } from "@tanstack/react-query";

import { Form, FormControl, FormField, FormItem } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { useToast } from "@/components/ui/use-toast";

import {
  EditAccountNameSchema,
  TEditAccountSchema,
} from "@/lib/validation/account";
import { useClickOutside } from "@/lib/hooks/useClickOutside";
import { editAccountName, getAccountsQueryOptions } from "@/lib/api/account";
import { useParams } from "@tanstack/react-router";

interface CarouselCardNameFormProps {
  defaultName: string;
  accountId: string;
}

const CarouselCardNameForm: FC<CarouselCardNameFormProps> = ({
  defaultName,
  accountId,
}) => {
  const queryClient = useQueryClient();
  const { budgetId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId/accounts",
  });
  const form = useForm<TEditAccountSchema>({
    defaultValues: {
      name: defaultName,
    },
  });
  const { toast } = useToast();

  const {
    handleSubmit,
    control,
    reset,
    formState: { isDirty },
    setFocus,
  } = form;

  const { mutate, isError } = useMutation({
    mutationFn: editAccountName,
    onSettled: async () => {
      return await queryClient.invalidateQueries(
        getAccountsQueryOptions(budgetId),
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

  const onSubmit: SubmitHandler<TEditAccountSchema> = (data) => {
    const validatedData = EditAccountNameSchema.safeParse(data);

    if (validatedData.error) {
      toast({
        variant: "destructive",
        title: "Error",
        description: "Invalid subcategory name.",
      });

      setTimeout(() => setFocus("name"), 0);
      return;
    }

    const { name: newName } = validatedData.data;

    if (newName === defaultName) return;

    mutate({ accountId, budgetId, newName });
  };

  useEffect(() => {
    if (isError) {
      reset({
        name: defaultName,
      });
    }
  }, [isError, reset]);

  const formRef = useClickOutside<HTMLFormElement>(() => {
    if (isDirty) {
      handleSubmit(onSubmit)();
    }
  });

  return (
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
                  className="border-none bg-transparent text-lg focus-visible:ring-0 focus-visible:ring-primary-foreground focus-visible:ring-offset-1 md:text-2xl"
                />
              </FormControl>
            </FormItem>
          )}
        />
      </form>
    </Form>
  );
};

export default CarouselCardNameForm;
