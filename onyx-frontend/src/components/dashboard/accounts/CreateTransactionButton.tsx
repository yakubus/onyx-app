import { FC, useState } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useParams, useSearch } from "@tanstack/react-router";

import { CalendarIcon } from "lucide-react";
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

import { cn } from "@/lib/utils";
import { format } from "date-fns";
import {
  CreateTransactionSchema,
  TCreateTransactionSchema,
} from "@/lib/validation/transaction";
import { Account } from "@/lib/validation/account";
import { useMutation } from "@tanstack/react-query";
import {
  CreateTransactionPayload,
  createTransaction,
} from "@/lib/api/transaction";

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
  const [transactionSign, setTransactionSign] = useState<"+" | "-">("+");
  const form = useForm<TCreateTransactionSchema>({
    defaultValues: {
      amount: "0.00",
      counterpartyName: "",
      subcategoryId: "",
      transactedAt: new Date(),
      categoryId: "",
    },
    resolver: zodResolver(CreateTransactionSchema),
  });
  const { accMonth, accYear } = useSearch({
    from: "/_dashboard-layout/budget/$budgetId/accounts/$accountId",
  });
  const { budgetId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId/accounts/$accountId",
  });

  const { control, setFocus, handleSubmit, watch, reset } = form;
  const selectedCategoryId = watch("categoryId");
  const selectableSubcategories =
    selectedCategoryId &&
    selectableCategories.find((c) => c.value === selectedCategoryId)!
      .subcategories;
  const {
    balance: { currency },
  } = account;

  const { mutate } = useMutation({
    mutationFn: createTransaction,
    onError: (err) => {
      console.error(err);
    },
    onSuccess: () => {
      reset();
    },
  });

  const onSubmit: SubmitHandler<TCreateTransactionSchema> = (data) => {
    const { amount, counterpartyName, subcategoryId, transactedAt } = data;
    const formattedAmount =
      transactionSign === "-"
        ? Number(transactionSign + amount)
        : Number(amount);

    const payload: CreateTransactionPayload = {
      accountId: account.id,
      amount: {
        amount: formattedAmount,
        currency,
      },
      counterpartyName,
      subcategoryId,
      transactedAt,
    };

    mutate({ budgetId, payload });
  };

  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button variant="outline" size="sm">
          Add Transaction
        </Button>
      </DialogTrigger>
      <DialogContent className="max-w-[400px]">
        <DialogHeader>
          <DialogTitle>New Transaction</DialogTitle>
          <Form {...form}>
            <form onSubmit={handleSubmit(onSubmit)} className="space-y-4 pt-10">
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
                control={control}
                name="amount"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel onClick={() => setFocus("amount")}>
                      Amount:
                    </FormLabel>
                    <FormControl>
                      <div className="flex items-center space-x-2">
                        <AmountInput field={field} className="relative pl-11" />
                        <span
                          className="absolute left-7 text-sm"
                          onClick={() => setFocus("amount")}
                        >
                          {currency}
                        </span>
                        <PlusMinusButton
                          state={transactionSign}
                          setState={setTransactionSign}
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
                            date.getMonth() !== Number(accMonth) - 1 ||
                            date.getFullYear() !== Number(accYear)
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
              <FormField
                control={form.control}
                name="categoryId"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Category:</FormLabel>
                    <Select
                      onValueChange={field.onChange}
                      defaultValue={field.value}
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
                      defaultValue={field.value}
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
              <div className="pt-6">
                <Button className="w-full" type="submit">
                  Create
                </Button>
              </div>
            </form>
          </Form>
        </DialogHeader>
      </DialogContent>
    </Dialog>
  );
};

export default CreateTransactionButton;
