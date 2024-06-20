import { FC } from "react";

import TargetCard from "@/components/dashboard/budget/subcategoryAccordion/TargetCard";
import DeleteSubcategoryButton from "@/components/dashboard/budget/subcategoryAccordion/DeleteSubcategoryButton";
import SubcategoryDescriptionForm from "@/components/dashboard/budget/subcategoryAccordion/SubcategoryDescriptionForm";

import { Subcategory } from "@/lib/validation/subcategory";
import { Button } from "@/components/ui/button";

interface SubcategoryAccordionContentProps {
  subcategory: Subcategory;
  currencyToDisplay: string;
  setIsNameEditActive: (state: boolean) => void;
}

const SubcategoryAccordionContent: FC<SubcategoryAccordionContentProps> = ({
  subcategory,
  currencyToDisplay,
  setIsNameEditActive,
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
          <Button
            onClick={() => setIsNameEditActive(true)}
            className="w-full"
            variant="outline"
          >
            Edit name
          </Button>
          <DeleteSubcategoryButton subcategoryId={subcategory.id} />
        </div>
      </div>
    </div>
  );
};

export default SubcategoryAccordionContent;
