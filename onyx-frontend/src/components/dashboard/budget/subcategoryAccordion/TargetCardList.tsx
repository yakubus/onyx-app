import { FC } from "react";

import { Button } from "@/components/ui/button";

import { Target } from "@/lib/validation/base";
import { MONTHS } from "@/lib/constants/date";
import { cn, formatAmount } from "@/lib/utils";

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
            {formatAmount(targetAmount.amount.toString())} {currencyToDisplay}
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
            {formatAmount(collectedAmount.amount.toString())}{" "}
            {collectedAmount.currency.toString()}
          </span>
        </li>
        <li className="space-x-4 border-b-2 border-primary py-1">
          <span>Assigned every month:</span>
          <span className="font-semibold">
            {formatAmount(amountAssignedEveryMonth.amount.toString())}{" "}
            {amountAssignedEveryMonth.currency.toString()}
          </span>
        </li>
      </ul>
      <Button
        variant="outline"
        className="mt-4 w-full"
        onClick={() => setIsCreating(true)}
      >
        Edit
      </Button>
    </>
  );
};

export default TargetCardList;
