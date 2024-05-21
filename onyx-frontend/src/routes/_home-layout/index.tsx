import { Button } from "@/components/ui/button";
import { Link, createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_home-layout/")({
  component: () => (
    <div className="p-10">
      <Button asChild>
        <Link to="/budget">Get Started</Link>
      </Button>
    </div>
  ),
});
