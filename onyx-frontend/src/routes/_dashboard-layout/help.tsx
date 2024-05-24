import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/_dashboard-layout/help")({
  component: () => <div>Hello /_dashboard-layout/help!</div>,
});
