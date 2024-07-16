import { useState } from "react";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import { FormControl } from "@/components/ui/form";
import { Button } from "@/components/ui/button";
import {
  ControllerRenderProps,
  FieldValues,
  Path,
  PathValue,
  UseFormSetValue,
} from "react-hook-form";
import { Check, ChevronsUpDown } from "lucide-react";
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
} from "@/components/ui/command";
import { cn } from "@/lib/utils";

interface Selectable {
  label: string;
  value: string;
}

export interface SelectableCategories extends Selectable {
  subcategories: Selectable[];
}

interface SubcategoriesPopoverFormFieldProps<
  TFieldValues extends FieldValues,
  TName extends Path<TFieldValues>,
> {
  field: ControllerRenderProps<TFieldValues, TName>;
  selectableCategories: SelectableCategories[];
  selectedSubcategoryName: string | undefined;
  setValue: UseFormSetValue<TFieldValues>;
  disabled?: boolean;
}

const SubcategoriesPopoverFormField = <
  TFieldValues extends FieldValues,
  TName extends Path<TFieldValues>,
>({
  field,
  selectableCategories,
  selectedSubcategoryName,
  setValue,
  disabled,
}: SubcategoriesPopoverFormFieldProps<TFieldValues, TName>) => {
  const [open, setOpen] = useState(false);
  const handleSelect = (value: string, label: string) => {
    setValue(field.name, value as PathValue<TFieldValues, TName>);
    setValue(
      "subcategoryName" as Path<TFieldValues>,
      label as PathValue<TFieldValues, Path<TFieldValues>>,
      { shouldValidate: true },
    );
    setOpen(false);
  };

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <FormControl>
          <Button
            variant="outline"
            role="combobox"
            className="w-full justify-between"
            disabled={disabled}
          >
            {selectedSubcategoryName || "Select subcategory..."}
            <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
          </Button>
        </FormControl>
      </PopoverTrigger>
      <PopoverContent className="w-full min-w-[250px] p-0">
        <Command>
          <CommandInput placeholder="Search subcategory..." />
          <CommandList className="scrollbar-thin">
            <CommandEmpty>No subcategory found.</CommandEmpty>
            {selectableCategories.map((category) => (
              <CommandGroup key={category.value} heading={category.label}>
                {category.subcategories.map((subcategory) => (
                  <CommandItem
                    value={subcategory.label}
                    key={subcategory.value}
                    onSelect={() =>
                      handleSelect(subcategory.value, subcategory.label)
                    }
                  >
                    <Check
                      className={cn(
                        "mr-2 h-4 w-4",
                        subcategory.value === field.value
                          ? "opacity-100"
                          : "opacity-0",
                      )}
                    />
                    {subcategory.label}
                  </CommandItem>
                ))}
              </CommandGroup>
            ))}
          </CommandList>
        </Command>
      </PopoverContent>
    </Popover>
  );
};

export default SubcategoriesPopoverFormField;
