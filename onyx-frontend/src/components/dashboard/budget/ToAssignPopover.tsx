import { FC, useCallback } from "react";
import { useNavigate, useParams } from "@tanstack/react-router";
import { useIsFetching, useQueryClient } from "@tanstack/react-query";

import { MIN_YEAR } from "@/lib/constants/date";
import MonthYearPicker, { DisableConfig } from "../MonthYearPicker";

interface ToAssignPopoverProps {
  selectedMonth: number;
  selectedYear: number;
}

const ToAssignPopover: FC<ToAssignPopoverProps> = ({
  selectedMonth,
  selectedYear,
}) => {
  const queryClient = useQueryClient();
  const { budgetId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId",
  });
  const navigate = useNavigate();
  const isFetching = useIsFetching({ queryKey: ["toAssign", budgetId] }) > 0;

  const isCurrentlySelectedYear = (year: number) => year === selectedYear;
  const handleSetNewDate = useCallback(
    (newMonth: number, newYear: number) => {
      if (isFetching) return;
      navigate({
        search: (prev) => ({
          ...prev,
          month: newMonth.toString(),
          year: newYear.toString(),
        }),
        mask: { to: `/budget/${budgetId}` },
      });
      setTimeout(() => {
        queryClient.invalidateQueries({ queryKey: ["toAssign", budgetId] });
      }, 0);
    },
    [budgetId, isFetching, navigate, queryClient],
  );

  const config: DisableConfig = {
    isDecreaseYearDisabled: (_selectedMonth, _selectedYear, localYear) =>
      MIN_YEAR >= localYear,
    isIncreaseYearDisabled: (_selectedMonth, _selectedYear, localYear) =>
      new Date().getFullYear() <= localYear,
    isUnselectable: (month: number, year: number) => {
      const maxMonth = new Date().getMonth() + 2;
      return month > maxMonth && isCurrentlySelectedYear(year);
    },
  };

  return (
    <MonthYearPicker
      onDateChange={handleSetNewDate}
      selectedMonth={selectedMonth}
      selectedYear={selectedYear}
      disableConfig={config}
    />
  );
};

export default ToAssignPopover;
