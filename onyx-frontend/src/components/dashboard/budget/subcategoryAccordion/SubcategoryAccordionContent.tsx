import { FC } from "react";

import TargetCard from "@/components/dashboard/budget/subcategoryAccordion/TargetCard";

import { Subcategory } from "@/lib/validation/subcategory";
import SubcategoryDescriptionForm from "./SubcategoryDescriptionForm";
import { Button } from "@/components/ui/button";

interface SubcategoryAccordionContentProps {
  subcategory: Subcategory;
  currencyToDisplay: string;
}

const SubcategoryAccordionContent: FC<SubcategoryAccordionContentProps> = ({
  subcategory,
  currencyToDisplay,
}) => {
  return (
    <div className="grid grid-cols-2 gap-x-4 p-4">
      <TargetCard
        subcategory={subcategory}
        currencyToDisplay={currencyToDisplay}
      />
      <div className="flex h-full flex-col space-y-4">
        <SubcategoryDescriptionForm subcategory={subcategory} />
        <div className="flex w-full justify-end space-x-2">
          <Button className="w-full" variant="outline">
            Edit name
          </Button>
          <Button className="w-full" variant="outline">
            Delete
          </Button>
        </div>
      </div>
    </div>
  );
};

export default SubcategoryAccordionContent;
