import { FC } from "react";

import MonthYearPicker, {
  DisableConfig,
} from "@/components/dashboard/MonthYearPicker";
import { useFormContext } from "react-hook-form";
import { MIN_YEAR } from "@/lib/constants/date";

interface TargetCardFormDatePickerProps {
  searchMonth: string;
  searchYear: string;
}

const TargetCardFormDatePicker: FC<TargetCardFormDatePickerProps> = ({
  searchMonth,
  searchYear,
}) => {
  const { watch, setValue } = useFormContext();
  const localMonth = watch("month");
  const localYear = watch("year");

  const onDateChange = (month: number, year: number) => {
    const strSelectedMonth = month.toString();
    const strSelectedYear = year.toString();

    if (strSelectedMonth === localMonth && strSelectedYear === localYear)
      return;

    setValue("month", month.toString());
    setValue("year", year.toString());
  };

  const pickerConfig: DisableConfig = {
    isDecreaseYearDisabled: (_month, _year, localYear) => MIN_YEAR >= localYear,
    isUnselectable: (month, year) =>
      !(month > Number(searchMonth)) && year === Number(searchYear),
  };
  return (
    <MonthYearPicker
      selectedMonth={Number(localMonth)}
      selectedYear={Number(localYear)}
      onDateChange={onDateChange}
      disableConfig={pickerConfig}
    />
  );
};

export default TargetCardFormDatePicker;
