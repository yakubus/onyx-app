import { VariantProps } from "class-variance-authority";
import { ButtonHTMLAttributes, FC } from "react";
import { Button, buttonVariants } from "./ui/button";
import { LoaderCircle } from "lucide-react";

interface LoadingButtonProps
  extends ButtonHTMLAttributes<HTMLButtonElement>,
    VariantProps<typeof buttonVariants> {
  isLoading: boolean;
}

const LoadingButton: FC<LoadingButtonProps> = ({
  isLoading,
  children,
  ...props
}) => {
  return (
    <Button {...props} disabled={isLoading}>
      {isLoading && (
        <LoaderCircle className="inline-flex size-6 flex-shrink-0 animate-spin" />
      )}
      <span className={isLoading ? "ml-2" : ""}>{children}</span>
    </Button>
  );
};

export default LoadingButton;
