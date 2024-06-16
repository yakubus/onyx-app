import { FC } from "react";

import TargetCard from "@/components/dashboard/budget/subcategoryAccordion/TargetCard";

import { Subcategory } from "@/lib/validation/subcategory";

interface SubcategoryAccordionContentProps {
  subcategory: Subcategory;
  currencyToDisplay: string;
}

const SubcategoryAccordionContent: FC<SubcategoryAccordionContentProps> = ({
  subcategory,
  currencyToDisplay,
}) => {
  return (
    <div className="grid grid-cols-2 p-4">
      <TargetCard
        subcategory={subcategory}
        currencyToDisplay={currencyToDisplay}
      />
    </div>
  );
};

export default SubcategoryAccordionContent;
