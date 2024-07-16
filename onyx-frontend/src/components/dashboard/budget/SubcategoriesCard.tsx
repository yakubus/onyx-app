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
    <Card className="w-full overflow-auto">
      <div className="grid min-w-[400px] grid-cols-3 gap-x-4  rounded-t-md bg-primary px-4 py-1 text-primary-foreground">
        <p className="col-span-1">Subcategory</p>
        <div className="col-span-2 grid grid-cols-2 justify-items-end">
          <p>Actual Amount</p>
          <p>Assigned</p>
        </div>
      </div>
      {activeCategory?.subcategories && (
        <div className="min-w-[400px] p-1">
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
