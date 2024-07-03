import { ChangeEvent } from "react";
import { ControllerRenderProps, FieldValues, Path } from "react-hook-form";
import { Input } from "../ui/input";
import { amountLiveValidation } from "@/lib/validation/base";
import { addSpacesToAmount, formatDecimals } from "@/lib/utils";

interface AmountInputProps<
  TFieldValues extends FieldValues,
  TName extends Path<TFieldValues>,
> {
  field: ControllerRenderProps<TFieldValues, TName>;
  className?: string;
}

const AmountInput = <
  TFieldValues extends FieldValues,
  TName extends Path<TFieldValues>,
>({
  field,
  className,
}: AmountInputProps<TFieldValues, TName>) => {
  return (
    <Input
      type="text"
      autoComplete="off"
      {...field}
      onChange={(e: ChangeEvent<HTMLInputElement>) => {
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
      className={className}
      name={field.name as string}
    />
  );
};

export default AmountInput;
