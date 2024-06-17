import { FC } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";

import { Button } from "@/components/ui/button";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormMessage,
} from "@/components/ui/form";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";

import { CreateBudget, CreateBudgetSchema } from "@/lib/validation/budget";
import { useClickOutside } from "@/lib/hooks/useClickOutside";
import { CURRENCY } from "@/lib/constants/currency";
import { Input } from "@/components/ui/input";
import { User } from "@/lib/validation/user";
import { useCreateBudgetMutation } from "@/lib/hooks/mutations/useCreateBudgetMutation";

interface CreateBudgetFormProps {
  setIsCreating: (state: boolean) => void;
  user: User;
}

const CreateBudgetForm: FC<CreateBudgetFormProps> = ({
  setIsCreating,
  user,
}) => {
  const form = useForm<CreateBudget>({
    resolver: zodResolver(CreateBudgetSchema),
    defaultValues: {
      name: "",
      currency: user.currency,
      userId: user.id,
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

  const onMutationError = () => {
    setError("name", {
      type: "network",
      message: "Something went wrong. Please try again.",
    });
  };

  const onMutationSuccess = () => {
    reset();
    setIsCreating(false);
  };

  const { mutate } = useCreateBudgetMutation({
    onMutationError,
    onMutationSuccess,
  });

  const onSubmit: SubmitHandler<CreateBudget> = (data) => {
    console.log(data);
    const { name: budgetName, currency: budgetCurrency, userId } = data;
    mutate({ budgetName, budgetCurrency, userId });
  };

  const formRef = useClickOutside<HTMLFormElement>(() => {
    if (errors) {
      clearErrors();
    }
  });

  return (
    <Form {...form}>
      <form
        onSubmit={handleSubmit(onSubmit)}
        ref={formRef}
        className="grid w-full grid-cols-9 gap-x-4 px-4 py-6"
      >
        <FormField
          control={control}
          name="name"
          render={({ field }) => (
            <FormItem className="col-span-3">
              <FormControl>
                <Input
                  placeholder="Name..."
                  {...field}
                  className="h-14 w-full"
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={control}
          name="currency"
          render={({ field }) => (
            <FormItem className="col-span-2">
              <Select
                onValueChange={field.onChange}
                defaultValue={field.value}
                disabled
              >
                <FormControl>
                  <SelectTrigger className="h-14">
                    <SelectValue />
                  </SelectTrigger>
                </FormControl>
                <SelectContent>
                  {CURRENCY.map((c) => (
                    <SelectItem key={c.value} value={c.value}>
                      {c.label}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={control}
          name="userId"
          render={({ field }) => (
            <FormItem className="col-span-3">
              <FormControl>
                <Input {...field} className="h-14 w-full" disabled />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <Button
          type="submit"
          variant="outline"
          className="col-span-1 h-14 rounded-l-none font-semibold"
        >
          Create
        </Button>
      </form>
    </Form>
  );
};

export default CreateBudgetForm;
