import { FC } from "react";

import CreateSubcategoryButton from "@/components/dashboard/budget/CreateSubcategoryButton";
import SubcategoryAccordion from "@/components/dashboard/budget/subcategoryAccordion/SubcategoryAccordion";
import { Card } from "@/components/ui/card";

import { Category } from "@/lib/validation/category";

interface SubcategoriesCardProps {
  activeCategory: Category;
}

const SubcategoriesCard: FC<SubcategoriesCardProps> = ({ activeCategory }) => {
  return (
    <Card className="lg:col-span-3">
      <div className="flex justify-between rounded-t-md bg-primary px-4 py-1 text-primary-foreground">
        <p>Subcategory</p>
        <p>Assigned</p>
      </div>
      {activeCategory?.subcategories && (
        <ul className="p-1">
          {activeCategory.subcategories.map((subcategory) => (
            <SubcategoryAccordion
              key={subcategory.id}
              subcategory={subcategory}
              parentCategoryId={activeCategory.id}
            />
          ))}
          <CreateSubcategoryButton parentCategoryId={activeCategory.id} />
        </ul>
      )}
    </Card>
  );
};

export default SubcategoriesCard;
