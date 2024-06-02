import { FC, useState } from "react";
import { useMutationState } from "@tanstack/react-query";

import { ChevronDown } from "lucide-react";

import { type Category } from "@/lib/validation/category";
import LeftNavigation from "./LeftNavigation";
import MiddleSection from "./MiddleSection";

import { cn } from "@/lib/utils";
import { useClickOutside } from "@/lib/hooks/useClickOutside";

interface SelectCategoryButtonProps {
  activeCategoryId: string;
  category: Category;
  onSelect: () => void;
}

export interface SelectCategorySectionProps {
  category: Category;
  isEdit: boolean;
  setIsEdit: (state: boolean) => void;
  isSelected: boolean;
}

const SelectCategoryButton: FC<SelectCategoryButtonProps> = ({
  activeCategoryId,
  category,
  onSelect,
}) => {
  const [isEdit, setIsEdit] = useState(false);
  const { id } = category;
  const isSelected = activeCategoryId === id;
  const isDeleting = useMutationState({
    filters: { mutationKey: ["deleteCategory", id], status: "pending" },
    select: (mutation) => mutation.state.status,
  });

  const selectRef = useClickOutside<HTMLLIElement>(() => {
    setIsEdit(false);
  });

  return (
    <li
      ref={selectRef}
      className={cn(
        "cursor-pointer rounded-lg border border-input hover:bg-accent hover:text-accent-foreground",
        isSelected &&
          "bg-primary text-primary-foreground hover:bg-primary hover:text-primary-foreground",
        isDeleting.length && "bg-primary/50",
      )}
      onClick={onSelect}
    >
      <div
        className={cn("flex h-16 w-full items-center px-6", isEdit && "px-2")}
      >
        <div className="flex flex-1 items-center truncate">
          <LeftNavigation
            category={category}
            isEdit={isEdit}
            isSelected={isSelected}
            setIsEdit={setIsEdit}
          />
          <MiddleSection
            category={category}
            isEdit={isEdit}
            isSelected={isSelected}
            setIsEdit={setIsEdit}
          />
        </div>
        {isEdit ? null : (
          <ChevronDown
            className={cn(
              "block flex-shrink-0 -rotate-90 text-muted-foreground transition-all duration-200 ease-in-out lg:rotate-0",
              isSelected && "rotate-0 text-primary-foreground lg:-rotate-90",
            )}
          />
        )}
      </div>
    </li>
  );
};

export default SelectCategoryButton;
