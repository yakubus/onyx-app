import { FC } from "react";

import CreateSubcategoryButton from "@/components/dashboard/budget/CreateSubcategoryButton";
import {
  Accordion,
  AccordionContent,
  AccordionItem,
  AccordionTrigger,
} from "@/components/ui/accordion";
import { Card } from "@/components/ui/card";

import { Category } from "@/lib/validation/category";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";

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
        <Accordion type="single" collapsible>
          {activeCategory.subcategories.map((subcategory) => (
            <AccordionItem
              value={subcategory.id}
              disabled={subcategory.optimistic}
            >
              <AccordionTrigger className="space-x-5 pl-3 pr-4">
                <div className="flex flex-1 justify-between">
                  <span>{subcategory.name}</span>
                  <span>
                    {subcategory.assigments?.reduce(
                      (a, c) => (a += c.assignedAmount.amount),
                      0,
                    ) || 0}
                  </span>
                </div>
              </AccordionTrigger>
              <AccordionContent className="space-y-6 border-t pb-4 pl-3 pr-4 pt-2">
                <div className="space-x-2">
                  <Button size="sm" variant="outline">
                    Edit name
                  </Button>
                  <Button size="sm" variant="outline">
                    Add description
                  </Button>
                  <DropdownMenu>
                    <DropdownMenuTrigger asChild>
                      <Button size="sm" variant="outline">
                        Assign Options
                      </Button>
                    </DropdownMenuTrigger>
                    <DropdownMenuContent>
                      <DropdownMenuItem>Assign</DropdownMenuItem>
                      <DropdownMenuItem>Unassign</DropdownMenuItem>
                      <DropdownMenuItem>Reassign</DropdownMenuItem>
                    </DropdownMenuContent>
                  </DropdownMenu>
                  <Button size="sm" variant="outline">
                    Transact
                  </Button>
                </div>
                <div className="flex space-x-4">
                  <Card className="w-full">
                    <div className="flex justify-between rounded-t-md bg-primary px-4 py-1 text-primary-foreground">
                      <p>Target</p>
                      <p>0.00 PLN</p>
                    </div>
                    <div className="flex items-center justify-center p-6">
                      <Button>Add Target</Button>
                    </div>
                  </Card>
                  <Card className="w-full">
                    <div className="flex justify-between rounded-t-md bg-primary px-4 py-1 text-primary-foreground">
                      <p>Statistics</p>
                      <p>0.00 PLN</p>
                    </div>
                    <div className="flex items-center justify-center p-6">
                      <span>Table</span>
                    </div>
                  </Card>
                </div>
              </AccordionContent>
            </AccordionItem>
          ))}
          <CreateSubcategoryButton parentCategoryId={activeCategory.id} />
        </Accordion>
      )}
    </Card>
  );
};

export default SubcategoriesCard;
