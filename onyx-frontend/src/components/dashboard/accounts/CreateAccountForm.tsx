import { SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useParams, useRouteContext } from "@tanstack/react-router";

import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { useToast } from "@/components/ui/use-toast";

import {
  TCreateAccountForm,
  CreateAccountSchema,
} from "@/lib/validation/account";
import { ACCOUNT_TYPES } from "@/lib/constants/account";
import { CURRENCY } from "@/lib/constants/currency";
import {
  addSpacesToAmount,
  formatDecimals,
  removeSpacesFromAmount,
} from "@/lib/utils";
import { amountLiveValidation } from "@/lib/validation/base";
import { useCreateAccountMutation } from "@/lib/hooks/mutations/useCreateAccountMutation";
import { CreateAccountPayload } from "@/lib/api/account";

const CreateAccountForm = () => {
  const {
    auth: { user },
  } = useRouteContext({ from: "/_dashboard-layout/budget/$budgetId/accounts" });
  const { budgetId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId/accounts",
  });
  const { toast } = useToast();
  const form = useForm<TCreateAccountForm>({
    resolver: zodResolver(CreateAccountSchema),
    defaultValues: {
      name: "",
      currency: user!.currency,
      amount: "0.00",
      accountType: "Checking",
    },
  });

  const { handleSubmit, control, reset } = form;

  const onMutationSuccess = () => {
    reset();
  };

  const onMutationError = () => {
    toast({
      title: "Error",
      variant: "destructive",
      description: "Oops... Something went wrong. Please try again later.",
    });
  };

  const { mutate } = useCreateAccountMutation({
    budgetId,
    onMutationSuccess,
    onMutationError,
  });

  const onSubmit: SubmitHandler<TCreateAccountForm> = (data) => {
    const { accountType, amount, currency, name } = data;
    const amountWithoutCommas = removeSpacesFromAmount(amount);
    const payload: CreateAccountPayload = {
      name,
      accountType,
      balance: { amount: Number(amountWithoutCommas), currency },
    };
    mutate({ budgetId, payload });
  };

  return (
    <Form {...form}>
      <form
        onSubmit={handleSubmit(onSubmit)}
        className="md:grid md:grid-cols-3 md:place-items-center md:gap-x-6"
      >
        <div className="w-full space-y-2 rounded-xl bg-gradient-to-b from-primary via-primary to-primaryDark p-4 text-primary-foreground shadow-lg shadow-primaryDark/50 md:col-span-2 md:w-3/4 xl:w-3/5">
          <FormField
            control={control}
            name="name"
            render={({ field }) => (
              <FormItem>
                <FormControl>
                  <Input
                    {...field}
                    placeholder="Your account name"
                    autoComplete="off"
                    className="bg-transparent text-lg placeholder:text-primary-foreground/90 focus-visible:ring-0 focus-visible:ring-primary-foreground focus-visible:ring-offset-1 md:text-2xl"
                  />
                </FormControl>
                <FormMessage className="w-fit rounded-md bg-primary-foreground px-1" />
              </FormItem>
            )}
          />
          <div className="flex justify-between space-x-4">
            <FormField
              control={control}
              name="amount"
              render={({ field }) => (
                <FormItem className="space-y-0">
                  <FormLabel className="text-xs font-thin">Balance:</FormLabel>
                  <FormControl>
                    <Input
                      type="text"
                      autoComplete="off"
                      {...field}
                      onChange={(e) => {
                        let { value } = e.target;
                        value = amountLiveValidation(value);
                        value = addSpacesToAmount(value);
                        field.onChange(value);
                      }}
                      onBlur={(e) => {
                        const { value } = e.target;
                        const formattedValue = formatDecimals(value);
                        field.onChange(formattedValue);
                      }}
                      className="bg-transparent text-lg focus-visible:ring-0 focus-visible:ring-primary-foreground focus-visible:ring-offset-1"
                    />
                  </FormControl>
                </FormItem>
              )}
            />
            <FormField
              control={control}
              name="currency"
              render={({ field }) => (
                <FormItem className="w-1/3 space-y-0">
                  <FormLabel className="text-xs font-thin">Currency:</FormLabel>
                  <FormControl>
                    <Select
                      onValueChange={field.onChange}
                      defaultValue={field.value}
                    >
                      <FormControl>
                        <SelectTrigger className="bg-transparent text-primary-foreground focus:ring-0 focus:ring-primary-foreground focus:ring-offset-1">
                          <SelectValue />
                        </SelectTrigger>
                      </FormControl>
                      <SelectContent>
                        {CURRENCY.map(({ value, label }) => (
                          <SelectItem key={value} value={value}>
                            {label}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  </FormControl>
                </FormItem>
              )}
            />
          </div>
          <FormField
            control={control}
            name="accountType"
            render={({ field }) => (
              <FormItem className="flex w-full items-center space-x-2 space-y-0">
                <FormLabel className="flex-grow text-end">
                  Account type:
                </FormLabel>
                <Select
                  onValueChange={field.onChange}
                  defaultValue={field.value}
                >
                  <FormControl>
                    <SelectTrigger className="w-1/2 bg-transparent text-primary-foreground focus:ring-0 focus:ring-primary-foreground  focus:ring-offset-1 lg:w-1/3">
                      <SelectValue />
                    </SelectTrigger>
                  </FormControl>
                  <SelectContent>
                    {ACCOUNT_TYPES.map((type) => (
                      <SelectItem key={type} value={type}>
                        {type}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              </FormItem>
            )}
          />
        </div>
        <Button
          type="submit"
          size="lg"
          className="mt-8 w-full tracking-widest shadow-xl shadow-primaryDark/40 md:mt-0 md:w-2/3 md:justify-self-start"
        >
          Create
        </Button>
      </form>
    </Form>
  );
};

export default CreateAccountForm;
