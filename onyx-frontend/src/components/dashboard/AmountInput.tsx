import CurrencyInput from "react-currency-input-field";
import { ControllerRenderProps, FieldValues, Path } from "react-hook-form";
import { USER_LOCALE } from "@/lib/constants/locale";
import { cn } from "@/lib/utils";

interface AmountInputProps<
  TFieldValues extends FieldValues,
  TName extends Path<TFieldValues>,
> {
  field: ControllerRenderProps<TFieldValues, TName>;
  currency: string;
  className?: string;
}

const AmountInput = <
  TFieldValues extends FieldValues,
  TName extends Path<TFieldValues>,
>({
  field,
  className,
  currency,
}: AmountInputProps<TFieldValues, TName>) => {
  return (
    <CurrencyInput
      name={field.name}
      defaultValue={field.value}
      value={field.value}
      decimalsLimit={2}
      onValueChange={field.onChange}
      intlConfig={{ locale: USER_LOCALE, currency }}
      decimalScale={2}
      maxLength={9}
      groupSeparator={USER_LOCALE === "pl-PL" ? " " : undefined}
      allowNegativeValue={false}
      className={cn(
        "flex h-10 w-full rounded-md bg-transparent px-3 py-2 text-right ring-offset-background placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50",
        className,
      )}
    />
  );
};

export default AmountInput;
