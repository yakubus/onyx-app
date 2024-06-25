import { FC, useCallback } from "react";
import { useNavigate, useParams } from "@tanstack/react-router";
import { useIsFetching, useQueryClient } from "@tanstack/react-query";

import MonthYearPicker, { DisableConfig } from "../MonthYearPicker";
import { type AvailableDates } from "@/routes/_dashboard-layout/budget.$budgetId/index.lazy";

interface ToAssignPopoverProps {
  selectedMonth: number;
  selectedYear: number;
  availableDates: AvailableDates;
}

const ToAssignPopover: FC<ToAssignPopoverProps> = ({
  selectedMonth,
  selectedYear,
  availableDates,
}) => {
  const queryClient = useQueryClient();
  const { budgetId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId/",
  });
  const navigate = useNavigate();
  const isFetching = useIsFetching({ queryKey: ["toAssign", budgetId] }) > 0;

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

  const availableYears = Object.keys(availableDates).map(Number);

  const config: DisableConfig = {
    isDecreaseYearDisabled: (_selectedMonth, _selectedYear, localYear) =>
      availableYears.indexOf(localYear) === 0,
    isIncreaseYearDisabled: (_selectedMonth, _selectedYear, localYear) =>
      availableYears.indexOf(localYear) === availableYears.length - 1,
    isUnselectable: (month: number, year: number) =>
      !availableDates[year] || !availableDates[year].includes(month),
  };

  return (
    <MonthYearPicker
      onDateChange={handleSetNewDate}
      selectedMonth={selectedMonth}
      selectedYear={selectedYear}
      disableConfig={config}
      availableYears={availableYears}
    />
  );
};

export default ToAssignPopover;
