import { FC, useState } from "react";

import CreateSubcategoryButton from "@/components/dashboard/budget/CreateSubcategoryButton";
import SubcategoryAccordion from "@/components/dashboard/budget/subcategoryAccordion/SubcategoryAccordion";

import { Category } from "@/lib/validation/category";
import { ScrollArea } from "@/components/ui/scroll-area";

interface SubcategoriesCardProps {
  activeCategory: Category;
}

const SubcategoriesCard: FC<SubcategoriesCardProps> = ({ activeCategory }) => {
  const [activeSubcategory, setActiveSubcategory] = useState<string | null>(
    null,
  );

  return (
    <ScrollArea className="h-full w-full overflow-auto rounded-lg border bg-card lg:col-span-3">
      <div className="sticky top-0 z-10 grid min-w-[400px] grid-cols-3 gap-x-4  rounded-t-md bg-primary px-4 py-1 text-primary-foreground">
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
    </ScrollArea>
  );
};

export default SubcategoriesCard;
