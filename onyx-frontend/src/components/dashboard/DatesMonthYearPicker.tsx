import { FC } from "react";

import MonthYearPicker, {
  DisableConfig,
} from "@/components/dashboard/MonthYearPicker";

export interface AvailableDates {
  [key: number]: number[];
}

interface DatesMonthYearPickerProps {
  selectedMonth: number;
  selectedYear: number;
  availableDates: AvailableDates;
  handleSetNewDate: (newMonth: number, newYear: number) => void;
}

const DatesMonthYearPicker: FC<DatesMonthYearPickerProps> = ({
  availableDates,
  selectedMonth,
  selectedYear,
  handleSetNewDate,
}) => {
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

export default DatesMonthYearPicker;
