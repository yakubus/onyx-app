import { FC, useMemo, useState } from "react";
import { useNavigate, useParams } from "@tanstack/react-router";
import { useIsFetching, useQueryClient } from "@tanstack/react-query";

import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import { Button } from "@/components/ui/button";

import { cn } from "@/lib/utils";
import { MIN_YEAR, MONTHS } from "@/lib/constants/date";
import { SquareChevronLeft, SquareChevronRight } from "lucide-react";

interface ToAssignPopoverProps {
  selectedMonth: number;
  selectedYear: number;
}

const ToAssignPopover: FC<ToAssignPopoverProps> = ({
  selectedMonth,
  selectedYear,
}) => {
  const [localMonth, setLocalMonth] = useState<number | null>(null);
  const [localYear, setLocalYear] = useState(selectedYear);
  const [isOpen, setIsOpen] = useState(false);
  const queryClient = useQueryClient();
  const { budgetId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId",
  });
  const navigate = useNavigate();
  const isFetching = useIsFetching({ queryKey: ["toAssign", budgetId] }) > 0;

  const isCurrentlySelectedYear = (year: number) => year === selectedYear;
  const isCurrentlySelectedMonth = (month: number, year: number) =>
    selectedMonth === month && isCurrentlySelectedYear(year);
  const isUnselectable = (month: number, year: number) => {
    const maxMonthNumber = new Date().getMonth() + 2;
    return month > maxMonthNumber && isCurrentlySelectedYear(year);
  };

  const handleMonthButton = (monthNumber: number, year: number) => {
    if (
      isCurrentlySelectedMonth(monthNumber, year) ||
      isUnselectable(monthNumber, year)
    )
      return;
    setLocalMonth(monthNumber);
  };

  const handleSetNewDate = async (newMonth: string, newYear: string) => {
    if (isFetching) return;
    navigate({
      search: (prev) => ({ ...prev, month: newMonth, year: newYear }),
      mask: { to: `/budget/${budgetId}` },
    });
    setTimeout(() => {
      queryClient.invalidateQueries({ queryKey: ["toAssign", budgetId] });
    }, 0);
    setIsOpen(false);
  };

  const handleYearChange = (direction: "increase" | "decrease") => {
    setLocalYear((prevYear) =>
      direction === "increase" ? prevYear + 1 : prevYear - 1,
    );
  };

  const localMonthName = useMemo(
    () => (localMonth ? MONTHS[localMonth - 1] : ""),
    [localMonth],
  );

  return (
    <Popover open={isOpen} onOpenChange={setIsOpen}>
      <PopoverTrigger>
        {MONTHS[Number(selectedMonth) - 1]} {selectedYear}
      </PopoverTrigger>
      <PopoverContent align="start">
        <div className="flex items-center justify-between py-4">
          <button
            disabled={MIN_YEAR >= localYear}
            onClick={() => handleYearChange("decrease")}
            className="h-fit w-fit p-0 opacity-70 transition-all hover:opacity-100 disabled:pointer-events-none disabled:opacity-50"
          >
            <SquareChevronLeft />
          </button>
          <p className="text-lg font-bold">{localYear}</p>
          <button
            disabled={new Date().getFullYear() <= localYear}
            onClick={() => handleYearChange("increase")}
            className="h-fit w-fit p-0 opacity-70 transition-all hover:opacity-100 disabled:pointer-events-none disabled:opacity-50"
          >
            <SquareChevronRight />
          </button>
        </div>
        <div className="grid grid-cols-3 gap-1">
          {MONTHS.map((month, i) => (
            <Button
              key={i}
              onClick={() => handleMonthButton(i + 1, selectedYear)}
              className={cn(
                "w-full",
                isCurrentlySelectedMonth(i + 1, selectedYear) &&
                  "bg-secondary text-secondary-foreground",
                localMonth === i + 1 &&
                  "bg-primary text-primary-foreground hover:bg-primary hover:text-primary-foreground",
              )}
              variant="outline"
              disabled={
                isCurrentlySelectedMonth(i + 1, selectedYear) ||
                isUnselectable(i + 1, selectedYear)
              }
            >
              {month}
            </Button>
          ))}
        </div>
        {localMonth && localYear && (
          <Button
            className="mt-4 w-full"
            disabled={isFetching}
            onClick={() =>
              handleSetNewDate(localMonth.toString(), localYear.toString())
            }
          >
            Set to: {localMonthName} {localYear}
          </Button>
        )}
      </PopoverContent>
    </Popover>
  );
};

export default ToAssignPopover;
