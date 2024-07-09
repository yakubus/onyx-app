import { FC, useMemo } from "react";
import { useNavigate, useParams, useSearch } from "@tanstack/react-router";

import DatesMonthYearPicker from "@/components/dashboard/DatesMonthYearPicker";
import DatesMonthYearPickerButtons from "@/components/dashboard/DatesMonthYearPickerButtons";

interface AccountCardDatePickerProps {
  availableDates: { [key: number]: number[] };
}

const AccountCardDatePicker: FC<AccountCardDatePickerProps> = ({
  availableDates,
}) => {
  const navigate = useNavigate();
  const { budgetId, accountId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId/accounts/$accountId",
  });
  const { accMonth, accYear } = useSearch({
    from: "/_dashboard-layout/budget/$budgetId/accounts/$accountId",
  });

  const numericSearchParamsAccMonth = Number(accMonth);
  const numericSearchParamsAccYear = Number(accYear);

  const months = availableDates[numericSearchParamsAccYear] || [];
  const monthIndex = months.indexOf(numericSearchParamsAccMonth);

  const isMinMonth = useMemo(() => monthIndex === 0, [monthIndex]);
  const isMaxMonth = useMemo(
    () => monthIndex === months.length - 1,
    [monthIndex, months.length],
  );

  const handleMonthChange = async (newMonth: number) => {
    await navigate({
      search: (prev) => ({ ...prev, accMonth: newMonth.toString() }),
      params: { accountId, budgetId },
      mask: { to: `/budget/${budgetId}/accounts/${accountId}` },
    });
  };

  const handleDecreaseMonth = () => {
    if (isMinMonth) return;
    handleMonthChange(months[monthIndex - 1]);
  };

  const handleIncreaseMonth = () => {
    if (isMaxMonth) return;
    handleMonthChange(months[monthIndex + 1]);
  };

  return (
    <div className="flex items-center text-2xl font-semibold leading-none tracking-tight">
      <div className="w-full">
        <DatesMonthYearPicker
          availableDates={availableDates}
          selectedMonth={numericSearchParamsAccMonth}
          selectedYear={numericSearchParamsAccYear}
          handleSetNewDate={handleMonthChange}
        />
        <p className="pt-2 text-sm font-thin tracking-normal text-muted-foreground">
          Select a month and track your expenses.
        </p>
      </div>
      <DatesMonthYearPickerButtons
        decreaseButtonDisabled={isMinMonth}
        increaseButtonDisabled={isMaxMonth}
        handleDecreaseMonth={handleDecreaseMonth}
        handleIncreaseMonth={handleIncreaseMonth}
      />
    </div>
  );
};

export default AccountCardDatePicker;
