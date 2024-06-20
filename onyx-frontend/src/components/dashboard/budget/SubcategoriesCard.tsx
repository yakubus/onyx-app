import { FC, useState } from "react";

import CreateSubcategoryButton from "@/components/dashboard/budget/CreateSubcategoryButton";
import SubcategoryAccordion from "@/components/dashboard/budget/subcategoryAccordion/SubcategoryAccordion";
import { Card } from "@/components/ui/card";

import { Category } from "@/lib/validation/category";

interface SubcategoriesCardProps {
  activeCategory: Category;
}

const SubcategoriesCard: FC<SubcategoriesCardProps> = ({ activeCategory }) => {
  const [activeSubcategory, setActiveSubcategory] = useState<string | null>(
    null,
  );

  return (
    <Card className="lg:col-span-3">
      <div className="grid grid-cols-3 gap-x-4 rounded-t-md bg-primary px-4 py-1 text-primary-foreground">
        <p className="col-span-1 truncate">Subcategory</p>
        <div className="col-span-2 grid grid-cols-2 justify-items-end">
          <p className="truncate">Actual Amount</p>
          <p>Assigned</p>
        </div>
      </div>
      {activeCategory?.subcategories && (
        <div className="p-1">
          <ul>
            {activeCategory.subcategories.map((subcategory) => (
              <SubcategoryAccordion
                key={subcategory.id}
                subcategory={subcategory}
                activeSubcategory={activeSubcategory}
                setActiveSubcategory={setActiveSubcategory}
              />
            ))}
          </ul>
          <div className="border-t">
            <CreateSubcategoryButton parentCategoryId={activeCategory.id} />
          </div>
        </div>
      )}
    </Card>
  );
};

export default SubcategoriesCard;
