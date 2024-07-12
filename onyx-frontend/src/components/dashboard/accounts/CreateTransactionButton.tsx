import { FC, useCallback, useEffect, useMemo } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useParams, useSearch } from "@tanstack/react-router";

import { CalendarIcon, Plus } from "lucide-react";
import AmountInput from "@/components/dashboard/AmountInput";
import PlusMinusButton from "@/components/dashboard/PlusMinusButton";
import { Button } from "@/components/ui/button";
import { Calendar } from "@/components/ui/calendar";
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";

import { cn, removeSpacesFromAmount } from "@/lib/utils";
import { format } from "date-fns";
import {
  CreateTransactionSchema,
  TCreateTransactionSchema,
} from "@/lib/validation/transaction";
import { Account } from "@/lib/validation/account";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import {
  CreateTransactionPayload,
  createTransaction,
  getTransactionsQueryOptions,
} from "@/lib/api/transaction";
import { getAccountsQueryOptions } from "@/lib/api/account";
import { getCategoriesQueryOptions } from "@/lib/api/category";
import { CURRENCY } from "@/lib/constants/currency";
import LoadingButton from "@/components/LoadingButton";

interface Selectable {
  label: string;
  value: string;
}

interface SelectableCategories extends Selectable {
  subcategories: Selectable[];
}

interface CreateTransactionButtonProps {
  account: Account;
  selectableCategories: SelectableCategories[];
}

