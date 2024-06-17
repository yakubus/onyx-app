import { FC } from "react";
import { useMutationState } from "@tanstack/react-query";

import { Button } from "@/components/ui/button";

import { Target } from "@/lib/validation/base";
import { MONTHS } from "@/lib/constants/date";
import { CreateTargetForm } from "@/lib/api/subcategory";

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
  const variables = useMutationState<CreateTargetForm>({
    filters: { mutationKey: ["createTarget"], status: "pending" },
    select: (mutation) => mutation.state.variables as CreateTargetForm,
  });

  return (
    <>
      <ul className="space-y-1 border-b py-4">
        <li className="space-x-4">
          <span>Amount:</span>
          <span className="font-semibold">
            {currentTarget.targetAmount.amount.toString()} {currencyToDisplay}
          </span>
        </li>
        <li className="space-x-4">
          <span>Started at:</span>
          <span className="font-semibold">
            {MONTHS[currentTarget.startedAt.month]}{" "}
            {currentTarget.startedAt.year.toString()}
          </span>
        </li>
        <li className="space-x-4">
          <span>Up to:</span>
          <span className="font-semibold">
            {MONTHS[currentTarget.upToMonth.month]}{" "}
            {currentTarget.upToMonth.year.toString()}
          </span>
        </li>
        <li className="space-x-4">
          <span>Collected:</span>
          <span className="font-semibold">
            {currentTarget.collectedAmount.amount.toString()}{" "}
            {currentTarget.collectedAmount.currency.toString()}
          </span>
        </li>
        <li className="space-x-4">
          <span>Assigned every month:</span>
          <span className="font-semibold">
            {currentTarget.amountAssignedEveryMonth.amount.toString()}{" "}
            {currentTarget.amountAssignedEveryMonth.currency.toString()}
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
