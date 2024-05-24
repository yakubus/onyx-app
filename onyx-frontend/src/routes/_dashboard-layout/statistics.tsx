import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_dashboard-layout/statistics")({
  component: () => <div>Hello /_dashboard-layout/statistics!</div>,
});