const CreateTransactionButton: FC<CreateTransactionButtonProps> = ({
  account,
  selectableCategories,
}) => {
  const { accMonth, accYear } = useSearch({
    from: "/_dashboard-layout/budget/$budgetId/accounts/$accountId",
  });
  const { budgetId, accountId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId/accounts/$accountId",
  });
  const queryClient = useQueryClient();
  const {
    balance: { currency: accountCurrency },
  } = account;
  const isCurrentMonthSelected =
    Number(accMonth) === new Date().getMonth() + 1 &&
    Number(accYear) === new Date().getFullYear();
  const dafaultTransactedAt = useMemo(
    () =>
      isCurrentMonthSelected
        ? new Date()
        : new Date(`${accYear}-${accMonth}-01`),
    [accMonth, accYear, isCurrentMonthSelected],
  );

  const form = useForm<TCreateTransactionSchema>({
    defaultValues: {
      currency: accountCurrency,
      amount: "0.00",
      counterpartyName: "",
      subcategoryId: "",
      transactedAt: dafaultTransactedAt,
      categoryId: "",
      transactionSign: "-",
    },
    resolver: zodResolver(CreateTransactionSchema),
  });
  const { control, setFocus, handleSubmit, watch, reset, setValue } = form;
  const selectedCategoryId = watch("categoryId");
  const selectedCurrency = watch("currency");
  const transactionSign = watch("transactionSign");

  useEffect(() => {
    reset((defaultValues) => ({
      ...defaultValues,
      transactedAt: dafaultTransactedAt,
    }));
  }, [dafaultTransactedAt, reset]);

  const selectableSubcategories = useMemo(
    () =>
      selectedCategoryId
        ? selectableCategories.find((c) => c.value === selectedCategoryId)
            ?.subcategories
        : [],
    [selectedCategoryId, selectableCategories],
  );

  const { mutate, isPending, isSuccess } = useMutation({
    mutationFn: createTransaction,
    onError: (err) => {
      console.error(err);
    },
    onSuccess: async () => {
      await Promise.all([
        queryClient.invalidateQueries(
          getTransactionsQueryOptions(budgetId, accountId, {
            accountId,
          }),
        ),
        queryClient.invalidateQueries(getAccountsQueryOptions(budgetId)),
        queryClient.invalidateQueries(getCategoriesQueryOptions(budgetId)),
      ]);
    },
  });

  useEffect(() => {
    if (isSuccess) reset();
  }, [isSuccess, reset]);

  const onSubmit: SubmitHandler<TCreateTransactionSchema> = (data) => {
    const {
      amount,
      counterpartyName,
      subcategoryId,
      transactedAt,
      currency,
      transactionSign,
    } = data;
    const formattedAmount =
      transactionSign === "-"
        ? Number(transactionSign + removeSpacesFromAmount(amount))
        : Number(removeSpacesFromAmount(amount));

    const payload: CreateTransactionPayload = {
      accountId: account.id,
      amount: {
        amount: formattedAmount,
        currency,
      },
      counterpartyName,
      subcategoryId: subcategoryId === "" ? null : subcategoryId,
      transactedAt,
    };

    mutate({ budgetId, payload });
  };

  const handlePlusMinusBtn = useCallback(
    (state: "+" | "-") => {
      setValue("transactionSign", state);
    },
    [setValue],
  );

  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button variant="outline" className="space-x-2">
          <Plus className="inline-flex size-5 flex-shrink-0" />
          <span className="inline-flex">Create</span>
        </Button>
      </DialogTrigger>
      <DialogContent className="h-full w-full overflow-y-auto px-4 md:h-auto md:max-w-[400px] md:overflow-y-hidden">
        <DialogHeader>
          <DialogTitle>New Transaction</DialogTitle>
          <Form {...form}>
            <form onSubmit={handleSubmit(onSubmit)} className="pt-10">
              <div className="space-y-4 px-1.5">
                <FormField
                  control={control}
                  name="counterpartyName"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Counterparty name:</FormLabel>
                      <FormControl>
                        <Input {...field} />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
                <FormField
                  control={form.control}
                  name="currency"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Currency:</FormLabel>
                      <Select
                        onValueChange={field.onChange}
                        value={field.value}
                      >
                        <FormControl>
                          <SelectTrigger>
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
                      <FormMessage />
                    </FormItem>
                  )}
                />
                <FormField
                  control={control}
                  name="amount"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel onClick={() => setFocus("amount")}>
                        Amount:
                      </FormLabel>
                      <FormControl>
                        <div className="flex items-center space-x-2">
                          <AmountInput
                            field={field}
                            className="relative pl-11"
                          />
                          <span
                            className="absolute left-7 text-sm"
                            onClick={() => setFocus("amount")}
                          >
                            {selectedCurrency || accountCurrency}
                          </span>
                          <PlusMinusButton
                            state={transactionSign}
                            setState={handlePlusMinusBtn}
                          />
                        </div>
                      </FormControl>
                      <FormMessage />
                      <FormDescription>
                        To set whether the amount is an income or an expense,
                        select the button.
                      </FormDescription>
                    </FormItem>
                  )}
                />
                <FormField
                  control={form.control}
                  name="transactedAt"
                  render={({ field }) => (
                    <FormItem className="flex flex-col pt-2">
                      <FormLabel>Transacted at:</FormLabel>
                      <Popover>
                        <PopoverTrigger asChild>
                          <FormControl>
                            <Button
                              variant="outline"
                              className={cn(
                                "pl-3 text-left font-normal",
                                !field.value && "text-muted-foreground",
                              )}
                            >
                              {field.value ? (
                                format(field.value, "PPP")
                              ) : (
                                <span>Pick a date</span>
                              )}
                              <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                            </Button>
                          </FormControl>
                        </PopoverTrigger>
                        <PopoverContent className="w-auto p-0" align="start">
                          <Calendar
                            mode="single"
                            selected={field.value}
                            onSelect={field.onChange}
                            disabled={(date) =>
                              isCurrentMonthSelected &&
                              date.getDate() >= new Date().getDate()
                            }
                            initialFocus
                            disableNavigation
                          />
                        </PopoverContent>
                      </Popover>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </div>
              <div
                className={cn(
                  "grid grid-rows-[1fr] transition-all duration-300",
                  transactionSign === "+" && "grid-rows-[0fr]",
                )}
              >
                <div className="mt-4 space-y-4 overflow-hidden px-1.5 pb-1.5">
                  <FormField
                    control={form.control}
                    name="categoryId"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel>Category:</FormLabel>
                        <Select
                          onValueChange={field.onChange}
                          value={field.value}
                        >
                          <FormControl>
                            <SelectTrigger>
                              <SelectValue placeholder="Select transaction category..." />
                            </SelectTrigger>
                          </FormControl>
                          <SelectContent>
                            {selectableCategories.map((c) => (
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
                    control={form.control}
                    name="subcategoryId"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel>Subcategory:</FormLabel>
                        <Select
                          onValueChange={field.onChange}
                          value={field.value}
                          disabled={!selectedCategoryId}
                        >
                          <FormControl>
                            <SelectTrigger>
                              <SelectValue placeholder="Select transaction subcategory..." />
                            </SelectTrigger>
                          </FormControl>
                          <SelectContent>
                            {selectableSubcategories &&
                              selectableSubcategories.map((s) => (
                                <SelectItem key={s.value} value={s.value}>
                                  {s.label}
                                </SelectItem>
                              ))}
                          </SelectContent>
                        </Select>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                </div>
              </div>
              <div className="px-1.5 pt-6">
                <LoadingButton
                  isLoading={isPending}
                  className="w-full"
                  type="submit"
                >
                  Create
                </LoadingButton>
              </div>
            </form>
          </Form>
        </DialogHeader>
      </DialogContent>
    </Dialog>
  );
};

export default CreateTransactionButton;
