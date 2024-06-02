import { FC } from "react";

import { ChevronDown, ChevronUp, PlusIcon } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardTitle,
} from "@/components/ui/card";

interface BudgetAssigmentCardProps {}

const BudgetAssigmentCard: FC<BudgetAssigmentCardProps> = () => {
  return (
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
  );
};

export default BudgetAssigmentCard;
