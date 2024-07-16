import { FC } from "react";

import { Plus } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormMessage,
} from "@/components/ui/form";

import { Account } from "@/lib/validation/account";
import { useCreateTransactionForm } from "@/lib/hooks/useCreateTransactionForm";
import CalendarInput from "../CalendarInput";
import { Input } from "@/components/ui/input";
import SubcategoriesPopoverFormField, {
  SelectableCategories,
} from "./SubcategoriesPopoverFormField";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { CURRENCY } from "@/lib/constants/currency";
import PlusMinusButton from "../PlusMinusButton";
import AmountInput from "../AmountInput";
import { useClickOutside } from "@/lib/hooks/useClickOutside";

interface CreateTransactionTableFormProps {
  account: Account;
  selectableCategories: SelectableCategories[];
}

const CreateTransactionTableForm: FC<CreateTransactionTableFormProps> = ({
  account,
  selectableCategories,
}) => {
  const {
    handlePlusMinusBtn,
    form,
    handleSubmit,
    onSubmit,
    control,
    transactionSign,
    selectedCurrency,
    isCurrentMonthSelected,
    isPending,
    setValue,
    selectedSubcategoryName,
    clearErrors,
  } = useCreateTransactionForm({ account });

  const formRef = useClickOutside<HTMLFormElement>(() => clearErrors());

  return (
    <Form {...form}>
      <form
        onSubmit={handleSubmit(onSubmit)}
        className="grid grid-cols-table-layout gap-x-6 py-1.5"
        ref={formRef}
      >
        <Button size="icon" type="submit" disabled={isPending}>
          <Plus />
        </Button>
        <FormField
          control={form.control}
          name="transactedAt"
          render={({ field }) => (
            <FormItem className="w-full pl-1.5">
              <CalendarInput
                field={field}
                disabled={(date) =>
                  isCurrentMonthSelected &&
                  date.getDate() >= new Date().getDate()
                }
              />
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={control}
          name="counterpartyName"
          render={({ field }) => (
            <FormItem>
              <FormControl>
                <Input
                  {...field}
                  placeholder="Counterparty..."
                  className="placeholder:text-foreground"
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={control}
          name="subcategoryId"
          render={({ field }) => (
            <FormItem>
              <SubcategoriesPopoverFormField
                field={field}
                selectableCategories={selectableCategories}
                selectedSubcategoryName={selectedSubcategoryName}
                setValue={setValue}
                disabled={transactionSign === "+"}
              />
              <FormMessage />
            </FormItem>
          )}
        />
        <div className="flex">
          <FormField
            control={form.control}
            name="currency"
            render={({ field }) => (
              <FormItem className="mr-3">
                <Select onValueChange={field.onChange} value={field.value}>
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
                <FormControl>
                  <div className="flex items-center">
                    <PlusMinusButton
                      state={transactionSign}
                      setState={handlePlusMinusBtn}
                    />
                    <div className="px-1.5">
                      <AmountInput
                        field={field}
                        currency={selectedCurrency || account.balance.currency}
                        className="border text-right"
                      />
                    </div>
                  </div>
                </FormControl>
                <FormMessage className="w-full pr-1.5 text-right" />
              </FormItem>
            )}
          />
        </div>
      </form>
    </Form>
  );
};

export default CreateTransactionTableForm;
