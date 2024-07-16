import { FC, useMemo } from "react";
import { useIsFetching, useQueryClient } from "@tanstack/react-query";
import { useNavigate, useParams, useSearch } from "@tanstack/react-router";
import ToAssignPopover from "./ToAssignPopover";
import {
  Card,
  CardContent,
  CardDescription,
  CardTitle,
} from "@/components/ui/card";
import { Money } from "@/lib/validation/base";
import { cn, getFormattedCurrency } from "@/lib/utils";
import { getToAssignQueryKey } from "@/lib/api/budget";
import { type AvailableDates } from "@/components/dashboard/DatesMonthYearPicker";
import DatesMonthYearPickerButtons from "../DatesMonthYearPickerButtons";

interface BudgetAssignmentCardProps {
  toAssign: Money;
  availableDates: AvailableDates;
}

const BudgetAssignmentCard: FC<BudgetAssignmentCardProps> = ({
  toAssign,
  availableDates,
}) => {
  const queryClient = useQueryClient();
  const navigate = useNavigate();
  const { budgetId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId/",
  });
  const { amount, currency } = toAssign;
  const { month: selectedMonth, year: selectedYear } = useSearch({
    from: "/_dashboard-layout/budget/$budgetId/",
  });

  const isFetching = useIsFetching({ queryKey: ["toAssign", budgetId] }) > 0;

  const numericSearchParamsMonth = Number(selectedMonth);
  const numericSearchParamsYear = Number(selectedYear);

  const months = availableDates[numericSearchParamsYear] || [];
  const monthIndex = months.indexOf(numericSearchParamsMonth);

  const isMinMonth = useMemo(() => monthIndex === 0, [monthIndex]);
  const isMaxMonth = useMemo(
    () => monthIndex === months.length - 1,
    [monthIndex, months.length],
  );

  const handleMonthChange = async (newMonth: number) => {
    await navigate({
      search: (prev) => ({ ...prev, month: newMonth.toString() }),
      mask: { to: `/budget/${budgetId}` },
    });
    queryClient.refetchQueries({ queryKey: getToAssignQueryKey(budgetId) });
  };

  const handleDecreaseMonth = () => {
    if (isMinMonth || isFetching) return;
    handleMonthChange(months[monthIndex - 1]);
  };

  const handleIncreaseMonth = () => {
    if (isMaxMonth || isFetching) return;
    handleMonthChange(months[monthIndex + 1]);
  };

  return (
    <Card>
      <div className="flex items-center justify-between p-6">
        <div className="space-y-1.5">
          <CardTitle className="flex items-center justify-between">
            <ToAssignPopover
              selectedMonth={numericSearchParamsMonth}
              selectedYear={numericSearchParamsYear}
              availableDates={availableDates}
            />
          </CardTitle>
          <CardDescription>
            Select a month and display the amount to assign.
          </CardDescription>
        </div>
        <DatesMonthYearPickerButtons
          decreaseButtonDisabled={isFetching || isMinMonth}
          handleDecreaseMonth={handleDecreaseMonth}
          handleIncreaseMonth={handleIncreaseMonth}
          increaseButtonDisabled={isMaxMonth || isFetching}
        />
      </div>
      <CardContent>
        <div className="w-full space-x-4 rounded-lg bg-primary px-4 py-2 text-start text-primary-foreground">
          <p className="text-sm">TO ASSIGN:</p>
          <p
            className={cn(
              "text-center text-lg font-semibold tracking-wide",
              isFetching && "opacity-50",
            )}
          >
            {getFormattedCurrency(amount, currency)}
          </p>
        </div>
      </CardContent>
    </Card>
  );
};

export default BudgetAssignmentCard;
