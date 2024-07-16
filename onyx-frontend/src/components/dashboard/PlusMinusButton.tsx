import { FC } from "react";

import { Minus, Plus } from "lucide-react";
import { Button } from "@/components/ui/button";
import { cn } from "@/lib/utils";

interface PlusMinusButtonProps {
  state: "+" | "-";
  setState: (state: "+" | "-") => void;
}

const PlusMinusButton: FC<PlusMinusButtonProps> = ({ state, setState }) => {
  const handleClick = () => {
    setState(state === "+" ? "-" : "+");
  };

  return (
    <Button
      type="button"
      onClick={handleClick}
      variant="outline"
      size="icon"
      className={cn(
        state === "+"
          ? "text-primary hover:text-primary"
          : "text-destructive hover:text-destructive",
      )}
    >
      {state === "+" ? <Plus /> : <Minus />}
    </Button>
  );
};

export default PlusMinusButton;
