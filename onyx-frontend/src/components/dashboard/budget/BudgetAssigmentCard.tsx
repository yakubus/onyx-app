import { FC, useMemo } from "react";
import { useIsFetching, useQueryClient } from "@tanstack/react-query";
import { useNavigate, useParams, useSearch } from "@tanstack/react-router";

import { ChevronDown, ChevronUp } from "lucide-react";
import ToAssignPopover from "./ToAssignPopover";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardTitle,
} from "@/components/ui/card";

import { Money } from "@/lib/validation/base";
import { cn } from "@/lib/utils";

interface BudgetAssignmentCardProps {
  toAssign: Money;
}

const BudgetAssignmentCard: FC<BudgetAssignmentCardProps> = ({ toAssign }) => {
  const queryClient = useQueryClient();
  const navigate = useNavigate();
  const { budgetId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId",
  });
  const { amount, currency } = toAssign;
  const { month: selectedMonth, year: selectedYear } = useSearch({
    from: "/_dashboard-layout/budget/$budgetId",
  });

  const isFetching = useIsFetching({ queryKey: ["toAssign", budgetId] }) > 0;

  const numericSearchParamsMonth = Number(selectedMonth);
  const numericSearchParamsYear = Number(selectedYear);

  const isMinMonth = useMemo(
    () => numericSearchParamsMonth <= 1,
    [numericSearchParamsMonth],
  );
  const isMaxMonth = useMemo(
    () => numericSearchParamsMonth >= new Date().getMonth() + 2,
    [numericSearchParamsMonth],
  );

  const handleMonthChange = (newMonth: number) => {
    navigate({
      search: (prev) => ({ ...prev, month: newMonth.toString() }),
      mask: { to: `/budget/${budgetId}` },
    });
    setTimeout(() => {
      queryClient.invalidateQueries({ queryKey: ["toAssign", budgetId] });
    }, 0);
  };

  const handleDecreaseMonth = () => {
    if (isMinMonth || isFetching) return;
    handleMonthChange(numericSearchParamsMonth - 1);
  };

  const handleIncreaseMonth = () => {
    if (isMaxMonth || isFetching) return;
    handleMonthChange(numericSearchParamsMonth + 1);
  };

  return (
    <Card>
      <div className="flex items-center justify-between p-6">
        <div className="space-y-1.5">
          <CardTitle className="flex items-center justify-between">
            <ToAssignPopover
              selectedMonth={numericSearchParamsMonth}
              selectedYear={numericSearchParamsYear}
            />
          </CardTitle>
          <CardDescription>
            Select a month and display the amount to assign.
          </CardDescription>
        </div>
        <div className="flex flex-col">
          <Button
            onClick={handleIncreaseMonth}
            disabled={isMaxMonth || isFetching}
            size="icon"
            variant="ghost"
            className="h-8 w-8"
          >
            <ChevronUp className="size-6" />
          </Button>
          <Button
            onClick={handleDecreaseMonth}
            disabled={isMinMonth || isFetching}
            size="icon"
            variant="ghost"
            className="h-8 w-8"
          >
            <ChevronDown className="size-6" />
          </Button>
        </div>
      </div>
      <CardContent>
        <div className="w-full space-x-4 rounded-lg bg-primary px-4 py-2 text-start text-primary-foreground">
          <p className="text-sm">TO ASSIGN:</p>
          <p
            className={cn(
              "text-center text-lg font-semibold",
              isFetching && "opacity-50",
            )}
          >
            {amount} {currency}
          </p>
        </div>
      </CardContent>
    </Card>
  );
};

export default BudgetAssignmentCard;
