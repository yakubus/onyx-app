import { FC } from "react";
import { useRouter } from "@tanstack/react-router";

import {
  Card,
  CardContent,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Button } from "@/components/ui/button";

interface RouteLoadingErrorProps {
  reset: () => void;
}

const RouteLoadingError: FC<RouteLoadingErrorProps> = ({ reset }) => {
  const router = useRouter();

  const handleRetry = () => {
    reset();
    router.invalidate();
  };
  return (
    <div className="flex justify-center pt-28 md:pt-40">
      <Card className="relative mx-4 w-full max-w-[450px]">
        <CardHeader>
          <CardTitle className="text-center">
            Oops, an error occurred.
          </CardTitle>
        </CardHeader>
        <CardContent>
          <p>
            We are terribly sorry, but it seems we have some problems loading
            your content. Please try again or come back later.
          </p>
        </CardContent>
        <CardFooter>
          <Button onClick={handleRetry} className="w-full" variant="outline">
            Try again
          </Button>
        </CardFooter>
      </Card>
    </div>
  );
};

export default RouteLoadingError;
