import { FC, MouseEvent, useRef, useState } from "react";
import { useQueryClient } from "@tanstack/react-query";
import { useParams, useSearch } from "@tanstack/react-router";

import { ChevronRight } from "lucide-react";
import SubcategoryAccordionContent from "@/components/dashboard/budget/subcategoryAccordion/SubcategoryAccordionContent";
import AssignmentForm from "@/components/dashboard/budget/subcategoryAccordion/AssignmentForm";
import NameForm from "@/components/dashboard/budget/subcategoryAccordion/NameForm";

import { Subcategory } from "@/lib/validation/subcategory";
import { cn, getFormattedCurrency } from "@/lib/utils";
import { Money } from "@/lib/validation/base";
import { getToAssignQueryKey } from "@/lib/api/budget";

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
  const queryClient = useQueryClient();
  const { budgetId: selectedBudget } = useParams({
    from: "/_dashboard-layout/budget/$budgetId/",
  });
  const { month, year } = useSearch({
    from: "/_dashboard-layout/budget/$budgetId/",
  });
  const budgetToAssign = queryClient.getQueryData<Money>(
    getToAssignQueryKey(selectedBudget),
  );

  const [isNameEditActive, setIsNameEditActive] = useState(false);

  const assignFormRef = useRef<HTMLDivElement>(null);
  const isActive = activeSubcategory === subcategory.id;

  const onExpandClick = (
    e: MouseEvent<HTMLDivElement, globalThis.MouseEvent>,
  ) => {
    const isAssignFormClicked = assignFormRef.current?.contains(
      e.target as Node,
    );
    if (isAssignFormClicked || isNameEditActive) return;
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
    <li
      className={cn(
        isActive && "border-b last-of-type:border-none",
        subcategory.optimistic && "opacity-50",
      )}
    >
      <div
        onClick={(e) => onExpandClick(e)}
        className={cn(
          "grid cursor-pointer grid-cols-3 items-center space-x-4 p-3 transition-all duration-200 hover:bg-accent",
          isActive && "border-b",
        )}
      >
        <div className="col-span-1 flex items-center md:space-x-6">
          <ChevronRight
            className={cn(
              "flex-shrink-0 rotate-0 opacity-60 transition-all duration-300 ease-in-out",
              isActive && "rotate-90",
            )}
          />
          <NameForm
            subcategory={subcategory}
            isNameEditActive={isNameEditActive}
            setIsNameEditActive={setIsNameEditActive}
          />
        </div>
        <div className="col-span-2 grid grid-cols-2 items-center justify-items-end gap-x-4">
          <p>
            {getFormattedCurrency(
              currentlyAssigned?.actualAmount.amount || 0,
              currencyToDisplay,
            )}
          </p>
          <div className="md:ml-10" ref={assignFormRef}>
            {currencyToDisplay && (
              <AssignmentForm
                defaultAmount={currentlyAssigned?.assignedAmount.amount}
                subcategoryId={subcategory.id}
                currencyToDisplay={currencyToDisplay}
              />
            )}
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
            setIsNameEditActive={setIsNameEditActive}
          />
        </div>
      </div>
    </li>
  );
};

export default SubcategoryAccordion;
