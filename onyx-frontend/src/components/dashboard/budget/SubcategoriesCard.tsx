import { FC } from "react";

import { ChevronRight } from "lucide-react";
import { Card } from "@/components/ui/card";

import { Subcategory } from "@/lib/validation/subcategory";

interface SubcategoriesCardProps {
  subcategories: Subcategory[] | undefined;
}

const SubcategoriesCard: FC<SubcategoriesCardProps> = ({ subcategories }) => {
  return (
    <Card className="lg:col-span-3">
      <div className="flex justify-between rounded-t-md bg-primary px-4 py-1 text-primary-foreground">
        <p>Subcategory</p>
        <p>Assigned</p>
      </div>
      {subcategories?.length && (
        <ul className="p-1">
          {subcategories.map((subcategory) => (
            <li
              key={subcategory.id}
              className="flex cursor-pointer items-center justify-between py-3 pl-2 pr-4 transition-all duration-200 hover:bg-accent hover:text-accent-foreground"
            >
              <p className="flex space-x-2">
                <ChevronRight className="text-muted-foreground" />
                <span>{subcategory.name}</span>
              </p>
              <p>0.00 PLN</p>
            </li>
          ))}
        </ul>
      )}
    </Card>
  );
};

export default SubcategoriesCard;
