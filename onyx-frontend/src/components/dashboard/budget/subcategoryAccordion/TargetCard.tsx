import { FC, useState } from "react";

import TargetCardList from "./TargetCardList";
import TargetCardForm from "./TargetCardForm";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";

import { Subcategory } from "@/lib/validation/subcategory";

interface TargetCardProps {
  subcategory: Subcategory;
  currencyToDisplay: string;
}

const TargetCard: FC<TargetCardProps> = ({
  subcategory,
  currencyToDisplay,
}) => {
  const [isCreating, setIsCreating] = useState(false);

  const currentTarget = subcategory.target;

  return (
    <Card className="overflow-hidden rounded-lg border">
      <CardHeader className="border-b py-4">
        <CardTitle className="text-center text-lg">Target</CardTitle>
      </CardHeader>
      <CardContent>
        {currentTarget && !isCreating && (
          <TargetCardList
            currencyToDisplay={currencyToDisplay}
            currentTarget={currentTarget}
            setIsCreating={setIsCreating}
          />
        )}
        {isCreating || !currentTarget ? (
          <TargetCardForm
            currentTarget={currentTarget}
            setIsCreating={setIsCreating}
            subcategoryId={subcategory.id}
          />
        ) : null}
      </CardContent>
    </Card>
  );
};

export default TargetCard;
