import { FC, useCallback } from "react";
import { useNavigate, useParams } from "@tanstack/react-router";
import { useIsFetching, useQueryClient } from "@tanstack/react-query";

import DatesMonthYearPicker, {
  AvailableDates,
} from "@/components/dashboard/DatesMonthYearPicker";
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

  return (
    <DatesMonthYearPicker
      availableDates={availableDates}
      handleSetNewDate={handleSetNewDate}
      selectedMonth={selectedMonth}
      selectedYear={selectedYear}
    />
  );
};

export default ToAssignPopover;
