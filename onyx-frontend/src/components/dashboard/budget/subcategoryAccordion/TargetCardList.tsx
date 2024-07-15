import { FC } from "react";

import { Button } from "@/components/ui/button";

import { Target } from "@/lib/validation/base";
import { MONTHS } from "@/lib/constants/date";
import { cn, getFormattedCurrency } from "@/lib/utils";

interface TargetCardListProps {
  currentTarget: Target;
  currencyToDisplay: string;
  setIsCreating: (state: boolean) => void;
}

const TargetCardList: FC<TargetCardListProps> = ({
  currentTarget,
  currencyToDisplay,
  setIsCreating,
}) => {
  const {
    optimistic,
    targetAmount,
    startedAt,
    upToMonth,
    collectedAmount,
    amountAssignedEveryMonth,
  } = currentTarget;
  return (
    <>
      <ul className={cn("space-y-1 py-4", optimistic && "opacity-70")}>
        <li className="space-x-4 border-b-2 border-primary py-1">
          <span>Amount:</span>
          <span className="font-semibold">
            {getFormattedCurrency(targetAmount.amount, currencyToDisplay)}
          </span>
        </li>
        <li className="space-x-4 border-b-2 border-primary py-1">
          <span>Started at:</span>
          <span className="font-semibold">
            {MONTHS[startedAt.month - 1]} {startedAt.year.toString()}
          </span>
        </li>
        <li className="space-x-4 border-b-2 border-primary py-1">
          <span>Up to:</span>
          <span className="font-semibold">
            {MONTHS[upToMonth.month - 1]} {upToMonth.year.toString()}
          </span>
        </li>
        <li className="space-x-4 border-b-2 border-primary py-1">
          <span>Collected:</span>
          <span className="font-semibold">
            {getFormattedCurrency(collectedAmount.amount, currencyToDisplay)}
          </span>
        </li>
        <li className="space-x-4 border-b-2 border-primary py-1">
          <span>Assigned every month:</span>
          <span className="font-semibold">
            {getFormattedCurrency(
              amountAssignedEveryMonth.amount,
              currencyToDisplay,
            )}
          </span>
        </li>
      </ul>
      <Button
        variant="outline"
        className="mt-4 w-full"
        onClick={() => setIsCreating(true)}
      >
        Change
      </Button>
    </>
  );
};

export default TargetCardList;
