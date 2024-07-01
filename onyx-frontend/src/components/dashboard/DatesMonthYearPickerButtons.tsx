import { FC } from "react";

import { ChevronDown, ChevronUp } from "lucide-react";
import { Button } from "@/components/ui/button";

interface DatesMonthYearPickerButtonsProps {
  handleIncreaseMonth: () => void;
  increaseButtonDisabled: boolean;
  handleDecreaseMonth: () => void;
  decreaseButtonDisabled: boolean;
}

const DatesMonthYearPickerButtons: FC<DatesMonthYearPickerButtonsProps> = ({
  decreaseButtonDisabled,
  increaseButtonDisabled,
  handleIncreaseMonth,
  handleDecreaseMonth,
}) => {
  return (
    <div className="flex flex-col">
      <Button
        onClick={handleIncreaseMonth}
        disabled={increaseButtonDisabled}
        size="icon"
        variant="ghost"
        className="h-8 w-8"
      >
        <ChevronUp className="size-6" />
      </Button>
      <Button
        onClick={handleDecreaseMonth}
        disabled={decreaseButtonDisabled}
        size="icon"
        variant="ghost"
        className="h-8 w-8"
      >
        <ChevronDown className="size-6" />
      </Button>
    </div>
  );
};

export default DatesMonthYearPickerButtons;
