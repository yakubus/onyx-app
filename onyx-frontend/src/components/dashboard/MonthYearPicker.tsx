import { FC, useMemo, useState } from "react";

import { SquareChevronLeft, SquareChevronRight } from "lucide-react";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import { Button } from "@/components/ui/button";

import { cn } from "@/lib/utils";
import { MONTHS } from "@/lib/constants/date";

export interface DisableConfig {
  isUnselectable?: (month: number, year: number) => boolean;
  isDecreaseYearDisabled?: (
    month: number,
    year: number,
    localYear: number,
    localMonth: number | null,
  ) => boolean;
  isIncreaseYearDisabled?: (
    month: number,
    year: number,
    localYear: number,
    localMonth: number | null,
  ) => boolean;
}

interface MonthYearPickerProps {
  selectedMonth: number;
  selectedYear: number;
  onDateChange: (newMonth: number, newYear: number) => void;
  disableConfig?: DisableConfig;
  center?: boolean;
}

const MonthYearPicker: FC<MonthYearPickerProps> = ({
  selectedMonth,
  selectedYear,
  onDateChange,
  disableConfig = {},
  center,
}) => {
  const [localMonth, setLocalMonth] = useState<number | null>(null);
  const [localYear, setLocalYear] = useState(selectedYear);
  const [isOpen, setIsOpen] = useState(false);
  const { isUnselectable, isDecreaseYearDisabled, isIncreaseYearDisabled } =
    disableConfig;

  const isCurrentlySelectedYear = (year: number) => year === selectedYear;
  const isCurrentlySelectedMonth = (month: number, year: number) =>
    selectedMonth === month && isCurrentlySelectedYear(year);

  const handleMonthButton = (monthNumber: number, year: number) => {
    if (
      isCurrentlySelectedMonth(monthNumber, year) ||
      (isUnselectable && isUnselectable(monthNumber, year))
    )
      return;
    setLocalMonth(monthNumber);
  };

  const handleYearChange = (direction: "increase" | "decrease") => {
    setLocalYear((prevYear) => {
      if (direction === "increase") {
        if (
          isIncreaseYearDisabled &&
          isIncreaseYearDisabled(
            selectedMonth,
            selectedYear,
            localYear,
            localMonth,
          )
        ) {
          return prevYear;
        }
        return prevYear + 1;
      } else {
        if (
          isDecreaseYearDisabled &&
          isDecreaseYearDisabled(
            selectedMonth,
            selectedYear,
            localYear,
            localMonth,
          )
        ) {
          return prevYear;
        }
        return prevYear - 1;
      }
    });
  };

  const handleDateChange = () => {
    if (localMonth && localYear) {
      onDateChange(localMonth, localYear);
      setIsOpen(false);
    }
  };

  const localMonthName = useMemo(
    () => (localMonth ? MONTHS[localMonth - 1] : ""),
    [localMonth],
  );

  return (
    <Popover open={isOpen} onOpenChange={setIsOpen}>
      <PopoverTrigger
        className={cn(
          "flex h-full w-full items-center",
          center && "justify-center",
        )}
      >
        {MONTHS[Number(selectedMonth) - 1]} {selectedYear}
      </PopoverTrigger>
      <PopoverContent align="start">
        <div className="flex items-center justify-between py-4">
          <button
            disabled={
              isDecreaseYearDisabled &&
              isDecreaseYearDisabled(
                selectedMonth,
                selectedYear,
                localYear,
                localMonth,
              )
            }
            onClick={() => handleYearChange("decrease")}
            className="h-fit w-fit p-0 opacity-70 transition-all hover:opacity-100 disabled:pointer-events-none disabled:opacity-50"
          >
            <SquareChevronLeft />
          </button>
          <p className="text-lg font-bold">{localYear}</p>
          <button
            disabled={
              isIncreaseYearDisabled &&
              isIncreaseYearDisabled(
                selectedMonth,
                selectedYear,
                localYear,
                localMonth,
              )
            }
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
              onClick={() => handleMonthButton(i + 1, localYear)}
              className={cn(
                "w-full",
                isCurrentlySelectedMonth(i + 1, localYear) &&
                  "bg-secondary text-secondary-foreground",
                localMonth === i + 1 &&
                  "bg-primary text-primary-foreground hover:bg-primary hover:text-primary-foreground",
              )}
              variant="outline"
              disabled={
                isCurrentlySelectedMonth(i + 1, localYear) ||
                (isUnselectable && isUnselectable(i + 1, localYear))
              }
            >
              {month}
            </Button>
          ))}
        </div>
        {localMonth && localYear && (
          <Button className="mt-4 w-full" onClick={handleDateChange}>
            Set to: {localMonthName} {localYear}
          </Button>
        )}
      </PopoverContent>
    </Popover>
  );
};

export default MonthYearPicker;
