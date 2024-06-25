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
    <div className="grid gap-x-4 gap-y-4 p-4 md:grid-cols-2 md:gap-y-0">
      <div className="flex h-full flex-col space-y-4">
        <div className="flex w-full justify-end space-x-2">
          <DeleteSubcategoryButton subcategoryId={subcategory.id} />
          <Button
            onClick={() => setIsNameEditActive(true)}
            className="w-full"
            variant="outline"
          >
            Edit name
          </Button>
        </div>
        <SubcategoryDescriptionForm subcategory={subcategory} />
      </div>
      <TargetCard
        subcategory={subcategory}
        currencyToDisplay={currencyToDisplay}
      />
    </div>
  );
};

export default SubcategoryAccordionContent;
