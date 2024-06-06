import { FC } from "react";

import SelectCategoryButton from "@/components/dashboard/budget/selectCategoryButton/SelectCategoryButton";
import AddCategoryButton from "@/components/dashboard/budget/CreateCategoryButton";
import { ScrollArea } from "@/components/ui/scroll-area";

import { Category } from "@/lib/validation/category";

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
  const onSelect = (categoryId: string, optimistic?: boolean) => {
    if (optimistic) return;
    setActiveCategoryId(categoryId);
  };

  return (
    <ScrollArea className="h-full flex-grow rounded-lg border bg-card">
      <h2 className="sticky top-0 z-10 border-b bg-card p-6 text-center text-2xl font-semibold">
        Categories
      </h2>
      <ul className="space-y-4 p-6">
        {categories.map((category) => (
          <SelectCategoryButton
            key={category.id}
            activeCategoryId={activeCategoryId}
            category={category}
            onSelect={() => onSelect(category.id, category.optimistic)}
          />
        ))}
        <AddCategoryButton categoriesCount={categories.length} />
      </ul>
    </ScrollArea>
  );
};

export default CategoriesCard;
