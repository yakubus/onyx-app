import { FC, MouseEvent, ReactNode, useRef, useState } from "react";
import { useQueryClient } from "@tanstack/react-query";
import { useSearch } from "@tanstack/react-router";

import { ChevronRight } from "lucide-react";
import NameTooltip from "@/components/dashboard/budget/subcategoryAccordion/NameTooltip";
import AssignmentTooltip from "@/components/dashboard/budget/subcategoryAccordion/AssignmentTooltip";
import SubcategoryAccordionContent from "@/components/dashboard/budget/subcategoryAccordion/SubcategoryAccordionContent";

import { Subcategory } from "@/lib/validation/subcategory";
import { cn } from "@/lib/utils";
import { Money } from "@/lib/validation/base";
import { getToAssignQueryKey } from "@/lib/api/budget";
import AssignmentForm from "./AssignmentForm";

interface SubcategoryAccordionProps {
  subcategory: Subcategory;
  setActiveSubcategory: (id: string | null) => void;
  activeSubcategory: string | null;
}

const SubcategoryAccordion: FC<SubcategoryAccordionProps> = ({
  subcategory,
  setActiveSubcategory,
  activeSubcategory,
}) => {
  const [isNameTooltipOpen, setIsNameTooltipOpen] = useState(false);
  const queryClient = useQueryClient();
  const { month, year, selectedBudget } = useSearch({
    from: "/_dashboard-layout/budget/$budgetId",
  });
  const budgetToAssign = queryClient.getQueryData<Money>(
    getToAssignQueryKey(selectedBudget),
  );

  const assignFormRef = useRef<HTMLDivElement>(null);
  const isActive = activeSubcategory === subcategory.id;

  const onExpandClick = (
    e: MouseEvent<HTMLDivElement, globalThis.MouseEvent>,
  ) => {
    const isAssignFormClicked = assignFormRef.current?.contains(
      e.target as Node,
    );
    if (isNameTooltipOpen || isAssignFormClicked) return;
    setActiveSubcategory(isActive ? null : subcategory.id);
  };

  const currentlyAssigned = subcategory.assignments?.find(
    (asignment) =>
      asignment.month.month === Number(month) &&
      asignment.month.year === Number(year),
  );
  const currencyToDisplay =
    currentlyAssigned?.actualAmount.currency || budgetToAssign?.currency || "";

  return (
    <li className={cn(isActive && "border-b")}>
      <div
        onClick={(e) => onExpandClick(e)}
        className={cn(
          "grid cursor-pointer grid-cols-3 items-center space-x-4 p-3 transition-all duration-200 hover:bg-accent",
          isActive && "border-b",
        )}
      >
        <div className="col-span-1 flex space-x-6">
          <ChevronRight
            className={cn(
              "rotate-0 opacity-60 transition-all duration-300 ease-in-out",
              isActive && "rotate-90",
            )}
          />
          <NameTooltip
            isNameTooltipOpen={isNameTooltipOpen}
            setIsNameTooltipOpen={setIsNameTooltipOpen}
            subcategory={subcategory}
          />
        </div>
        <div className="col-span-2 grid grid-cols-2 items-center justify-items-end gap-x-4">
          <p>
            {currentlyAssigned?.actualAmount.amount || "0"} {currencyToDisplay}
          </p>
          <div className="flex items-center space-x-2 pl-4" ref={assignFormRef}>
            <AssignmentForm
              defaultAmount={currentlyAssigned?.assignedAmount.amount.toString()}
              subcategoryId={subcategory.id}
            />
            <p>{currencyToDisplay}</p>
          </div>
        </div>
      </div>
      <div
        className={cn(
          "grid grid-rows-[0fr] transition-all duration-300 ease-in-out",
          isActive && "grid-rows-[1fr]",
        )}
      >
        <div className="overflow-hidden">
          <SubcategoryAccordionContent
            subcategory={subcategory}
            currencyToDisplay={currencyToDisplay}
          />
        </div>
      </div>
    </li>
  );
};

export default SubcategoryAccordion;
