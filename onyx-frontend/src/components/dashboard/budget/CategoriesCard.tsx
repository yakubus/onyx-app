import { FC } from "react";
import { useMutationState } from "@tanstack/react-query";

import { ChevronDown } from "lucide-react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import SelectCategoryButton from "./selectCategoryButton/SelectCategoryButton";
import AddCategoryButton from "./CreateCategoryButton";

import { Category } from "@/lib/validation/category";
import { capitalize } from "@/lib/utils";

interface CategoriesCardProps {
  categories: Category[];
  activeCategoryId: string;
  setActiveCategoryId: (id: string) => void;
}

const CategoriesCard: FC<CategoriesCardProps> = ({
  categories,
  activeCategoryId,
  setActiveCategoryId,
}) => {
  const optimisticallyAddedCategory = useMutationState({
    filters: { mutationKey: ["createCategory"], status: "pending" },
    select: (mutation) => mutation.state.variables,
  });
  return (
    <Card className="h-full flex-grow overflow-y-auto scrollbar-none">
      <CardHeader className="sticky top-0 z-10 border-b bg-card text-center">
        <CardTitle>Categories</CardTitle>
      </CardHeader>
      <CardContent>
        <ul className="space-y-4 py-6">
          {categories.map((category) => (
            <SelectCategoryButton
              key={category.id}
              activeCategoryId={activeCategoryId}
              category={category}
              onSelect={() => setActiveCategoryId(category.id)}
            />
          ))}
          {optimisticallyAddedCategory.length > 0 && (
            <li className="cursor-pointer rounded-lg border border-input opacity-50 hover:bg-accent hover:text-accent-foreground">
              <div className="flex h-16 w-full items-center px-6">
                <div className="flex flex-1 items-center truncate">
                  <p className="w-full truncate pr-6">
                    {capitalize(optimisticallyAddedCategory[0] as string)}
                  </p>
                </div>
                <ChevronDown className="block flex-shrink-0 -rotate-90 text-muted-foreground transition-all duration-200 ease-in-out lg:rotate-0" />
              </div>
            </li>
          )}
          <AddCategoryButton categoriesCount={categories.length} />
        </ul>
      </CardContent>
    </Card>
  );
};

export default CategoriesCard;
