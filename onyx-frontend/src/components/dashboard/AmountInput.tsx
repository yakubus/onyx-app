import { ChangeEvent, FC } from "react";
import { ControllerRenderProps } from "react-hook-form";
import { Input } from "../ui/input";
import { amountLiveValidation } from "@/lib/validation/base";
import { addSpacesToAmount, formatDecimals } from "@/lib/utils";

interface AmountInputProps {
  field: ControllerRenderProps<{ amount: string }, "amount">;
  className?: string;
}

const AmountInput: FC<AmountInputProps> = ({ field, className }) => {
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
    />
  );
};

export default AmountInput;
