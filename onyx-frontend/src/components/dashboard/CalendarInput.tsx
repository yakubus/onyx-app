import { useState } from "react";
import { ControllerRenderProps, FieldValues, Path } from "react-hook-form";
import { format } from "date-fns";

import { CalendarIcon } from "lucide-react";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import { FormControl } from "@/components/ui/form";
import { Button } from "@/components/ui/button";
import { Calendar } from "@/components/ui/calendar";

import { cn } from "@/lib/utils";

interface CalendarInputProps<
  TFieldValues extends FieldValues,
  TName extends Path<TFieldValues>,
> {
  field: ControllerRenderProps<TFieldValues, TName>;
  disabled: (date: Date) => boolean;
}

const CalendarInput = <
  TFieldValues extends FieldValues,
  TName extends Path<TFieldValues>,
>({
  field,
  disabled,
}: CalendarInputProps<TFieldValues, TName>) => {
  const [open, setOpen] = useState(false);

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <FormControl>
          <Button
            variant="outline"
            className={cn(
              "w-full pl-3 text-left font-normal",
              !field.value && "text-muted-foreground",
            )}
            onClick={() => setOpen(true)}
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
          onSelect={(date) => {
            field.onChange(date);
            setOpen(false);
          }}
          disabled={disabled}
          initialFocus
          disableNavigation
        />
      </PopoverContent>
    </Popover>
  );
};

export default CalendarInput;
