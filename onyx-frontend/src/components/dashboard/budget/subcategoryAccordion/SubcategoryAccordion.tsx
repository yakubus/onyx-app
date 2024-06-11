import { FC, useState } from "react";

import { Subcategory } from "@/lib/validation/subcategory";
import { cn } from "@/lib/utils";
import { ChevronRight } from "lucide-react";
import { useClickOutside } from "@/lib/hooks/useClickOutside";
import NameTooltip from "./NameTooltip";
import AssignmentTooltip from "./AssignmentTooltip";

interface SubcategoryAccordionProps {
  subcategory: Subcategory;
  parentCategoryId: string;
}

const SubcategoryAccordion: FC<SubcategoryAccordionProps> = ({
  subcategory,
  parentCategoryId,
}) => {
  const [expanded, setExpanded] = useState(false);
  const [isNameTooltipOpen, setIsNameTooltipOpen] = useState(false);
  const [isAssignmentTooltipOpen, setAssignmentTooltipOpen] = useState(false);

  const accordionRef = useClickOutside<HTMLLIElement>(() => {
    setExpanded(false);
  });

  const onExpandClick = () => {
    if (isNameTooltipOpen || isAssignmentTooltipOpen) return;
    setExpanded(!expanded);
  };

  return (
    <li
      onClick={onExpandClick}
      ref={accordionRef}
      className="cursor-pointer p-3 transition-all duration-200 hover:bg-accent"
    >
      <div className="flex justify-between space-x-4">
        <div className="flex space-x-6">
          <ChevronRight
            className={cn(
              "rotate-0 opacity-60 transition-all duration-300 ease-in-out",
              expanded && "rotate-90",
            )}
          />
          <NameTooltip
            isNameTooltipOpen={isNameTooltipOpen}
            setIsNameTooltipOpen={setIsNameTooltipOpen}
            subcategory={subcategory}
          />
        </div>
        <div>
          <AssignmentTooltip
            isAssignmentTooltipOpen={isAssignmentTooltipOpen}
            setIsAssignmentTooltipOpen={setAssignmentTooltipOpen}
            subcategory={subcategory}
          />
        </div>
      </div>
      <div
        className={cn(
          "grid grid-rows-[0fr] transition-all duration-300 ease-in-out",
          expanded && "grid-rows-[1fr]",
        )}
      >
        <div className="overflow-hidden">content</div>
      </div>
    </li>
  );
};

export default SubcategoryAccordion;
