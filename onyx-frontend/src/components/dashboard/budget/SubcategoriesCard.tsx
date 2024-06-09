import { FC } from "react";

import { ChevronRight } from "lucide-react";
import { Card } from "@/components/ui/card";
import CreateSubcategoryButton from "./CreateSubcategoryButton";
import { Category } from "@/lib/validation/category";
import { cn } from "@/lib/utils";

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
            <li
              key={subcategory.id}
              className={cn(
                "flex cursor-pointer items-center justify-between py-3 pl-2 pr-4 transition-all duration-200 hover:bg-accent hover:text-accent-foreground",
                subcategory.optimistic && "opacity-50",
              )}
            >
              <p className="flex space-x-2">
                <ChevronRight className="text-muted-foreground" />
                <span>{subcategory.name}</span>
              </p>
              <p>0.00 PLN</p>
            </li>
          ))}
          <CreateSubcategoryButton parentCategoryId={activeCategory.id} />
        </ul>
      )}
    </Card>
  );
};

export default SubcategoriesCard;
