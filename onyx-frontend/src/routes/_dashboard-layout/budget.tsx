import { useState } from "react";
import { useSuspenseQuery } from "@tanstack/react-query";
import { createFileRoute } from "@tanstack/react-router";

import { cn } from "@/lib/utils";
import { getCategoriesQueryOptions } from "@/lib/api";

import { ChevronDown, ChevronRight, ChevronUp, PlusIcon } from "lucide-react";
import AddCategoryButton from "@/components/dashboard/AddCategoryButton";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Button } from "@/components/ui/button";

export const Route = createFileRoute("/_dashboard-layout/budget")({
  component: Budget,
  loader: ({ context: { queryClient } }) =>
    queryClient.ensureQueryData(getCategoriesQueryOptions),
});

function Budget() {
  const [activeCategoryId, setActiveCategoryId] = useState("");

  const categoriesQuery = useSuspenseQuery(getCategoriesQueryOptions);
  const categories = categoriesQuery.data;
  const subcategories = categories.find(
    (category) => category.id === activeCategoryId,
  )?.subcategories;

  console.log(activeCategoryId);

  return (
    <div className="grid h-full grid-cols-1 gap-2 rounded-md p-2 lg:grid-cols-5">
      <div className="flex flex-col space-y-4 lg:col-span-2">
        <Card>
          <div className="flex items-center justify-between p-6">
            <div className="space-y-1.5">
              <CardTitle className="flex items-center justify-between">
                May 2024
              </CardTitle>
              <CardDescription>
                Set amount for current or next month.
              </CardDescription>
            </div>
            <div className="flex flex-col">
              <Button size="icon" variant="ghost" className="h-8 w-8">
                <ChevronUp className="size-6" />
              </Button>
              <Button size="icon" variant="ghost" className="h-8 w-8">
                <ChevronDown className="size-6" />
              </Button>
            </div>
          </div>
          <CardContent>
            <Button className="flex h-20 w-full items-center justify-between text-start">
              <span className="space-y-1">
                <span className="block text-xs font-light">TO ASSIGN</span>
                <span className="block text-xl">2500.00 PLN</span>
              </span>
              <PlusIcon className="h-6 w-6 rounded-full border" />
            </Button>
          </CardContent>
        </Card>
        <Card className="flex-grow">
          <CardHeader className="border-b text-center">
            <CardTitle>Categories</CardTitle>
          </CardHeader>
          <CardContent className="flex flex-col space-y-4 py-8">
            {categories.map((category) => (
              <Button
                key={category.id}
                size="lg"
                variant="outline"
                className={cn(
                  "h-14 justify-between",
                  activeCategoryId === category.id &&
                    "bg-primary text-primary-foreground hover:bg-primary hover:text-primary-foreground",
                )}
                onClick={() => setActiveCategoryId(category.id)}
              >
                <span className="text-base">{category.name}</span>
                <ChevronDown
                  className={cn(
                    "-rotate-90 text-muted-foreground transition-all duration-200 ease-in-out lg:rotate-0",
                    activeCategoryId === category.id &&
                      "rotate-0 text-primary-foreground lg:-rotate-90",
                  )}
                />
              </Button>
            ))}
            <AddCategoryButton categoriesCount={categories.length} />
          </CardContent>
        </Card>
      </div>
      {activeCategoryId && (
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
      )}
    </div>
  );
}
